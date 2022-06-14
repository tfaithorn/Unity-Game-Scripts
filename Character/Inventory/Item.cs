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

    public static List<Item> GetByCriteria(List<SqlClient.Expr> criteria)
    {
        string sql = @"SELECT 
                            i.id AS itemId,
                            i.name AS name,
                            i.value AS value,
                            ic.id AS itemCategoryId,
                            ic.name AS itemCategoryName,
                            ic.baseIdentifier AS itemCategoryBaseIdentifier,
                            li.nameIdentifierId,
                            li.descriptionIdentifierId,
                            li1.id AS descriptionIdentifierId,
                            li1.name AS descriptionIdentifier
                       FROM item i 
                       JOIN itemCategory ic ON ic.id = i.categoryTypeId
                       LEFT JOIN languageIdentifier li ON li.id = i.nameIdentifierId
                       LEFT JOIN languageIdentifier li1 ON li1.id = i.descriptionIdentifierId
                       {criteria}";

        var paramGroup = new SqlClient.ParamGroup();

        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);

        sql = SqlClient.ReplaceToken(sql,"criteria", preparedWhere);

        var result =  SqlClient.Execute(sql, paramGroup);

        List<Item> items = new List<Item>();
        foreach (Dictionary<string, object> row in result)
        {
            items.Add(new Item(
                (long)row["itemId"], 
                (float)row["value"],
                (ItemCategory.CategoryType)(int)row["itemCategoryId"],
                new LanguageIdentifier((long)row["nameIdentifierId"], (string)row["descriptionIdentifier"]),
                new LanguageIdentifier((long)row["descriptionIdentifierId"], (string)row["descriptionIdentifier"]),
                (bool)row["stackable"],
                (string)row["icon"]
                )
            );
        }

        return items;
    }

}
