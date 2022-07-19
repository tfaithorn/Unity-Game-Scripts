using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentInstance
{
    public Talent talent;
    public int rank;

    public TalentInstance(Talent talent, int rank)
    {
        this.talent = talent;
        this.rank = rank;
    }
}
