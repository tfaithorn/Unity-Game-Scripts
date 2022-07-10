using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInGame : MonoBehaviour
{
    public bool hide = true;

    void Awake()
    {
        var renderer = this.GetComponent<Renderer>();

        if (renderer != null && hide) {
            renderer.enabled = false;
        }
    }

}
