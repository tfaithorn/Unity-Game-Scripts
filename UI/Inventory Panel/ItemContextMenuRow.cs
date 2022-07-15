using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemContextMenuRow : MonoBehaviour, IPointerClickHandler
{
    public enum State
    {
        USE,
        EQUIPT
    }

    public State state;
    public PlayerCharacterMB playerCharacterMB;
    public ItemContextMenu itemContextMenu;
    private Item item;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("You left-clicked this button!" + item.id);
            itemContextMenu.gameObject.SetActive(false);
        }
    }

    public void SetItemContextRow(Item item)
    {
        this.item = item;
    }

    
}
