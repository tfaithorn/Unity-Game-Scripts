using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveCharacterRepository : DbRepository, IRepository<SaveCharacter>
{
    const string tableName = "saveCharacter";
    const string fields = @"save.playerId AS playerId,
                            save.id AS saveId,
                            save.name AS saveName,
                            save.createdAt AS createdAt,
                            character.id AS characterId,
                            character.name AS characterName,
                            character.prefabPath AS prefabPath,
                            saveCharacter.sceneId AS sceneId,
                            saveCharacter.saveData AS saveData";
    const string tableJoins = @"JOIN save ON save.id = saveCharacter.saveId
                                JOIN character ON character.id = saveCharacter.characterId
                                LEFT JOIN npcCharacter ON npcCharacter.characterId = character.id";

    string orderBy = "save.createdAt DESC";

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

    public List<SaveCharacter> LoadNpcCharactersForScene(long sceneId, long playerId)
    {
        var criteria = new List<SqlClient.Expr>()
        {
            new SqlClient.Cond("save.playerId", playerId, SqlClient.OP_EQUAL),
            new SqlClient.Cond("save.sceneId", sceneId, SqlClient.OP_EQUAL),
        };

        var sql = @"SELECT
                        save.id AS saveId,
                        character.prefabPath AS prefabPath,
                        saveCharacter.characterId AS characterId,
                        character.name AS characterName,
                        save.sceneId AS sceneId,
                        saveCharacter.saveData AS saveData
                    FROM save 
                    JOIN saveCharacter ON saveCharacter.saveId = save.id  
                    JOIN character ON character.id = saveCharacter.characterId
                    {criteria}
                    ORDER BY 
                        save.createdAt DESC
                    LIMIT 1";

        SqlClient.ParamGroup paramGroup = new SqlClient.ParamGroup();
        var preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);
        sql = SqlClient.ReplaceToken(sql,preparedWhere, "{criteria}");
        var result = SqlClient.Execute(sql,paramGroup);
        var saveCharacters = new List<SaveCharacter>();

        foreach (var row in result)
        {
            saveCharacters.Add(
                new SaveCharacter(
                    SaveDatabase.GetSave((long)row["saveId"]),
                    new NpcCharacter((long)row["characterId"], (string)row["characterName"], (string)row["prefabPath"]),
                    (long)row["sceneId"],
                    (string)row["saveData"])
                );
        }

        return saveCharacters;
    }

    public List<SaveCharacter> GetByCriteria(List<SqlClient.Expr> criteria = null)
    {
        var result = GetResult(criteria);
        var saveCharacters = new List<SaveCharacter>();

        foreach (var row in result)
        {
            Character character = null;

            saveCharacters.Add(new SaveCharacter(
                SaveDatabase.GetSave((long)row["saveId"]),
                new NpcCharacter(
                        (long)row["characterId"],
                        (string)row["characterName"],
                        (string)row["prefabPath"]
                        ),
                    (long)row["sceneId"],
                    (string)row["saveData"]
                ));
        }

        return saveCharacters;
    }

    public void SaveCharacter(string characterSaveData, long characterId, long saveId, long sceneId)
    {
        Dictionary<string, object> values = new Dictionary<string, object>() {
            {"saveId", saveId},
            {"sceneId", sceneId},
            {"characterId", characterId},
            {"saveData", characterSaveData}
        };

        this.Insert(values, false);
    }
}
