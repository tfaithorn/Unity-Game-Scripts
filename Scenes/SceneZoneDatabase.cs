using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneZoneDatabase
{
    private static Dictionary<long, SceneZone> sceneZones;
    static SceneZoneDatabase()
    {
        sceneZones = new Dictionary<long, SceneZone>() 
        {
            {1, new StartScreenZone()},
            {2, new StartingZone()}
        };
    }

    public static SceneZone GetSceneZone(long id)
    {
        return sceneZones[id];
    }
}
