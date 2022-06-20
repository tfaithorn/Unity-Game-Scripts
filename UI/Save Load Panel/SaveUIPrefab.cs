using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public class SaveUIPrefab : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI createdAt;
    public Sprite previewSprite;
    public SaveListPanel saveListPanel;
    public SavePanelController savePanelController;
    public Save save;

    public void SetValues(Save save, SaveListPanel saveListPanel, SavePanelController savePanelController)
    {
        this.save = save;
        this.name.text = save.name;
        this.createdAt.text = save.createdAt.ToString();
        this.saveListPanel = saveListPanel;
        this.savePanelController = savePanelController;
        /*
        string path = Application.streamingAssetsPath + "/" + Constants.saveScreenshotPath + "/" + save.id + "." + Constants.saveScreenshotExtension;

        if (File.Exists(path)) 
        {
            byte[] pngBytes = System.IO.File.ReadAllBytes(path);

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(pngBytes);
            previewSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }*/
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("On  pointer enter called?");
        if (saveListPanel.allowInteraction)
        {
            saveListPanel.savePanelController.SetSavePreview(this.save);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (savePanelController.mode == SavePanelController.Mode.SAVE)
        {
            savePanelController.ShowOverridePanel(save);
        }
    }
}
