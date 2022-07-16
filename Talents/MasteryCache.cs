using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MasteryCache
{
    private static Dictionary<long, Mastery> masteries;
    static MasteryCache()
    {
        masteries = new Dictionary<long, Mastery>()
        {
            { 1, new ManAtArms()},
            { 2, new MageApprentice()},
        };
    }

    public static Mastery GetMastery(long id)
    {
        if (masteries.ContainsKey(id))
        {
            return masteries[id];
        }
        else
        {
            return null;
        }
    }
}
