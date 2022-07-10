using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class responsible for maintaining the state of a buff on a character
/// </summary>
public sealed class BuffInstance
{
    public readonly Buff buff;
    public bool isPermanent;
    public float tickRate;
    public bool hasTick;
    public int currentTick;
    public int stacks;
    public int maxStacks;
    public Timer timer;
    public IBuffInstance buffInterface;
    public CharacterMB creator;

    public BuffInstance(Buff buff, CharacterMB creator, CharacterMB target, BuffOptions buffOptions)
    {
        this.buff = buff;
        this.creator = creator;
        this.timer = new Timer(buffOptions.duration);
        this.maxStacks = buffOptions.maxStacks;
        this.stacks = buffOptions.stacks;
        this.isPermanent = buffOptions.isPermaneant;
        this.hasTick = buffOptions.hasTick;
        this.tickRate = buffOptions.tickRate;

        if (buff.hasComponent)
        {
            buffInterface = (IBuffInstance)target.gameObject.AddComponent(Type.GetType(buff.className));
            buffInterface.BuffStart(this);
        }
    }

    private void ApplyOptions(Dictionary<string, object> options)
    {
        if (options.ContainsKey("duration"))
        {
            this.timer = new Timer((float)options["duration"]);
        }

        if (options.ContainsKey("stacks"))
        {
            this.stacks = (int)options["stacks"];
        }

        if (options.ContainsKey("maxStacks"))
        {
            this.maxStacks = (int)options["maxStacks"];
        }

        if (options.ContainsKey("isPermanent"))
        {
            this.isPermanent = (bool)options["isPermanent"];
        }
    }

    public void SetTimer(float duration)
    {
        this.timer = new Timer(duration);
    }
}
