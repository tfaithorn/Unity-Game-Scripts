using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashHeal : Ability
{
    public FlashHeal()
    {
        this.id = 1;
        this.name = "Flash Heal";
        this.abilityScriptName = "FlashHealMB";
        this.icon = "flash-heal-icon";
    }

    public override string GetDescription(CharacterMB characterMB)
    {
        return "This is the Flash Heal Description";
    }
}
