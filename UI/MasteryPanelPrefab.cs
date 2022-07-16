using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MasteryPanelPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public long masteryId;
    public TextMeshProUGUI masteryDescription;
    public MasteryNewGamePanelController masteryNewGamePanelController;
    private Image image;
    public Mastery mastery;
    public State state;
    public enum State {
        HOVER,
        SELECTED,
        INACTIVE
    }

    public void Awake()
    {
        mastery = MasteryCache.GetMastery(masteryId);
        image = GetComponent<Image>();
        var sprite = Resources.Load<Sprite>(Constants.masteryPreviewImagesPath + "/" + mastery.previewImagePath);
        Debug.Log(Constants.masteryPreviewImagesPath + "/" + mastery.previewImagePath);
        image.sprite = sprite;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {

        if (state != State.SELECTED)
        {
            state = State.HOVER;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }

        masteryDescription.text = mastery.description;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (state != State.SELECTED)
        {
            SetInactive();
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {

        if (masteryNewGamePanelController.CheckIfMasterySelected(mastery))
        {
            masteryNewGamePanelController.RemoveMastery(mastery);
            SetInactive();
        }
        else
        {
            masteryNewGamePanelController.SelectMastery(mastery);
            SetSelected();
        }
    }

    private void SetInactive()
    {
        state = State.INACTIVE;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
    }

    private void SetSelected()
    {
        state = State.SELECTED;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }
}
