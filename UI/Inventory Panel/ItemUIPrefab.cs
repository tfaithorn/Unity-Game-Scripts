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

    private void Start()
    {
        this.myRect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemMouseOverTooltipController.SetItem(itemName.text, itemDescription.text);
        float xPos = this.myRect.position.x - (this.myRect.rect.width / 2) - itemMouseOverTooltipController.panel.rect.width + 15;
        float yPos = this.myRect.position.y - (this.myRect.rect.height / 2) - (itemMouseOverTooltipController.panel.rect.height / 2);
        itemMouseOverTooltipController.panel.anchoredPosition = new Vector2(xPos, yPos);
        itemMouseOverTooltipController.Activate();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemMouseOverTooltipController.Deactivate();
    }

    public void SetItem(Item newitem, ItemMouseOverTooltipController itemMouseOverTooltipController)
    {

        this.itemMouseOverTooltipController = itemMouseOverTooltipController;
        this.itemDescription.text = LanguageController.GetPhrase(newitem.descriptionIdentifier.name);
        this.itemName.text = LanguageController.GetPhrase(newitem.nameIdentifier.name);

        if (newitem.icon != null)
        {
            this.itemImage.sprite = Resources.Load<Sprite>(Item.iconDirectory+ newitem.icon);
        }

    }
}
