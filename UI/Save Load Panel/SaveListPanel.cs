using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveListPanel : MonoBehaviour
{
    public List<Save> saves;
    public bool allowInteraction = true;
    public SavePanelController savePanelController;

    private void Awake()
    {
    }

    public void Init()
    {
        ResetList();
        PopulateList();
    }

    private void ResetList()
    {
        foreach (Transform saveUIPrefab in this.transform)
        {
            GameObject.Destroy(saveUIPrefab.gameObject);
        }

        saves = null;
    }

    private void PopulateList()
    {
        var saveRepository = new SaveRepository();
        saves = saveRepository.GetSavesByPlayerId(savePanelController.selectedPlayer.id);

        foreach (var save in saves)
        {
            SaveUIPrefab prefab = Resources.Load<SaveUIPrefab>(Constants.UIprefabsPath + "/save");
            var saveUIPrefab = Instantiate(prefab, this.transform,false);
            saveUIPrefab.SetValues(save, this, savePanelController);
            saveUIPrefab.gameObject.SetActive(true);
        }
    }

}
