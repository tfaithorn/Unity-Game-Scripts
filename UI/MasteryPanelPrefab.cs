using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MasteryPanelPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public long masteryId;
    public MasteryNewGamePanelController masteryNewGamePanelController;
    public Image backgroundImage;
    public Image displayImage;
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
        var sprite = Resources.Load<Sprite>(Constants.masteryPreviewImagesPath + "/" + mastery.previewImage);
        displayImage.sprite = sprite;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {

        if (state != State.SELECTED)
        {
            state = State.HOVER;
            displayImage.color = new Color(displayImage.color.r, displayImage.color.g, displayImage.color.b, 1f);
        }

        masteryNewGamePanelController.SetMasteryDescription(mastery);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (state != State.SELECTED)
        {
            SetInactive();
        }

        masteryNewGamePanelController.ClearMasteryDescription();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {

        if (masteryNewGamePanelController.CheckIfMasterySelected(mastery))
        {
            masteryNewGamePanelController.RemoveMastery(this);
            SetInactive();
        }
        else if (!masteryNewGamePanelController.CheckIfMasteryAtLimit())
        {
            masteryNewGamePanelController.SelectMastery(this);
            SetSelected();
        }
    }

    private void SetInactive()
    {
        state = State.INACTIVE;
        displayImage.color = new Color(displayImage.color.r, displayImage.color.g, displayImage.color.b, 0.5f);
    }

    private void SetSelected()
    {
        state = State.SELECTED;
        displayImage.color = new Color(displayImage.color.r, displayImage.color.g, displayImage.color.b, 1f);
    }
}
