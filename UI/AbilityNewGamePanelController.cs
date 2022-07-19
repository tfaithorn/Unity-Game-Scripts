using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityNewGamePanelController : MonoBehaviour
{
    public AbilityPreviewTooltipController abilityPreviewTooltipController;
    public TextMeshProUGUI countText;
    private int abilityLimit = 2;
    private List<Ability> abilities = new List<Ability>();
    private Canvas canvas;
    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();        
    }

    public void SelectAbility(AbilityPreviewPanel abilityPreviewPanel)
    {

    }

    public void SetAbilityTooltip(AbilityPreviewPanel abilityPreviewPanel)
    {
        var previewPanelRect = abilityPreviewPanel.GetComponent<RectTransform>();

        float xOffset = 0;
        float xPos = ((previewPanelRect.position.x * (1 / canvas.scaleFactor)) + (previewPanelRect.rect.width*2)) + xOffset;
        float yPos = (previewPanelRect.position.y * (1 / canvas.scaleFactor));// + (previewPanelRect.rect.height / 2));

        abilityPreviewTooltipController.ActivateTooltip(abilityPreviewPanel.ability, new Vector2(xPos, yPos));
    }

    public void HideAbilityPreviewTooltipController()
    {
        abilityPreviewTooltipController.DeactivateTooltip();
    }

    public bool CheckIfAbilityAtLimit()
    {
        if (abilities.Count < abilityLimit)
        {
            return true;
        }

        return false;
    }

    public bool CheckIfAbilitySelected(Ability ability)
    {
        if (abilities.Exists(x => x == ability))
        {
            return true;
        }
        return false;
    }

    public void RemoveAbility(Ability ability)
    { 
        
    }
}
