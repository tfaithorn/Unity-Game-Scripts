using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RiggingManager : MonoBehaviour
{
    public Rig headRig;
    public Rig chestRig;
    public Transform headTarget;
    public Transform chestTarget;

    public void PlaceHeadTarget(Vector3 headPosition)
    {
        headTarget.position = headPosition;
    }

    public void PlaceChestTarget(Vector3 chestPosition)
    {
        chestTarget.position = chestPosition;
    }

    public void EnableHeadRig()
    {
        headRig.weight = 1;
    }
    public void DisableHeadRig()
    {
        headRig.weight = 0;
    }


    public void EnableChestRig(float value)
    {
        chestRig.weight = value;
    }

    public void DisableChestRig()
    {
        chestRig.weight = 0;
    }


}
