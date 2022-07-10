using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveSceneData
{
    public long saveId;
    public DateTime createdAt;
    public string data;
    public long sceneId;

    public SaveSceneData(long saveId, string data, long sceneId)
    {
        this.saveId = saveId;
        this.createdAt = createdAt;
        this.data = data;
        this.sceneId = sceneId;
    }
}
