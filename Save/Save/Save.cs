using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Save
{
    public long id;
    public DateTime createdAt;
    public Player player;
    public string saveData;
    public string name;
    public long sceneId;
    public bool isSystem;
    public Save(long id, string name, DateTime createdAt, Player player, string saveData, long sceneId, bool isSystem)
    {
        this.id = id;
        this.name = name;
        this.createdAt = createdAt;
        this.player = player;
        this.saveData = saveData;
        this.sceneId = sceneId;
        this.isSystem = isSystem;
    }
}
