using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSource
{
    public StatsController.StatType statType;
    public float value;
    public SourceType sourceType;
    public long sourceId;
    public enum SourceType { 
        BASE = 0,
        GEAR = 1,
        BUFF = 2,    
    }

    public StatSource(StatsController.StatType statType, float value, SourceType sourceType, long sourceId)
    {
        this.statType = statType;
        this.value = value;
        this.sourceType = sourceType;
        this.sourceId = sourceId;
    }

}
