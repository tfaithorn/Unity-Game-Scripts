using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityPreviewPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public long abilityId;
    public AbilityNewGamePanelController abilityNewGamePanelController;
    public Image backgroundImage;
    public Image displayImage;
    public Ability ability;
    public State state;
    public enum State
    {
        HOVER,
        SELECTED,
        INACTIVE
    }

    private void Awake()
    {
        ability = AbilityCache.GetAbility(abilityId);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {

        if (state != State.SELECTED)
        {
            state = State.HOVER;
            displayImage.color = new Color(displayImage.color.r, displayImage.color.g, displayImage.color.b, 1f);
        }

        abilityNewGamePanelController.SetAbilityTooltip(this);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (state != State.SELECTED)
        {
            SetInactive();
        }

        abilityNewGamePanelController.HideAbilityPreviewTooltipController();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {

        if (abilityNewGamePanelController.CheckIfAbilitySelected(this.ability))
        {
            abilityNewGamePanelController.RemoveAbility(this.ability);
            SetInactive();
        }
        else if (!abilityNewGamePanelController.CheckIfAbilityAtLimit())
        {
            abilityNewGamePanelController.SelectAbility(this);
            SetSelected();
        }
    }

    private void SetInactive()
    {
        displayImage.color = new Color(displayImage.color.r, displayImage.color.g, displayImage.color.b, 0.5f);
    }

    private void SetSelected()
    {
        displayImage.color = new Color(displayImage.color.r, displayImage.color.g, displayImage.color.b, 1f);
    }
}
