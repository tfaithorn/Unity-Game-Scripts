using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemCharacter
{
    public Item item;
    public int equiptSlotId;
    public int quantity;

    public ItemCharacter(Item item, int equiptSlotId, int quantity)
    {
        this.item = item;
        this.equiptSlotId = equiptSlotId;
        this.quantity = quantity;
    }
}
