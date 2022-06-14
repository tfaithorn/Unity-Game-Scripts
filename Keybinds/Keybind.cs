using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Keybind
{

    public string keyType;
    public string path;
    List<string> list;
    public Keybind(string keyType, string path, List<string> list)
    {
        this.keyType = keyType;
        this.path = path;
        this.list = list;
    }

    public Keybind(string keyType)
    {
        this.keyType = keyType;
        this.path = null;
    }
}
