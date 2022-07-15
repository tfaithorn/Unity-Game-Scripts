using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContextMenu : MonoBehaviour
{
    public ItemContextMenuRow useRow;
    public ItemContextMenuRow equiptRow;
    public Item item;

    public void SetItemContextMenu(Item item)
    {
        if (item is ItemArmour || item is ItemWeapon)
        {
            equiptRow.gameObject.SetActive(true);
            equiptRow.SetItemContextRow(item);
        }

        if (item is ItemConsumable)
        {
            useRow.gameObject.SetActive(true);
            useRow.SetItemContextRow(item);
        }
    }
}
