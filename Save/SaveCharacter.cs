using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveCharacter
{
    public Save save;
    public Character character;
    public long sceneId;
    public string saveData;
    public string guid;
    public SaveCharacter(Save save, Character character, long sceneId, string saveData, string guid)
    {
        this.save = save;
        this.character = character;
        this.sceneId = sceneId;
        this.saveData = saveData;
        this.guid = guid;
    }
}
