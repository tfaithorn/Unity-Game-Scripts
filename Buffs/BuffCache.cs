using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuffCache
{
    public static Dictionary<long, Buff> buffs = new Dictionary<long, Buff>() {
        {1, new RegenerationBuff()}
    };
}
