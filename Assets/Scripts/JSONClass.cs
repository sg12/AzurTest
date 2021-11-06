using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JSONClassSingle
{
    public Vector3 Position;

    public int HashCode = 0;

    public int ClassType = 0; // 0 - animal, 1 - food

    public int ChildHashCode = 0;

    public float QuaternionX = 0f;
    public float QuaternionY = 0f;
    public float QuaternionZ = 0f;
    public float QuaternionW = 0f;
}

[Serializable]
public class JSONClassList
{
    public int _N = 2;
    public float DefaultSpeedAnimal = 5f;
    public Vector3 CameraPosition;
    public Vector3 CameraRotation;
    public float TimeScaleValue = 1f;

    public List<JSONClassSingle> ClassList;
}




