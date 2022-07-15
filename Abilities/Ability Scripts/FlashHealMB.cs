using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashHealMB : AbilityScript
{
    private void Awake()
    {
        this.isChanneled = false;
        this.hasCooldown = false;
        this.isInfinite = false;
        this.allowMovement = true;
        this.stopAbility = false;
        this.isPerformed = true;
        this.showAbility = true;
        this.isInterruptable = true;
    }
    public override void LoadAbility(Ability ability) 
    {

    }

    public override void StartAbility() 
    {
        Debug.Log("Ability Started?");
    }
    public override void FinishAbility() 
    {
        Debug.Log("Ability Finished?");
    }
    public override void InterruptAbility() { }
    public override void ResetAbility() { }

    public override float GetDuration(CharacterMB characterMB)
    {
        return 5;
    }

    public override float GetCooldown(CharacterMB characterMB)
    {
        return 10;
    }

    public override bool GetInterruptable(CharacterMB characterMB)
    {
        return true;
    }
}
