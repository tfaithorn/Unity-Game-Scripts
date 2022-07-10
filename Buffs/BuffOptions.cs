using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffOptions : MonoBehaviour
{
    public int stacks;
    public int maxStacks;
    public float duration;
    public bool isPermaneant;
    public float tickRate;
    public bool hasTick;

    public BuffOptions()
    {
        this.duration = 0;
        this.tickRate = 1;
    }
}
