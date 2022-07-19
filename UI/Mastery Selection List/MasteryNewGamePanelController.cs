using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MasteryNewGamePanelController : MonoBehaviour
{
    public TextMeshProUGUI masteryCountText;
    public List<Mastery> masteriesSelected;
    private int masteryLimit;
    public TextMeshProUGUI masteryCountLabelText;
    public MasteryPreviewController masteryPreviewController;

    private void Awake()
    {
        masteryLimit = 1;
        this.masteriesSelected = new List<Mastery>();
        masteryCountText.text = $"( {masteriesSelected.Count} / {masteryLimit} ) ";
    }

    public bool SelectMastery(MasteryPanelPrefab masteryPanelPrefab)
    {
        if (masteriesSelected.Count < masteryLimit)
        {
            masteriesSelected.Add(masteryPanelPrefab.mastery);
            UpdateCountText();
            masteryPanelPrefab.backgroundImage.enabled = true;
            return true;
        }
        return false;
    }

    public bool CheckIfMasterySelected(Mastery mastery)
    {
        return masteriesSelected.Exists(x => x == mastery);
    }

    public void RemoveMastery(MasteryPanelPrefab masteryPanelPrefab)
    {
        masteriesSelected.Remove(masteryPanelPrefab.mastery);
        masteryPanelPrefab.backgroundImage.enabled = false;
        UpdateCountText();
    }

    public void UpdateCountText()
    {
        masteryCountText.text = $@"( {masteriesSelected.Count} / {masteryLimit} )";
    }

    public bool CheckIfMasteryAtLimit()
    {
        if (masteriesSelected.Count == masteryLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShowTalentPreview(int masteryId)
    {
        masteryPreviewController.gameObject.SetActive(true);
        masteryPreviewController.SetMastery((long)masteryId, null);
    }
}
