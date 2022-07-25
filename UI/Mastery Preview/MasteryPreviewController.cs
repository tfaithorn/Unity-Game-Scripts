using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MasteryPreviewController : MonoBehaviour
{
    public long masteryId;
    public Image masteryPreviewImage;
    public Mastery mastery;
    public PlayerCharacterMB playerCharacterMB;
    public TextMeshProUGUI masteryDescription;
    public TextMeshProUGUI masteryNameText;
    public RectTransform talentPreviewContainer;
    public RectTransform background;
    private string masteryTalentPreviewPath = "Prefabs/UI Prefabs/Mastery Previews";

    private void Awake()
    {
        if (masteryId != 0)
        {
            //SetMastery(masteryId);
        }
    }

    private void OnEnable()
    {
        if (background != null)
        {
            background.gameObject.SetActive(true);
        }
    }

    public void SetMastery(long masteryId, CharacterMB characterMB)
    {
        this.mastery = MasteryCache.GetMastery(masteryId);
        this.masteryPreviewImage.sprite = Resources.Load<Sprite>(Constants.masteryPreviewImagesPath + "/" + mastery.previewImage);
        this.masteryDescription.text = this.mastery.description;
        this.masteryNameText.text = mastery.name;

        if (mastery is ManAtArms)
        {
            var talentPreview = Resources.Load<TalentPreviewSection>(masteryTalentPreviewPath + "/ManAtArms Talents");
            Debug.Log(talentPreview);
            SetTalentPreview(talentPreview, characterMB);
        }
    }

    private void SetTalentPreview(TalentPreviewSection talentPreviewSection, CharacterMB characterMB)
    {
        var instantiatedTalentSection = Instantiate(talentPreviewSection, talentPreviewContainer, false);
        instantiatedTalentSection.SetTalents(characterMB);
    }

    public void HidePreviewController()
    {
        if (background != null)
        {
            background.gameObject.SetActive(false);
        }

        this.gameObject.SetActive(false);
    }
}
