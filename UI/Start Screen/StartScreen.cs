using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public SceneController sceneController;
    public SavePanelController savePanelController;
    public State state;
    public StartScreenMenuPanel startMenuPanel;
    public StartScreenMenuPanel settingsPanel;
    public StartScreenMenuPanel loadPanel;
    public StartScreenMenuPanel newGamePanel;
    public StartScreenMenuPanel blackBackground;
    private StartScreenMenuPanel currentScreenMenuPanel;
    public LoadingScreen loadingScreen;
    List<StartScreenMenuPanel> menuPanels;
    public Camera cam;
    public Transform mainCameraAnchor;
    public Transform characterCreationCameraAnchor;

    private void Awake()
    {
        menuPanels = new List<StartScreenMenuPanel>() {
            loadPanel,
            settingsPanel,
            newGamePanel
        };

        loadingScreen.Init();
        loadingScreen.SetProgress(20f);

        loadingScreen.SetProgress(0.50f);
        loadingScreen.SetProgress(1f);
        loadingScreen.Finish();
    }

    public enum State
    {
        NONE,
        NEW_GAME,
        LOAD_GAME,
        SETTINGS
    }

    public void Continue()
    {
        //TODO: Update to be real
        NewGame();
    }

    public void NewGame()
    {
        if (state != State.NEW_GAME)
        {
            DeactivateOtherPanels(newGamePanel);
            state = State.NEW_GAME;
            currentScreenMenuPanel = newGamePanel;
            StartCoroutine(NewGameTransition());

        }
        else
        {
            state = State.NONE;
            newGamePanel.gameObject.SetActive(false);
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

    private void DeactivateOtherPanels(StartScreenMenuPanel currPanel)
    {
        foreach (StartScreenMenuPanel menuPanel in menuPanels)
        {
            if (menuPanel != currPanel)
            {
                menuPanel.gameObject.SetActive(false);
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private IEnumerator NewGameTransition()
    {
        StartCoroutine(FadeOutCanvas(startMenuPanel.canvasGroup, 0.2f));
        blackBackground.gameObject.SetActive(true);
        yield return StartCoroutine(FadeInCanvas(blackBackground.canvasGroup, 0.5f));
        PlaceCameraAtTransform(characterCreationCameraAnchor);
        yield return new WaitForSeconds(0.2f);
        newGamePanel.gameObject.SetActive(true);
        StartCoroutine(FadeInCanvas(newGamePanel.canvasGroup, 0.1f));
        yield return StartCoroutine(FadeOutCanvas(blackBackground.canvasGroup, 0.2f));
        blackBackground.gameObject.SetActive(false);
    }

    public void ReturnToStartMenu()
    {
        StartCoroutine(StartMenuTransition());
    }

    private IEnumerator StartMenuTransition()
    {
        StartCoroutine(FadeOutCanvas(currentScreenMenuPanel.canvasGroup, 0.2f));
        blackBackground.gameObject.SetActive(true);
        yield return StartCoroutine(FadeInCanvas(blackBackground.canvasGroup, 0.5f));
        PlaceCameraAtTransform(mainCameraAnchor);
        currentScreenMenuPanel.gameObject.SetActive(false);
        state = State.NONE;
        yield return new WaitForSeconds(0.2f);
        startMenuPanel.gameObject.SetActive(true);
        StartCoroutine(FadeInCanvas(startMenuPanel.canvasGroup, 0.1f));
        yield return StartCoroutine(FadeOutCanvas(blackBackground.canvasGroup, 0.2f));
        blackBackground.gameObject.SetActive(false);
    }

    private void PlaceCameraAtTransform(Transform transform)
    {
        cam.transform.position = transform.position;
        cam.transform.rotation = transform.rotation;
    }

    public IEnumerator FadeInCanvas(CanvasGroup canvasGroup, float endTime)
    {
        float t = 0;
        canvasGroup.alpha = 0;

        while (t < endTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / endTime;

            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    public IEnumerator FadeOutCanvas(CanvasGroup canvasGroup, float endTime)
    {
        float t = 0;

        while (t < endTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = (endTime - t) / endTime;

            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}