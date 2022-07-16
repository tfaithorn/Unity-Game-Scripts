using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentInstance
{
    Talent talent;
    int rank;
    int position;

    public TalentInstance(Talent talent, int rank, int position)
    {
        this.talent = talent;
        this.position = position;
        this.rank = rank;
    }
}
