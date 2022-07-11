using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullableQuanternion
{
    public Quaternion quaternion;

    public NullableQuanternion()
    { }

    public NullableQuanternion(Quaternion quaternion)
    {
        this.quaternion = quaternion;
    }
}
