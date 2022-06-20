using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Save
{
    public long id;
    public DateTime createdAt;
    public PlayerCharacter playerCharacter;
    public string saveData;
    public string name;
    public string previewImagePath;

    public Save(long id, string name, DateTime createdAt, PlayerCharacter playerCharacter, string saveData, string previewImagePath)
    {
        this.id = id;
        this.name = name;
        this.createdAt = createdAt;
        this.playerCharacter = playerCharacter;
        this.saveData = saveData;
        this.previewImagePath = previewImagePath;
    }
}
