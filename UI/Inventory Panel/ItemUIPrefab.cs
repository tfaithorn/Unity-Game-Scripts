using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemUIPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform myRect;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemName;
    public Image itemImage;
    public ItemMouseOverTooltipController itemMouseOverTooltipController;
    public Canvas canvas;

    private void Start()
    {
        this.myRect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        float xOffset = -3;

        float xPos = ((this.myRect.position.x * (1 / canvas.scaleFactor)) - (itemMouseOverTooltipController.panel.rect.width)) + xOffset;
        float yPos = (this.myRect.position.y * (1 / canvas.scaleFactor) + (itemMouseOverTooltipController.panel.rect.height / 2) - this.myRect.rect.height);

        itemMouseOverTooltipController.panel.anchoredPosition = new Vector2(xPos, yPos);
        itemMouseOverTooltipController.Activate();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemMouseOverTooltipController.Deactivate();
    }

    public void SetItem(Item newitem, ItemMouseOverTooltipController itemMouseOverTooltipController, Canvas canvas)
    {
        this.canvas = canvas;
        this.itemMouseOverTooltipController = itemMouseOverTooltipController;
        this.itemDescription.text = LanguageController.GetPhrase(newitem.descriptionIdentifier);
        this.itemName.text = LanguageController.GetPhrase(newitem.nameIdentifier);

        if (newitem.icon != null)
        {
            this.itemImage.sprite = Resources.Load<Sprite>(Item.iconDirectory+ newitem.icon);
        }

    }
}
