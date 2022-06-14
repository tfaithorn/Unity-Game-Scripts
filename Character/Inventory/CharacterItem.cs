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

    public static List<CharacterItem> GetByCriteria(List<SqlClient.Expr> criteria)
    {
        string sql = @"
            SELECT 
                ci.quantity AS quantity,
                ci.equiptSlotId AS equiptSlotId,
                i.id AS itemId,
                i.nameIdentifierId,
                li.name AS nameIdentifierName,
                i.descriptionIdentifierId,
                li1.name AS descriptionIdentifierName,
                i.name AS name,
                i.value AS value,
                i.stackable AS stackable,
                i.icon AS icon,
                ic.id AS itemCategoryId,
                ic.name AS itemCategoryName,
                ic.baseIdentifier AS itemCategoryBaseIdentifier
            FROM characterItem ci
            JOIN item i ON i.id = ci.itemId
            JOIN itemCategory ic ON ic.id = i.itemCategoryId
            LEFT JOIN languageIdentifier li ON li.id = i.nameIdentifierId
            LEFT JOIN languageIdentifier li1 ON li1.id = i.descriptionIdentifierId
            {criteria}";

        var paramGroup = new SqlClient.ParamGroup();
        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);

        sql = SqlClient.ReplaceToken(sql, "criteria", preparedWhere);
        var result = SqlClient.Execute(sql, paramGroup);

        List<CharacterItem> characterItems = new List<CharacterItem>();

        foreach (Dictionary<string, object> row in result)
        {
            characterItems.Add( 
                new CharacterItem(
                    new Item(
                        (long)row["itemId"],
                        (float)row["value"],
                        ItemCategory.GetCategoryByID((long)row["itemCategoryId"]),
                        new LanguageIdentifier((long)row["nameIdentifierId"], (string)row["nameIdentifierName"]),
                        new LanguageIdentifier((long)row["descriptionIdentifierId"], (string)row["descriptionIdentifierName"]),
                        (bool)row["stackable"],
                        (string)row["icon"]
                    ),
                    (int)row["equiptSlotId"],
                    (int)row["quantity"]
                    )
                );
        }

        return characterItems;
    }

}
