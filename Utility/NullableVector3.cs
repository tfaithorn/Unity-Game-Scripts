using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullableVector3
{
    public Vector3 vector3;

    public NullableVector3(float x, float y, float z)
    {
        vector3 = new Vector3(x, y, z);
    }

    public NullableVector3(Vector3 vector3)
    {
        this.vector3 = vector3;
    }
}
