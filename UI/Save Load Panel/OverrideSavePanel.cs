using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverrideSavePanel : MonoBehaviour
{
    public SavePanelController savePanelController;
    public TMP_InputField saveNameText;
    public Save save;

    public void SetValues(Save save)
    {
        this.save = save;
        saveNameText.text = save.name;
    }

    public void ConfirmOverrideSave()
    {
        save.name = saveNameText.text;
        savePanelController.OverrideSave(save);
    }

}
