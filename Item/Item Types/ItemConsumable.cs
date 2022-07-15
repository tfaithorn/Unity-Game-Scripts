using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConsumable : Item
{
    public bool consumeOnUse = false;
    public ItemUseableInterface itemUseableInterface;

    public ItemConsumable(Item item) : base(item)
    {

    }
}
