using UnityEngine;
using System;

[Serializable]
public class Vector3Serializer
{

    public float x;
    public float y;
    public float z;


    public Vector3Serializer(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }


    public static implicit operator Vector3Serializer(Vector3 a)
    {
        return new Vector3Serializer(a.x, a.y, a.z);
    }

    public static implicit operator Vector3(Vector3Serializer a)
    {
        return new Vector3(a.x, a.y, a.z);
    }
}
