using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityPreviewTooltipController : MonoBehaviour
{
    public AbilityPreviewTooltip abilityPreviewTooltip;

    public void ActivateTooltip(Ability ability, Vector2 anchoredPosition)
    {
        abilityPreviewTooltip.gameObject.SetActive(true);
        abilityPreviewTooltip.SetValues(ability.name, ability.GetDescription(null));
        
        var toolTipRecttransform = abilityPreviewTooltip.GetComponent<RectTransform>();
        toolTipRecttransform.anchoredPosition = anchoredPosition;
    }

    public void DeactivateTooltip()
    {
        abilityPreviewTooltip.gameObject.SetActive(false);
    }
}
