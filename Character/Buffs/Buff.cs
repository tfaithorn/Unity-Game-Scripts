using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public enum StatusType
    {
        NONE,
        STUN,
        ROOT,
        FEAR,
        FREEZE,
        KNOCKDOWN,
        BLIND
    }

    public int id;
    public string name;
    public float durationPassed;
    public float duration;
    public float tickRate;
    public int currentTick;
    public bool isPermanent;
    public bool isDebuff;
    public bool isVisible;
    public bool hasComponent;
    public string className;

    public StatusType statusType;
    public BuffInterface buffInterface;
    public GameObject source;

    public Dictionary<StatsController.StatType, float> statIncreases;
    public Dictionary<string,string> otherParameters;

    public StatsController creator;

    public Buff()
    {
        duration = 0f;
        durationPassed = 0f;
        tickRate = 1f;
        currentTick = 0;
        statusType = StatusType.NONE;
        hasComponent = false;
    }
}
