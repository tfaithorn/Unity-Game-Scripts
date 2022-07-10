using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationBuff : Buff
{
    public RegenerationBuff()
    {
        this.id = 1;
        this.name = "Regeneration";
        this.isDebuff = false;
        this.isVisible = true;
        this.isStackable = false;
        this.hasComponent = true;
        this.className = "Regeneration";
        this.statusType = Buff.StatusType.NONE;
    }

    public override float GetDuration(CharacterMB creator)
    {
        float duration = 5;
        return duration;
    }
}
