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
                    save.saveData AS saveData,
                    save.sceneId AS sceneId,
                    p.name AS playerName,
                    p.id AS playerId,
                    p.lastPlayed AS lastPlayed";

    const string tableJoins = @"JOIN player p ON p.id = save.playerId";
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

    public void OverrideSave(Save save)
    {
        var saveValues = new Dictionary<string, object>()
        {
            { "name", save.name},
            { "createdAt", SqlClient.Function.CURRENT_TIMESTAMP},
            { "saveData", save.saveData},
            { "sceneId", save.sceneId},
        };

        var criteria = new List<SqlClient.Expr>()
        {
            new SqlClient.Cond("id", save.id, SqlClient.OP_EQUAL)
        };

        Update(saveValues, criteria);
    }

    public Save GetSaveById(long id)
    {
        var criteria = new List<SqlClient.Expr>()
        {
            new SqlClient.Cond("save.id", id, SqlClient.OP_EQUAL)
        };

        return GetByCriteria(criteria)[0];
    }

    public Save NewSave(long playerId, string name, string saveData, long sceneId)
    {
        //Note: This should really be done in a transaction with the saveCharacter data
        var saveValues = new Dictionary<string, object>()
        {
            { "playerId", playerId},
            { "name", name},
            { "saveData", saveData},
            { "sceneId", sceneId}
        };

        var saveId = Insert(saveValues, true);

        var criteria = new List<SqlClient.Cond>() {
            {new SqlClient.Cond("id", saveId, SqlClient.OP_EQUAL)}
        };

        return GetByCriteria()[0];
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
                    PlayerDatabase.GetPlayer((long)row["playerId"]),
                    (string)row["saveData"],
                    (long)row["sceneId"]
                )
            );
        }

        return saves;
    }

    public List<Save> GetSavesByPlayerId(long id)
    {
        var critera = new List<SqlClient.Expr>()
        {
            new SqlClient.Cond("playerId", id, SqlClient.OP_EQUAL)
        };

        return GetByCriteria(critera);
    }
}
