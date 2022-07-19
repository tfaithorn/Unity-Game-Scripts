using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TalentPreviewSection : MonoBehaviour
{
    private List<TalentRow> talentRows;

    private void Awake()
    {
        talentRows = GetComponentsInChildren<TalentRow>().ToList();
    }

    public void SetTalents(CharacterMB characterMB)
    {
        foreach (TalentRow talentRow in talentRows)
        {
            talentRow.SetTalents(characterMB);
        }
    }
}
