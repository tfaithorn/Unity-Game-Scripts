using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCharacterOptions
{
    public NullableVector3 position;
    public NullableVector3 scale;
    public NullableQuanternion rotation;

    public PlaceCharacterOptions(){}

    public PlaceCharacterOptions(NullableVector3 position, NullableQuanternion rotation, NullableVector3 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }

    public void SetPosition(Vector3 vector3)
    {
        this.position = new NullableVector3(vector3);
    }

    public void SetScale(Vector3 vector3)
    {
        this.scale = new NullableVector3(vector3);
    }

    public void SetRotation(Quaternion quaternion)
    {
        this.rotation = new NullableQuanternion(quaternion);
    }
}
