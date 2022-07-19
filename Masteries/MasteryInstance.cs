using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MasteryInstance
{
    public Mastery mastery;
    public int position;
    public MasteryInstance(Mastery mastery, int position)
    {
        this.mastery = mastery;
        this.position = position;
    }
}
