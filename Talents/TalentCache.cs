using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TalentCache
{
    private static Dictionary<long, Talent> talents;

    static TalentCache()
    {
        talents = new Dictionary<long, Talent>()
        {
            {1, new LegSweepTalent()}
        };
    }

    public static Talent GetTalent(long id)
    {
        if (talents.ContainsKey(id))
        {
            return talents[id];
        }

        return null;
    }

    public static List<Talent> GetByMastery(Mastery mastery)
    {
        List<Talent> talentList = new List<Talent>();

        foreach (var item in talents)
        {
            if (item.Value.mastery == mastery)
            {
                talentList.Add(item.Value);
            }
        }

        return talentList;
    }
}
