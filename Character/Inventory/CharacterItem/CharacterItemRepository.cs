using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterItemRepository : DbRepository, IRepository<CharacterItem>
{
    const string tableName = "characterItem";
    const string fields = @" 
                characterItem.quantity AS quantity,
                characterItem.equiptSlotId AS equiptSlotId,
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
                ic.baseIdentifier AS itemCategoryBaseIdentifier";

    const string tableJoins = @"            
            JOIN item i ON i.id = characterItem.itemId
            JOIN itemCategory ic ON ic.id = i.itemCategoryId
            LEFT JOIN languageIdentifier li ON li.id = i.nameIdentifierId
            LEFT JOIN languageIdentifier li1 ON li1.id = i.descriptionIdentifierId";

    string orderBy = "name ASC";

    public override string GetTableName()
    {
        return tableName;
    }

    public override string GetFields()
    {
        return fields;
    }

    public override string GetTableJoins()
    {
        return tableJoins;
    }

    public override string GetOrderBy()
    {
        return orderBy;
    }

    public List<CharacterItem> GetByCriteria(List<SqlClient.Expr> criteria)
    {
        var result = GetResult(criteria);
        var characterItems = new List<CharacterItem>();

        foreach (var row in result)
        {
            characterItems.Add(
                new CharacterItem(
                    new Item(
                        (long)row["itemId"],
                        (float)row["value"],
                        ItemCategory.GetCategoryByID((long)row["itemCategoryId"]),
                        new LanguageIdentifier(
                            (long)row["nameIdentifierId"], 
                            (string)row["nameIdentifierName"]
                        ),
                        new LanguageIdentifier(
                            (long)row["descriptionIdentifierId"], 
                            (string)row["descriptionIdentifierName"]
                        ),
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
