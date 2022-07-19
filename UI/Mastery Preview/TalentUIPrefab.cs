using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TalentUIPrefab : MonoBehaviour
{
    public long talentId;
    public Talent talent;
    public TalentUIPrefabImage talentUIPrefabImage;
    public TextMeshProUGUI rankText;

    private void Awake()
    {
        this.talent = TalentCache.GetTalent(talentId);
        SetTalentUIPrefab(this.talent, null);
    }

    public void SetTalentUIPrefab(Talent talent, CharacterMB characterMB)
    {
        var image = talentUIPrefabImage.GetComponent<Image>();
        var iconSprite = Resources.Load<Sprite>(Constants.talentIconPath + "/" + this.talent.GetIconFileName(characterMB));
        image.sprite = iconSprite;


        int rankCount = 0;

        if (characterMB != null)
        {
            if (characterMB.masteryController != null)
            {
                var talentInstance = characterMB.masteryController.GetTalentInstances().Find(x => x.talent == talent);
                rankCount = talentInstance.rank;
            }
        }


        if (rankCount == 0)
        {
            var tempColor = image.color;
            tempColor.a = 0.5f;
            image.color = tempColor;
        }



        rankText.text = $"( {rankCount} / {talent.maxRank} )";
    }
}
