using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regeneration : MonoBehaviour, IBuffInstance
{
    BuffInstance buffInstance;

    public void BuffStart(BuffInstance buffInstance)
    {
        this.buffInstance = buffInstance;
    }
    public void BuffTick()
    {
        Debug.Log("did this tick?" + buffInstance.tickRate + " Transform:" + gameObject.transform.name);
    }

    public void BuffRemove()
    { 
    
    }
    public void BuffFinish()
    { 
    
    }

}
