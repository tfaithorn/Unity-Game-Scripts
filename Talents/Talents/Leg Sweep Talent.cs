using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegSweepTalent : Talent
{
    public LegSweepTalent()
    {
        this.id = 1;
        this.name = "Leg Sweep";
        this.mastery = MasteryCache.GetMastery(1);
        this.maxRank = 1;
    }

    public override string GetIconFileName(CharacterMB characterMB)
    {
        return "Leg Sweep";
    }
}
