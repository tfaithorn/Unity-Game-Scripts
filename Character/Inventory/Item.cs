using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public readonly long id;
    public ItemCategory.CategoryType categoryType;
    public LanguageIdentifier descriptionIdentifier;
    public LanguageIdentifier nameIdentifier;
    public float value;
    public bool stackable;
    public string icon;

    public const string iconDirectory = "Item Icons/";
    public Item(){}

    public Item(Item item) 
    {
        this.id = item.id;
        this.nameIdentifier = item.nameIdentifier;
        this.descriptionIdentifier = item.descriptionIdentifier;
        this.categoryType = item.categoryType;
        this.value = item.value;
        this.stackable = item.stackable;
        this.icon = item.icon;
    }

    public Item(long id, float value, ItemCategory.CategoryType categoryType, LanguageIdentifier nameIdentifier, LanguageIdentifier descriptionIdentifier, bool stackable, string icon)
    {
        this.id = id;
        this.value = value;
        this.categoryType = categoryType;
        this.descriptionIdentifier = descriptionIdentifier;
        this.stackable = stackable;
        this.icon = icon;
        this.nameIdentifier = nameIdentifier;
    }
}
