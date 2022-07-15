using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityCache
{
    private static Dictionary<long, Ability> abilities = new Dictionary<long, Ability> ();

    static AbilityCache()
    {
        abilities[1] = new FlashHeal();
    }

    public static Ability GetAbility(long id)
    {
        if (abilities.ContainsKey(id))
        {
            return abilities[id];
        }
        else
        {
            return null;
        }
    }
}
