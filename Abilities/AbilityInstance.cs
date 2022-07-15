using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInstance
{
    public Ability ability;
    public AbilityScript abilityScript;
    public Timer duration;
    public Timer cooldown;
    public bool disabled;
    public bool isLoaded;
    public bool onDuration;
    public bool onCooldown;
    public bool interruptAbility;

    public AbilityInstance(Ability ability)
    {
        this.ability = ability;
        this.isLoaded = false;
        this.duration = new Timer(0);
        this.cooldown = new Timer(0);
        this.interruptAbility = false;
        this.disabled = false;
    }
}
