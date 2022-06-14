using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorHealthPotion : ItemUseableInterface
{
    float healAmount = 20f;

    public void Use(Character character)
    {
        StatsController sc = character.statsController;
        sc.Heal(healAmount);
    }

}
