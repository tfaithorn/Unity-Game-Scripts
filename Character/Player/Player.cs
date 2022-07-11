using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : Character
{
    public DateTime lastPlayed;

    public Player(long id, string name, DateTime lastPlayed)
    {
        this.id = id;
        this.name = name;
        this.lastPlayed = lastPlayed;
    }
}
