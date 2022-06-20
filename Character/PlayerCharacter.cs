using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCharacter
{
    public long id;
    public string name;
    public DateTime lastPlayed;

    public PlayerCharacter(long id, string name, DateTime lastPlayed)
    {
        this.id = id;
        this.name = name;
        this.lastPlayed = lastPlayed;
    }
}
