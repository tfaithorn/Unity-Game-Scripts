using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveListPanel : MonoBehaviour
{
    public List<Save> saves;
    List<SaveUIPrefab> instantiatedSaveUIPrefabs = new List<SaveUIPrefab>();
    public bool allowInteraction = true;
    public SavePanelController savePanelController;
    private PoolingHelper savePoolingHelper;

    private void Awake()
    {
        savePoolingHelper = gameObject.AddComponent<PoolingHelper>();
        GameObject saveUIPrefab = Resources.Load<GameObject>(Constants.UIprefabsPath + "/save");
        savePoolingHelper.Initialize(saveUIPrefab, 20, "Save List Pooling Helper");
    }

    public void Init()
    {
        ResetList();
        PopulateList();
    }

    private void ResetList()
    {
        foreach (SaveUIPrefab saveUIPrefab in instantiatedSaveUIPrefabs)
        {
            savePoolingHelper.ReturnItem(saveUIPrefab.gameObject);
        }

        saves = null;
        instantiatedSaveUIPrefabs.Clear();
    }

    private void PopulateList()
    {

        saves = SaveManager.GetSavesByPlayerCharacterId(savePanelController.playerCharacter.id);

        foreach (var save in saves)
        {
            var instantiatedSave = savePoolingHelper.RequestItem();
            var saveUIPrefab = instantiatedSave.GetComponent<SaveUIPrefab>();

            saveUIPrefab.SetValues(save, this, savePanelController);
            instantiatedSave.transform.SetParent(this.transform,false);
            saveUIPrefab.gameObject.SetActive(true);
            instantiatedSaveUIPrefabs.Add(saveUIPrefab);
        }
    }

}
