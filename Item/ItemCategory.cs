using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCategory
{
    long id;
    string name;
    string baseIdentifier;

    public ItemCategory(long id, string name, string baseIdenifier)
    {
        this.id = id;
        this.name = name;
        this.baseIdentifier = baseIdenifier;
    }

    public enum CategoryType
    {
        NONE = 0,
        WEAPON = 1,
        CONSUMABLE = 2,
        ARMOR = 3,
        MISC = 4
    }

    public static CategoryType GetCategoryByID(long id)
    {
        return (CategoryType)id;
    }

}
