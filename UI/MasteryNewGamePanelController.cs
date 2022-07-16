using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MasteryNewGamePanelController : MonoBehaviour
{
    public List<MasteryPanelPrefab> masteryPanelPrefabs;
    public TextMeshProUGUI masteryCountText;
    private List<Mastery> masteriesSelected;
    private int masteryLimit;
    public TextMeshProUGUI masteryTitleText;
    public TextMeshProUGUI masteryDescriptionText;

    private void Awake()
    {
        masteryLimit = 2;
        this.masteriesSelected = new List<Mastery>();
        masteryCountText.text = $@"({masteriesSelected.Count}/{masteryLimit}";
    }

    public bool SelectMastery(Mastery mastery)
    {
        if (masteriesSelected.Count < masteryLimit)
        {
            masteriesSelected.Add(mastery);
            UpdateCountText();
            return true;
        }
        return false;
    }

    public bool CheckIfMasterySelected(Mastery mastery)
    {
        return masteriesSelected.Exists(x => x == mastery);
    }

    public void RemoveMastery(Mastery mastery)
    {
        masteriesSelected.Remove(mastery);
        UpdateCountText();
    }

    public void UpdateCountText()
    {
        masteryCountText.text = $@"( {masteriesSelected.Count} / {masteryLimit} )";
    }

    public void SetMasteryDescription(Mastery mastery)
    {
        masteryTitleText.text = mastery.name;
        masteryDescriptionText.text = mastery.description;
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

    public void ClearMasteryDescription()
    {
        masteryTitleText.text = "";
        masteryDescriptionText.text = "";
    }
}
