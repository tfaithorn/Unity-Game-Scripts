using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public readonly long id;
    public string name;

    List<QuestionObjective> questionObjectives;
    List<QuestStage> questionStages;

    public Quest(long id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public static List<Quest> GetByCriteria(List<SqlClient.Expr> criteria)
    {
        string sql = @"SELECT * FROM quest {criteria}";

        SqlClient.ParamGroup paramGroup = new SqlClient.ParamGroup();

        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);
        sql = SqlClient.ReplaceToken(sql, "criteria", preparedWhere);

        var result = SqlClient.Execute(sql, paramGroup);

        List <Quest> quests = new List<Quest>();

        foreach (var row in result)
        {
            quests.Add(new Quest((long)row["id"], (string)row["name"]));
        }

        return quests;

    }
}
