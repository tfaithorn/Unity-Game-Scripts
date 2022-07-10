using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneSaveDataRepository : DbRepository, IRepository<SaveSceneData>
{

    const string tableName = "saveSceneData";
    const string fields = @"*";

    const string tableJoins = @"JOIN save ON save.id = sceneSaveData.saveId
                                JOIN playerCharacter ON playerCharacter.characterId = save.playerId";
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

    public List<SaveSceneData> GetByCriteria(List<SqlClient.Expr> criteria = null)
    {
        var result = GetResult(criteria);
        var sceneSaveDatas = new List<SaveSceneData>();

        foreach (var row in result)
        {
            sceneSaveDatas.Add(new SaveSceneData((long)row["id"], (string)row["data"], (long)row["sceneId"]));
        }

        return sceneSaveDatas;
    }

}
