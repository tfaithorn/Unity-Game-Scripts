using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewSavePanel : MonoBehaviour
{
    public TMP_InputField saveNameText;
    public SavePanelController savePanelController;
    public void SetValues(string text, SavePanelController savePanelController)
    {
        this.saveNameText.text = text;
        this.savePanelController = savePanelController;
    }

    public void SaveGame()
    {
        this.savePanelController.SaveGame(saveNameText.text);
    }
}
