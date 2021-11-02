using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System.Text;

namespace SpellSlinger
{
    public class GameManager : MonoBehaviour
    {
		void ListDevices()
        {
            for (int i = 0; i < SteamVR.connected.Length; ++i)
            {
                ETrackedPropertyError error = new ETrackedPropertyError();
                StringBuilder sb = new StringBuilder();
                OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_SerialNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
                var SerialNumber = sb.ToString();

                OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_ModelNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
                var ModelNumber = sb.ToString();
                if (SerialNumber.Length > 0 || ModelNumber.Length > 0)
                    Debug.Log("Device " + i.ToString() + " = " + SerialNumber + " | " + ModelNumber);
            }
        }
    }
}