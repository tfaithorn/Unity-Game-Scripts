using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveCharacterRepository : DbRepository, IRepository<SaveCharacter>
{
    const string tableName = "saveCharacter";
    const string fields = @"player.id AS playerId,
                            player.name AS playerName,
                            player.lastPlayed AS lastPlayed,
                            save.id AS saveId,
                            save.name AS saveName,
                            save.createdAt AS createdAt,
                            character.id AS characterId,
                            character.name AS characterName,
                            character.prefabPath AS prefabPath,
                            saveCharacter.sceneId AS sceneId,
                            saveCharacter.saveData AS saveData";
    const string tableJoins = @"JOIN save ON save.id = saveCharacter.saveId
                                JOIN player ON player.id = save.playerId
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

    public List<SaveCharacter> GetNpcsForScene(long sceneId)
    {
        //TBC, recurisvely obtain data for scene from save or parent saves
        var sql = @"WITH RECURSIVE
                    ancestor(saveId, characterId, sceneId, createdAt) AS (
                     SELECT
 	                    save.id AS saveId,
 	                    saveCharacter.characterId AS characterId,
 	                    saveCharacter.sceneId AS sceneId,
 	                    ""
                     FROM saveCharacter
                     JOIN save ON save.id = saveCharacter.saveId
                     WHERE 
 	                    saveCharacter.sceneId = 2
                     UNION ALL
                     SELECT
 	                    s.id AS saveId,
 	                    sc.characterId AS characterId,
 	                    sc.sceneId AS sceneId,
 	                    s.createdAt
                     FROM saveCharacter sc
                     JOIN save s ON s.id = sc.saveId
                     JOIN ancestor a ON a.saveId = s.parentId
                    )
                    SELECT * FROM ancestor;";

        var criteria = new List<SqlClient.Expr>()
        {
            new SqlClient.Cond("saveCharacter.sceneId",sceneId, SqlClient.OP_EQUAL)
        };

        return GetByCriteria(criteria);
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
