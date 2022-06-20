using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterItem
{
    public Item item;
    public int equiptSlotId;
    public int quantity;

    public CharacterItem(Item item, int equiptSlotId, int quantity)
    {
        this.item = item;
        this.equiptSlotId = equiptSlotId;
        this.quantity = quantity;
    }
}
