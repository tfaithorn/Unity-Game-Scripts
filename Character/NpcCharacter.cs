using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCharacter : Character
{
    public NpcCharacter(long id, string name, string prefabPath)
    {
        this.id = id;
        this.name = name;
        this.prefabPath = prefabPath;
    }
}
