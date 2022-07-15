using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    protected List<ItemInstance> inventory = new List<ItemInstance>();
    protected CharacterMB character;

    public void AddToInventory(Item item, int quantity = 1)
    {
        ItemInstance characterItem = inventory.SingleOrDefault(x => x.item.id == item.id);

        //TODO, redo to not be stupid and put in stacklogic
        inventory.Add(new ItemInstance(item, 0, quantity));
    }

    public void AddToInventoryWithoutCheck(ItemInstance itemInstance)
    {
        inventory.Add(itemInstance);
    }

    /// <summary>
    /// Completely removes an item from your inventory. Do not use to lower quantity
    /// </summary>
    /// <param name="item"></param>
    public void RemoveFromInventory(Item item)
    {
        ItemInstance itemInstance = inventory.SingleOrDefault(x => x.item.id == item.id);
        inventory.Add(itemInstance);
    }

    public List<ItemInstance> GetItems()
    {
        return this.inventory;
    }

    /// <summary>
    /// Use an item. If it is a consumable quanitity is consumed. Removes item at 0 quantity
    /// </summary>
    /// <param name="item"></param>
    public void UseItem(Item item)
    {
        if (item is ItemConsumable)
        {
            ItemConsumable itemConsumable =  (ItemConsumable)item;
            itemConsumable.itemUseableInterface.Use(character);
        }
    }

    public List<ItemInstance> GetByType(ItemCategory.CategoryType itemCategory)
    {
        if (itemCategory == ItemCategory.CategoryType.NONE)
        {
            return inventory.OrderBy(x => LanguageController.GetPhrase(x.item.nameIdentifier)).ToList();
        }

        return inventory
            .FindAll(x => x.item.categoryType == itemCategory)
            .OrderBy(x => LanguageController.GetPhrase(x.item.nameIdentifier))
            .ToList();
    }

}
