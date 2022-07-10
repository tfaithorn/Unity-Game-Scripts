using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase
{
    /// <summary>
    /// Loads all items into a dictionary at run time for easy access
    /// </summary>
    static ItemDatabase()
    {
        var itemRepository = new ItemRepository();
        var itemsAsList = itemRepository.GetByCriteria();

        foreach (Item item in itemsAsList)
        {
            items.Add(item.id, item);
        }
    }

    public static Dictionary<long, Item> items = new Dictionary<long, Item>() {
        
    };
}
