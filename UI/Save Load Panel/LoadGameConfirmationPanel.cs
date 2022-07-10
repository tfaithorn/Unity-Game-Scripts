using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadGameConfirmationPanel : MonoBehaviour
{
    public Save save;
    public SaveController saveController;

    public void Initialise(SaveController saveController, Save save)
    {
        this.saveController = saveController;
        this.save = save;    
    }

    public void LoadSave()
    {
        saveController.LoadSave(save);
    }
}
