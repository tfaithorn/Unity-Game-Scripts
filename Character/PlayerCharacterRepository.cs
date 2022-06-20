using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCharacterRepository : DbRepository, IRepository<PlayerCharacter>
{
    const string tableName = "player";
    const string fields = @"c.id AS id,
                            c.name AS name,
                            player.lastPlayed AS lastPlayed";
    const string tableJoins = "JOIN character c ON c.id = player.characterId";

    string orderBy = "name ASC";

    public override string GetFields()
    {
        return fields;
    }

    public override string GetTableJoins()
    {
        return tableJoins;
    }

    public override string GetTableName()
    {
        return tableName;
    }

    public override string GetOrderBy()
    {
        return orderBy;
    }

    public List<PlayerCharacter> GetByCriteria(List<SqlClient.Expr> criteria = null)
    {
        var result = GetResult(criteria);
        var players = new List<PlayerCharacter>();

        foreach (var row in result)
        {
            players.Add(new PlayerCharacter((long)row["id"], (string)row["name"], (DateTime)row["lastPlayed"]));
        }

        return players;
    }

}
