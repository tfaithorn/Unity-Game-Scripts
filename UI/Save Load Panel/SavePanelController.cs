using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.IO;
using TMPro;

public class SavePanelController : MonoBehaviour
{
    public Camera cam;
    public Image previewImage;
    public TextMeshProUGUI previewSummary;
    public List<SaveUIPrefab> saveUIPrefabs;
    public NewSavePanel newSavePanel;
    public SaveListPanel saveListPanel;
    public Mode mode;
    public RectTransform saveContainerPanel;
    public RectTransform loadContainerPanel;
    public LoadGameConfirmationPanel loadGameConfirmationPanel;
    public TMP_Dropdown playerDropdown;
    public OverrideSavePanel overrideSavePanel;
    public Sprite defaultPreviewSprite;
    public SaveController saveController;
    private SceneController sceneController;

    private List<Player> playerList;
    public Player selectedPlayer;

    public enum Mode
    {
        SAVE = 1,
        LOAD = 2
    }

    public void Awake()
    {
        playerList = PlayerDatabase.GetPlayerList();
        saveController = SaveController.FindSaveController();
        sceneController = SceneController.FindSceneController();

        if (playerDropdown != null)
        {
            playerDropdown.onValueChanged.AddListener(x => {
                selectedPlayer = playerList[x];
                saveListPanel.Init();
            });
        }
    }

    public void EnabledSelected()
    {
        if (saveController.player != null)
        {
            selectedPlayer = saveController.player;
        }
        else
        {
            selectedPlayer  = PlayerDatabase.GetLastPlayed();
        }

        switch (mode) {
            case Mode.SAVE:
                if (saveContainerPanel != null) {
                    ResetPreviewPanel();
                    saveContainerPanel.gameObject.SetActive(true);
                }

                if (loadContainerPanel != null) {
                    loadContainerPanel.gameObject.SetActive(false);
                }
                break;
            case Mode.LOAD:
                if (loadContainerPanel != null) {
                    ResetPreviewPanel();
                    loadContainerPanel.gameObject.SetActive(true);
                }

                if (saveContainerPanel != null) {
                    saveContainerPanel.gameObject.SetActive(false);
                }

                SetPlayerCharacterDropdown(selectedPlayer);

                break;      
        }
    }

    private void ResetPreviewPanel()
    {
        previewImage.sprite = defaultPreviewSprite;
        previewSummary.text = "";
    }

    public void Init()
    {
        EnabledSelected();

        if (newSavePanel != null) {
            HideNewSavePanel();
        }

        if (overrideSavePanel != null) {
            HideOverridePanel();
        }

        saveListPanel.Init();
        saveListPanel.allowInteraction = true;
    }

    public void SetSaveMode(int num)
    {
        mode = (Mode)num;
        EnabledSelected();
        Init();
    }

    public void ShowNewSavePanel()
    {
        saveListPanel.allowInteraction = false;
        newSavePanel.gameObject.SetActive(true);
        newSavePanel.SetValues("Save "+ (saveListPanel.saves.Count + 1), this);
    }

    public void HideNewSavePanel()
    {
        newSavePanel.gameObject.SetActive(false);
    }

    public void SaveGame(string saveName)
    {
        SceneController sceneController = SceneController.FindSceneController();
        var sceneId = sceneController.currentSceneZone.GetSceneId();
        Save save = saveController.SaveGame(saveName, sceneId, cam);
        SaveScreenshot(save.id);
        Init();
    }

    public void LoadGame()
    {
        //TODO: look into adding a load screen here for when coroutine execution is complete
        saveController.LoadSave(loadGameConfirmationPanel.save);
    }

    private void SetPlayerCharacterDropdown(Player currentPlayer = null)
    {
        List<string> options = new List<string>();

        int i = 0;
        foreach (Player player in playerList)
        {
            if (player == currentPlayer)
            {
                playerDropdown.value = i;
            }

            options.Add(player.name);
            i++;
        }

        playerDropdown.ClearOptions();
        playerDropdown.AddOptions(options);
        //playerDropdown.value = playerList.FindIndex(x => x.id == selectedPlayer.id);
    }

    public void SetSavePreview(Save save)
    {
        ResetPreviewPanel();

        string path = Application.streamingAssetsPath + "/" + Constants.saveScreenshotPath + "/" + save.id + "." + Constants.saveScreenshotExtension;
        Sprite previewSprite;

        if (File.Exists(path))
        {
            byte[] pngBytes = System.IO.File.ReadAllBytes(path);

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(pngBytes);
            previewSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

            previewImage.sprite = previewSprite;
            previewSummary.text = save.player.name;
        }
    }

    /// <summary>
    /// Takes a screenshot using the main camera
    /// Note: Look at moving this to the camera script.
    /// </summary>
    /// <param name="saveId"></param>
    private void SaveScreenshot(long saveId)
    {
        int resHeight = Screen.height;
        int resWidth = Screen.width;

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = saveId + ".PNG";

        string path = Application.streamingAssetsPath + "/" + Constants.saveScreenshotPath + "/" + filename;

        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
    }

    public void ShowOverridePanel(Save save)
    {
        overrideSavePanel.gameObject.SetActive(true);
        overrideSavePanel.SetValues(save);
    }

    public void ShowLoadConfirmationPanel(Save save)
    {
        loadGameConfirmationPanel.gameObject.SetActive(true);
        loadGameConfirmationPanel.Initialise(saveController, save);
    }

    public void HideOverridePanel()
    {
        overrideSavePanel.gameObject.SetActive(false);
    }

    public void HideLoadConfirmationPanel()
    {
        loadGameConfirmationPanel.gameObject.SetActive(false);
    }
    public void OverrideSave(Save save)
    {
        

        saveController.OverrideSave(save, sceneController.currentSceneZone.GetSceneId());
        HideOverridePanel();
        Init();
    }
}
