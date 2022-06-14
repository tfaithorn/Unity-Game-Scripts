using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Quest Stages should not be visible through the UI.
/// They are used to track whether a quest has advanced, separate from the quest objectives.
/// </summary>
public class QuestStage
{
    public readonly int id;
    public string name;
    public int position;

    public QuestStage(int id, string name, int position)
    {
        this.id = id;
        this.name = name;
        this.position = position;
    }

    /// <summary>
    /// Note: Not tested
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static List<QuestStage> GetByCriteria(List<SqlClient.Expr> criteria)
    {
        string sql = "SELECT * FROM questStage {criteria}";

        var paramGroup = new SqlClient.ParamGroup();
        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);
        sql = SqlClient.ReplaceToken(sql, "criteria", preparedWhere);
        List<Dictionary<string,object>> result = SqlClient.Execute(sql, paramGroup);

        List<QuestStage> questStages = new List<QuestStage>();

        foreach (Dictionary<string, object> row in result)
        {
            questStages.Add(new QuestStage(Convert.ToInt32((Int64)row["id"]), (string)row["name"], (int)row["position"]));
        }

        return questStages;
    }
}
