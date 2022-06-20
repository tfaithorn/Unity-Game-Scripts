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
    public TMP_Dropdown playerDropdown;
    public OverrideSavePanel overrideSavePanel;
    public Sprite defaultPreviewSprite;

    private List<PlayerCharacter> playerCharacters;
    public PlayerCharacter playerCharacter;

    public enum Mode
    {
        SAVE = 1,
        LOAD = 2
    }

    public void Awake()
    {
        playerCharacter = PlayerCharacterManager.playerCharacter;
        playerCharacters = PlayerCharacterManager.playerCharacters;

        if (playerDropdown != null)
        {
            playerDropdown.onValueChanged.AddListener(x => {
                playerCharacter = playerCharacters[x];
                saveListPanel.Init();
            });
        }
    }

    public void EnabledSelected()
    {
        switch (mode) {
            case Mode.SAVE:
                if (saveContainerPanel != null) {
                    saveContainerPanel.gameObject.SetActive(true);
                }

                if (loadContainerPanel != null) {
                    loadContainerPanel.gameObject.SetActive(false);
                }
                break;
            case Mode.LOAD:
                if (loadContainerPanel != null) {
                    loadContainerPanel.gameObject.SetActive(true);
                }

                if (saveContainerPanel != null) {
                    saveContainerPanel.gameObject.SetActive(false);
                }
                
                SetPlayerCharacterDropdown();
                break;      
        }

        previewImage.sprite = defaultPreviewSprite;
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
        var saveValues = new Dictionary<string, object>()
        {
            { "playerId", playerCharacter.id},
            { "saveData", GetSaveData()},
            { "name", saveName}
        };

        var saveRepository = new SaveRepository();
        var saveId = saveRepository.Insert(saveValues, true);
        SaveScreenshot(saveId);
        Init();
    }

    private void SetPlayerCharacterDropdown()
    {
        playerCharacters = PlayerCharacterManager.playerCharacters;

        List<string> options = new List<string>();

        foreach (PlayerCharacter playerCharacter in playerCharacters)
        {
            options.Add(playerCharacter.name);
        }

        playerDropdown.ClearOptions();
        playerDropdown.AddOptions(options);
        playerDropdown.value = playerCharacters.FindIndex(x => x.id == this.playerCharacter.id);

    }

    public void SetSavePreview(Save save)
    {
        string path = Application.streamingAssetsPath + "/" + Constants.saveScreenshotPath + "/" + save.id + "." + Constants.saveScreenshotExtension;
        Debug.Log("calling set preview?");
        Sprite previewSprite;

        if (File.Exists(path))
        {
            Debug.Log("file exists?");
            byte[] pngBytes = System.IO.File.ReadAllBytes(path);

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(pngBytes);
            previewSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

            previewImage.sprite = previewSprite;
            previewSummary.text = save.playerCharacter.name;
        }
    }

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

    public void OverrideSave(Save save)
    {    
        var saveValues = new Dictionary<string, object>()
        {
            { "saveData", GetSaveData()},
            { "name", overrideSavePanel.saveNameText.text},
            { "createdAt", SqlClient.Function.CURRENT_TIMESTAMP}
        };

        var criteria = new List<SqlClient.Expr>()
        { 
            new SqlClient.Cond("id", save.id, SqlClient.OP_EQUAL)
        };

        SaveRepository saveRepository = new SaveRepository();
        saveRepository.Update(saveValues, criteria);
        SaveScreenshot(save.id);

        HideOverridePanel();
        saveListPanel.Init();
    }

    public void HideOverridePanel()
    {
        overrideSavePanel.gameObject.SetActive(false);
    }

    public string GetSaveData()
    {
        //TBC: return save data as a json string
        return "<TBC>";
    }
}
