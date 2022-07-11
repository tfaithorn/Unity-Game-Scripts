using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerRepository : DbRepository, IRepository<Player>
{
    const string tableName = "player";
    const string fields = @"player.id AS id,
                            player.name AS name,
                            player.lastPlayed AS lastPlayed";
    const string tableJoins = "";

    string orderBy = "player.name ASC";

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

    public Player AddNewPlayer(string name)
    {
        var values = new Dictionary<string, object>()
        {
            {"name", name}
        };
        var playerId = Insert(values, true);

        var criteria = new List<SqlClient.Expr>()
        {
            new SqlClient.Cond("player.id", playerId, SqlClient.OP_EQUAL)
        };

        return GetByCriteria(criteria)[0];
    }

    public List<Player> GetPlayers()
    {
        return GetByCriteria();
    }

    private List<Player> GetByCriteria(List<SqlClient.Expr> criteria = null)
    {
        var result = GetResult(criteria);
        var players = new List<Player>();

        foreach (var row in result)
        {
            players.Add(new Player((long)row["id"], (string)row["name"], (DateTime)row["lastPlayed"]));
        }

        return players;
    }

}
