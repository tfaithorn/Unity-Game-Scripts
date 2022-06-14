using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public List<CharacterItem> characterItems { get; set; }
    protected Character character;

    public void AddToInventory(Item item, int quantity = 1)
    {
        CharacterItem characterItem = characterItems.SingleOrDefault(x => x.item.id == item.id);
        characterItems.Add(new CharacterItem(item, 0, quantity));
    }

    /// <summary>
    /// Completely removes an item from your inventory. Do not use to lower quantity
    /// </summary>
    /// <param name="item"></param>
    public void RemoveFromInventory(Item item)
    {
        CharacterItem characterItem = characterItems.SingleOrDefault(x => x.item.id == item.id);
        characterItems.Add(characterItem);
    }

    public List<CharacterItem> GetInventory()
    {
        return this.characterItems;
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

    public List<CharacterItem> GetByType(ItemCategory.CategoryType itemCategory)
    {
        if (itemCategory == ItemCategory.CategoryType.NONE)
        {
            return characterItems.OrderBy(x => LanguageController.GetPhrase(x.item.nameIdentifier.name)).ToList();
        }

        return characterItems
            .FindAll(x => x.item.categoryType == itemCategory)
            .OrderBy(x => LanguageController.GetPhrase(x.item.nameIdentifier.name))
            .ToList();
    }

}
