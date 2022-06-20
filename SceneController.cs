using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public enum SceneType { 
        START_SCREEN = 1,
        LEVEL_1 = 2,
    }

    public static void LoadSceneAsync(SceneType scene)
    {
        switch (scene) {
            case SceneType.START_SCREEN:
                AssembleScene("StartScreen");
                break;
            case SceneType.LEVEL_1:
                AssembleScene("SampleScene");
                break;
        }
    }

    private static void AssembleScene(string filePath)
    {
        SceneManager.LoadSceneAsync(filePath);
        Debug.Log(filePath);
    }
}
