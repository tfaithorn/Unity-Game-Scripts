using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuPanel : MonoBehaviour
{
    public enum Type
    {
        NONE = 0,
        QUESTS = 1,
        INVENTORY = 2,
        TALENTSABILITIES = 3,
        SAVELOAD = 4,
        SETTINGS = 5
    }

    public abstract void Deactivate();
}
