using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemUIPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform myRect;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemName;
    public Image itemImage;
    public ItemMouseOverTooltipController itemMouseOverTooltipController;
    public Item item;
    private ItemContextMenu itemContextMenu;
    private Canvas canvas;

    private void Awake()
    {
        canvas = this.GetComponentInParent<Canvas>();
    }

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

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            var itemContextMenuRect = itemContextMenu.GetComponent<RectTransform>();
            float offsetX = 5f;
            float offsetY = 5;
            Debug.Log("Width:"+ itemContextMenuRect.rect.width);
            float xPos = (pointerEventData.position.x /  canvas.scaleFactor) + (itemContextMenuRect.rect.width/2) + offsetX;
            float yPos = pointerEventData.position.y / canvas.scaleFactor + (itemContextMenuRect.rect.height / 2) + offsetY;

            Debug.Log(pointerEventData.position);
            
            itemContextMenuRect.anchoredPosition = new Vector2(xPos, yPos);
            itemContextMenu.gameObject.SetActive(true);
        }
    }

    public void SetItem(Item item, InventoryPanel inventoryPanel)
    {
        this.item = item;
        this.itemMouseOverTooltipController = inventoryPanel.itemMouseOverTooltipController;
        this.itemDescription.text = LanguageController.GetPhrase(item.descriptionIdentifier);
        this.itemName.text = LanguageController.GetPhrase(item.nameIdentifier);
        this.itemContextMenu = inventoryPanel.itemContextMenu;

        if (item.icon != null)
        {
            this.itemImage.sprite = Resources.Load<Sprite>(Constants.itemIconDirectory + "/" + item.icon);
        }
    }
}
