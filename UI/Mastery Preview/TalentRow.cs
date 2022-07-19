using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TalentRow : MonoBehaviour
{
    public int rowNum;
    public int unlockNextRowNum;
    private List<TalentUIPrefab> talentUIPrefabs;
    public RectTransform BackgroundContainer;
    private int talentsAcquired;
    string talentRowPrefabPath = "Prefabs/UI Prefabs/Mastery Previews";

    public void Awake()
    {
        talentUIPrefabs = GetComponentsInChildren<TalentUIPrefab>().ToList();
    }

    public void SetTalents(CharacterMB characterMB)
    {
        int talentRankCount = 0;
        if (characterMB != null)
        {
            foreach (var talentInstance in characterMB.masteryController.GetTalentInstances())
            {
                if(talentUIPrefabs.Exists(x => x.talent == talentInstance.talent))
                {
                    talentRankCount += talentInstance.rank;
                }
            }
        }

        string talentRowBackgroundPath = talentRowPrefabPath + "/Talent Row Background";
        var talentBackgroundPrefab = Resources.Load<GameObject>(talentRowBackgroundPath);

        for (int i = 0; i < unlockNextRowNum; i++)
        {
            var instantiatedBackground = Instantiate(talentBackgroundPrefab, this.BackgroundContainer, false);
            var imageComp = instantiatedBackground.GetComponent<Image>();
            Debug.Log(imageComp);


            if (i > (talentRankCount-1))
            {
                var tempColor = imageComp.color;
                tempColor.a = 0.2f;
                imageComp.color = tempColor;
            }
            else
            {
                imageComp.enabled = false;
            }
        }
    }
}
