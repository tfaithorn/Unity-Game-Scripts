using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public Item item;
    public int amount;
    public bool isEquipt;

    public InventoryItem(Item item) 
    {
        amount = 1;
        this.item = item;
    }
}
