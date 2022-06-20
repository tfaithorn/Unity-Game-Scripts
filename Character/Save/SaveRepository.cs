using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveRepository : DbRepository, IRepository<Save>
{
    const string tableName = "save";
    const string fields = @"
                    save.id AS id,
                    save.name AS name,
                    save.createdAt AS createdAt,
                    c.name AS playerName,
                    c.id AS playerId,
                    p.lastPlayed AS lastPlayed,
                    save.saveData AS saveData,
                    save.previewImagePath AS previewImagePath";

    const string tableJoins = @"JOIN player p ON p.characterId = save.playerId                   
                                JOIN character c ON c.id = p.characterId";
    string orderBy = "createdAt DESC";

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

    public List<Save> GetByCriteria(List<SqlClient.Expr> criteria = null)
    {
        var result = GetResult(criteria);
        var saves = new List<Save>();

        foreach (var row in result)
        {
            saves.Add(
                new Save(
                    (long)row["id"],
                    (string)row["name"],
                    (DateTime)row["createdAt"],
                    new PlayerCharacter(
                        (long)row["playerId"],
                        (string)row["playerName"],
                        (DateTime)row["lastPlayed"]
                    ),
                    (string)row["saveData"],
                    (string)row["previewImagePath"]
                )
            );
        }

        return saves;
    }
}
