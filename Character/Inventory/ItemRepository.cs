using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRepository : DbRepository, IRepository<Item>
{
    const string tableName = "item";
    const string fields = @"
                            item.id AS itemId,
                            item.name AS name,
                            item.value AS value,
                            ic.id AS itemCategoryId,
                            ic.name AS itemCategoryName,
                            ic.baseIdentifier AS itemCategoryBaseIdentifier,
                            li.nameIdentifierId,
                            li.descriptionIdentifierId,
                            li1.id AS descriptionIdentifierId,
                            li1.name AS descriptionIdentifier";

    const string tableJoins = @"JOIN itemCategory ic ON ic.id = item.categoryTypeId
                                LEFT JOIN languageIdentifier li ON li.id = item.nameIdentifierId
                                LEFT JOIN languageIdentifier li1 ON li1.id = item.descriptionIdentifierId";

    string orderBy = "item.name ASC";

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

    public List<Item> GetByCriteria(List<SqlClient.Expr> criteria)
    {
        var result = GetResult(criteria);
        var items = new List<Item>();

        foreach (var row in result)
        {
            items.Add(new Item(
                (long)row["itemId"],
                (float)row["value"],
                (ItemCategory.CategoryType)(int)row["itemCategoryId"],
                new LanguageIdentifier((long)row["nameIdentifierId"], (string)row["descriptionIdentifier"]),
                new LanguageIdentifier((long)row["descriptionIdentifierId"], (string)row["descriptionIdentifier"]),
                (bool)row["stackable"],
                (string)row["icon"]
                ));
        }

        return items;
    }
}
