using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRepository : DbRepository, IRepository<Item>
{
    const string tableName = "item";
    const string fields = @"
                            item.id AS itemId,
                            item.value AS value,
                            item.stackable AS stackable,
                            item.icon AS icon,
                            ic.id AS itemCategoryId,
                            ic.name AS itemCategoryName,
                            ic.baseIdentifier AS itemCategoryBaseIdentifier,
                            itemNameIdentifier.id AS nameIdentifierId,
                            itemNameIdentifier.name AS nameIdentifierName,
                            descriptionIdentifier.id AS descriptionIdentifierId,
                            descriptionIdentifier.name AS descriptionIdentifierName";

    const string tableJoins = @"JOIN itemCategory ic ON ic.id = item.itemCategoryId
                                LEFT JOIN languageIdentifier itemNameIdentifier ON itemNameIdentifier.id = item.nameIdentifierId
                                LEFT JOIN languageIdentifier descriptionIdentifier ON descriptionIdentifier.id = item.descriptionIdentifierId";

    string orderBy = "item.id ASC";

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

    public List<Item> GetByCriteria(List<SqlClient.Expr> criteria = null)
    {
        var result = GetResult(criteria);
        var items = new List<Item>();

        foreach (var row in result)
        {
            items.Add(new Item(
                (long)row["itemId"],
                (float)row["value"],
                ItemCategory.GetCategoryByID((int)(long)row["itemCategoryId"]),
                (string)row["nameIdentifierName"],
                (string)row["descriptionIdentifierName"],
                (bool)row["stackable"],
                (string)row["icon"]
                ));
        }

        return items;
    }
}
