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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
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
        else if (savePanelController.mode == SavePanelController.Mode.LOAD)
        {
            savePanelController.ShowLoadConfirmationPanel(save);
        }
    }
}
