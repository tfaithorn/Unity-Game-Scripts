using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regeneration : MonoBehaviour, BuffInterface
{
    StatsController sourceController;
    StatsController thisStatsController;

    public void BuffStart(Buff buff)
    {
        sourceController = buff.source.GetComponent<StatsController>();
        thisStatsController = GetComponent<StatsController>();
    }
    public void BuffTick()
    { 
        
    }

    public void BuffRemove()
    { 
    
    }
    public void BuffFinish()
    { 
    
    }

}
