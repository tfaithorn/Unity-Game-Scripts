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


    private void Awake()
    {
        masteryLimit = 2;
        this.masteriesSelected = new List<Mastery>();
        masteryCountText.text = $@"({masteriesSelected.Count}/{masteryLimit}";
    }

    public bool SelectMastery(Mastery mastery)
    {
        if (masteriesSelected.Count <= masteryLimit)
        {
            masteriesSelected.Add(mastery);
            Debug.Log($@"({masteriesSelected.Count}/{masteryLimit})");
            masteryCountText.text = $@"({masteriesSelected.Count}/{masteryLimit}";
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
    }
}
