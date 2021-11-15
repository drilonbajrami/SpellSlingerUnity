using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class HandEngine_Client : MonoBehaviour
{
    //Definintions
    private const int DISCONNECTED = 0;
    private const int CONNECTED = 1;

    private int previousPacketCount = 0;
    private int currentPacketCount = 0;

    private string currentMessage;

    //TCP connection details
    public string host = "127.0.0.1";
    public int Port = 9000;
    public bool Connect = false; //Use as a push button input to connect/disconnect TCP connection     
    private int State = DISCONNECTED; //Current state of TCP connection
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private string messageBuffer = "";

    //Asset details
    public GameObject hand; //Root joint of hand asset (hand_l), if populated bind to asset, if blank create joints
    private Hand mHand;
    private string timeCode = "00:00:00:00";
    public string poseName = "NULL";
    public int poseID = -1;
    public bool poseActive = false;
    public float poseConf = 0;

    //Settings
    private bool updateRootJoint = false;

    // Update is called once per frame
    void Update()
    {
        if (Connect && State == DISCONNECTED)
        {
            //Reset connect flag
            Connect = false;

            //Try establish connection to tcp socket
            ConnectToTcpServer();
        }
        else if (Connect && State == CONNECTED)
        {
            //Reset connect flag
            Connect = false;

            //Try to dis-establish connection with tcp socket
            socketConnection.Close();
            clientReceiveThread.Abort();
            State = DISCONNECTED;
        }
        else if (State == CONNECTED)
        {
            if (currentMessage != null && currentMessage.Length > 1)
            {
                try
                {
                    //Deserialisation recieved JSON package
                    mHand = JsonUtility.FromJson<Hand>(currentMessage);
                    timeCode = mHand.timecode;
                    poseName = mHand.poseName;
                    poseID = mHand.poseID;
                    poseActive = mHand.poseActive;
                    poseConf = mHand.poseConf;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }

            //TCP connection is active 
            if (mHand != null)
            {
                //Update hand asset
                updateHandAsset(hand, mHand, updateRootJoint);
            }
        }
    }

    private void OnApplicationQuit()
    {
        //If connection is running, close down
        if (State == CONNECTED)
        {
            //Reset connect flag
            Connect = false;

            //Try to dis-establish connection with tcp socket
            socketConnection.Close();
            clientReceiveThread.Abort();
            State = DISCONNECTED;
        }
    }


    void updateHandAsset(GameObject handAsset, Hand inputHand, bool updateRootJoint)
    {
        //Check if there is a hand asset 
        if (handAsset != null)
        {
            //Apply rotations to hand asset
            Stack<GameObject> joints = new Stack<GameObject>();
            joints.Push(handAsset);

            while (joints.Count > 0)
            {
                //Get next available joint
                GameObject target = joints.Pop();

                //Check if this joint has children, add to stack
                for (int i = 0; i < target.transform.childCount; i++)
                {
                    joints.Push(target.transform.GetChild(i).gameObject);
                }

                //Update all child joints, only update root joint if enabled by updateRootJoint
                if (updateRootJoint || target != handAsset)
                {
                    Bone targetBone = findBoneByName(mHand, target.name);
                    //Check if bone is valid
                    if (targetBone != null)
                    {
                        //Comment out any of these to prevent updating selected transforms
                        target.transform.localPosition = getTransform(targetBone);
                        target.transform.localEulerAngles = getEulerAngles(targetBone);
                        target.transform.localScale = getScale(targetBone);
                    }
                    else
                    {
                        Debug.Log("Error: Bone " + target.name + " not in selected heirachy");
                    }
                }
            }
        }
    }

    //Search incoming JSON data and return bone from name
    Bone findBoneByName(Hand mHand, string name)
    {
        for (int i = 0; i < mHand.bones.Length; i++)
        {
            //This relies on <= one ':' in the name
            String[] removeNameSpace = name.Split(':');

            if (removeNameSpace[removeNameSpace.Length - 1].Equals(mHand.bones[i].name))
            {
                return mHand.bones[i];
            }
        }

        return null;
    }

    //Return current translation of named joint
    Vector3 getTransform(Bone target)
    {
        Vector3 transform = new Vector3();

        if (target != null)
        {
            transform = new Vector3(-float.Parse(target.translation[0]), float.Parse(target.translation[1]),
                float.Parse(target.translation[2]));

            //Scale transformations to unity units (cm -> m)
            transform /= 100.0f;
        }

        return transform;
    }

    //Return euler rotation of named joint
    Vector3 getEulerAngles(Bone target)
    {
        Vector3 angles = new Vector3();

        if (target != null)
        {
            Quaternion q_pre_rotation = new Quaternion(float.Parse(target.pre_rotation[0]),
                -float.Parse(target.pre_rotation[1]), -float.Parse(target.pre_rotation[2]),
                float.Parse(target.pre_rotation[3]));
            Quaternion q_rotation = new Quaternion(float.Parse(target.rotation[0]), -float.Parse(target.rotation[1]),
                -float.Parse(target.rotation[2]), float.Parse(target.rotation[3]));
            Quaternion q_post_rotation = new Quaternion(float.Parse(target.post_rotation[0]),
                -float.Parse(target.post_rotation[1]), -float.Parse(target.post_rotation[2]),
                float.Parse(target.post_rotation[3]));

            Quaternion q = q_pre_rotation * q_rotation * q_post_rotation;

            angles = q.eulerAngles;
        }

        return angles;
    }


    //Return scale of named joint
    Vector3 getScale(Bone target)
    {
        Vector3 scale = new Vector3();

        if (target != null)
        {
            scale = new Vector3(float.Parse(target.scale[0]), float.Parse(target.scale[1]),
                float.Parse(target.scale[2]));
        }

        return scale;
    }


    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    private void ConnectToTcpServer()
    {
        try
        {
            State = CONNECTED;
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            State = DISCONNECTED;
            Debug.Log("On client connect exception " + e);
        }
    }

    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>     
    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient(host, Port);
            Byte[] bytes = new Byte[4];
            while (State == CONNECTED)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    while (stream.Read(bytes, 0, bytes.Length) != 0)
                    {
                        var size = MakeIPAddressInt32(bytes);
                        bytes = new Byte[size];
                        int read = 0;
                        while (read != bytes.Length)
                        {
                            read += stream.Read(bytes, read, bytes.Length - read);
                        }

                        currentMessage = Encoding.ASCII.GetString(bytes);
                        // Read incomming stream into byte arrary. 					
                        bytes = new Byte[4];
                        currentPacketCount++;
                        System.Threading.Thread.Sleep(1);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            State = DISCONNECTED;
            Debug.Log("Socket exception: " + socketException);
        }
    }

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            OutputTime();
        }
    }

    void OutputTime()
    {
        if (currentPacketCount >= 0)
        {
            int intervalCount = currentPacketCount - previousPacketCount;
            previousPacketCount = currentPacketCount;
            //Debug.Log(intervalCount);
        }
    }

    public static uint SwapBytes(UInt32 x)
    {
        // swap adjacent 16-bit blocks
        x = (x >> 16) | (x << 16);
        // swap adjacent 8-bit blocks
        return ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
    }

    static uint MakeIPAddressInt32(byte[] array)
    {
        // Make a defensive copy.
        var ipBytes = new byte[array.Length];
        array.CopyTo(ipBytes, 0);

        // Reverse if we are on a little endian architecture.
        if (BitConverter.IsLittleEndian)
            Array.Reverse(ipBytes);

        // Convert these bytes to an unsigned 32-bit integer (IPv4 address).
        return BitConverter.ToUInt32(ipBytes, 0);
    }
}