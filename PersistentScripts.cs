using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentScripts : MonoBehaviour
{
    public bool destroyOnGame = false;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //search duplicates in scene and remove them
        GameObject[] persistentGameObjects = GameObject.FindGameObjectsWithTag(Constants.persistentScriptsTagName);

        if (persistentGameObjects.Length > 1) {
            foreach (GameObject gb in persistentGameObjects)
            {
                if (destroyOnGame) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
