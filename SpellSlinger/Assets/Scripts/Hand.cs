using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hand
{
    //Containing class for incoming hand data
    public string name;
    public Bone[] bones;
    public int side;
    public string timecode;
    public string poseName;
    public int poseID;
    public bool poseActive;
    public float poseConf;
}

[Serializable]
public class Bone {
    public string name;
    public int parent;
    public string[] translation;
    public string[] rotation;
    public string[] scale;
    public string[] pre_rotation;
    public string[] post_rotation;
    public string rotation_order;
}