using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemCache
{
    /// <summary>
    /// Loads all items into a dictionary at run time for easy access
    /// </summary>
    /// 
    private static Dictionary<long, Item> items;

    static ItemCache()
    {
        items = new Dictionary<long, Item>() { };
        var itemRepository = new ItemRepository();
        var itemsAsList = itemRepository.GetByCriteria();

        foreach (Item item in itemsAsList)
        {
            items.Add(item.id, item);
        }
    }

    public static Item GetItem(long id)
    {
        if (items.ContainsKey(id))
        {
            return items[id];
        }
        else
        {
            return null;
        }
    }
}
