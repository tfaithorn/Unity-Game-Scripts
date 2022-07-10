using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    protected List<ItemCharacter> inventory = new List<ItemCharacter>();
    protected CharacterMB character;

    public void AddToInventory(Item item, int quantity = 1)
    {
        ItemCharacter characterItem = inventory.SingleOrDefault(x => x.item.id == item.id);

        //TODO, redo to not be stupid and put in stacklogic
        inventory.Add(new ItemCharacter(item, 0, quantity));
    }

    public void AddToInventoryWithoutCheck(ItemCharacter itemCharacter)
    {
        inventory.Add(itemCharacter);
    }

    /// <summary>
    /// Completely removes an item from your inventory. Do not use to lower quantity
    /// </summary>
    /// <param name="item"></param>
    public void RemoveFromInventory(Item item)
    {
        ItemCharacter itemCharacter = inventory.SingleOrDefault(x => x.item.id == item.id);
        inventory.Add(itemCharacter);
    }

    public List<ItemCharacter> GetInventory()
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

    public List<ItemCharacter> GetByType(ItemCategory.CategoryType itemCategory)
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
