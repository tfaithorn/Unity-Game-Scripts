using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
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

    public long id;
    public string name;
    public bool isDebuff;
    public bool isVisible;
    public bool isStackable;
    public bool hasComponent;
    public string className;
    public StatusType statusType;

    public virtual Dictionary<StatsController.StatType, float> GetStatIncreases()
    {
        return null;
    }

    public abstract float GetDuration(CharacterMB characterMB);

}
