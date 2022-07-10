using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCharacterRepository : DbRepository, IRepository<ItemCharacter>
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

    public List<ItemCharacter> GetByCriteria(List<SqlClient.Expr> criteria)
    {
        var result = GetResult(criteria);
        var inventory = new List<ItemCharacter>();

        foreach (var row in result)
        {
            inventory.Add(
                new ItemCharacter(
                    ItemDatabase.items[(long)row["itemId"]],
                    (int)row["equiptSlotId"],
                    (int)row["quantity"]
                )
            );
        }

        return inventory;
    }
}
