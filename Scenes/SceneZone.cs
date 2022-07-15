using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public abstract class SceneZone
{
    public long id;
    public string name;
    public long GetSceneId()
    {
        return this.id;
    }

    public string GetSceneName()
    {
        return this.name;
    }

}
