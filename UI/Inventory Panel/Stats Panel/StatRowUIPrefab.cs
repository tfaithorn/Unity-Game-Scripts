using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StatRowUIPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Canvas canvas;
    public StatMouseOverTooltipController statMouseOverTooltipController;
    public string labelIdentifier;
    public string descriptionIdentifier;
    public StatsController.StatType statType;
    public StatsController statsController;
    public TextMeshProUGUI name;
    public TextMeshProUGUI value;
    private RectTransform myRect;

    private void Awake()
    {
        myRect = GetComponent<RectTransform>();
    }

    public void Start()
    {
        this.name.text = LanguageController.GetPhrase(labelIdentifier);
        this.value.text = statsController.GetStatValue(statType).ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        statMouseOverTooltipController.SetTooltip(LanguageController.GetPhrase(descriptionIdentifier));
        float xPos = (this.myRect.position.x * (1 / canvas.scaleFactor)) - (this.myRect.rect.width / 2) - statMouseOverTooltipController.panel.rect.width;
        float yPos = (this.myRect.position.y * (1 / canvas.scaleFactor)) - (this.myRect.rect.height / 2) - (statMouseOverTooltipController.panel.rect.height / 2);
        statMouseOverTooltipController.panel.anchoredPosition = new Vector2(xPos, yPos);
        statMouseOverTooltipController.Activate();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statMouseOverTooltipController.Deactivate();
    }
}
