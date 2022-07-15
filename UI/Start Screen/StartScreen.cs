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
    public RectTransform newGameGroup;
    public LoadingScreen loadingScreen;
    List<RectTransform> menuPanels;


    private void Awake()
    {
        menuPanels = new List<RectTransform>() {
            loadPanel,
            settingsPanel,
            newGameGroup
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

    public void NewGame()
    {
        if (state != State.NEW_GAME)
        {
            DeactivateOtherPanels(newGameGroup);
            newGameGroup.gameObject.SetActive(true);
            state = State.NEW_GAME;
        }
        else
        {
            state = State.NONE;
            newGameGroup.gameObject.SetActive(false);
        }
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
