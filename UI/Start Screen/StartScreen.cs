using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public SceneController sceneController;
    public SavePanelController savePanelController;
    public State state;
    public RectTransform settingsPanel;
    public RectTransform loadPanel;
    public LoadingScreen loadingScreen;
    List<RectTransform> menuPanels;

    private void Awake()
    {
        menuPanels = new List<RectTransform>() {
            loadPanel,
            settingsPanel
        };

        loadingScreen.Init();
        loadingScreen.SetProgress(20f);

        loadingScreen.SetProgress(0.50f);
        loadingScreen.SetProgress(1f);
        loadingScreen.Finish();
    }

    public enum State { 
        NONE,
        NEW_GAME,
        LOAD_GAME,
        SETTINGS
    }

    public void Continue()
    {
        
        //sceneController.LoadScene(sceneController., StartingZone.nodeName1);
        //sceneController.sceneOne.LoadScene(SceneOne.nodeName1);
    }

    public void NewGame()
    {
        sceneController.LoadScene(SceneZoneDatabase.GetSceneZone(1), StartingZone.nodeName1);
    }

    public void LoadGame()
    {
        if (state != State.LOAD_GAME)
        {
            DeactivateOtherPanels(loadPanel);
            loadPanel.gameObject.SetActive(true);
            savePanelController.Init();
            state = State.LOAD_GAME;
        }
        else
        {
            loadPanel.gameObject.SetActive(false);
            state = State.NONE;
        }
    }

    public void Settings()
    {
        if (state != State.SETTINGS)
        {
            DeactivateOtherPanels(settingsPanel);
            settingsPanel.gameObject.SetActive(true);
            state = State.SETTINGS;
        }
        else
        {
            settingsPanel.gameObject.SetActive(false);
            state = State.NONE;
        }
    }

    private void DeactivateOtherPanels(RectTransform currRect)
    {
        foreach (RectTransform rect in menuPanels)
        {
            if (rect != currRect)
            {
                rect.gameObject.SetActive(false);
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
