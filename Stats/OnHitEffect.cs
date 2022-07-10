using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitEffect
{
    float chance;
    string description;

    public void Cast() {
        float randomRoll = Random.Range(0, 100);

        if (randomRoll <= chance)
        {
            
        }
    }
}
