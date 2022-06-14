using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserQuests
{
    int userId;
    int questId;
    Status status;

    public enum Status { 
        STARTED = 1,
        INPROGRESS = 2,
        COMPLETE = 3,
        FAILED = 4
    }

    public UserQuests(int userId, int questId, Status status)
    {
        this.userId = userId;
        this.questId = questId;
        this.status = status;
    }

    public List<UserQuests> GetByCriteria(List<SqlClient.Expr> criteria)
    {
        string sql =
            @"SELECT * 
            FROM userQuests
            {criteria}";

        var paramGroup = new SqlClient.ParamGroup();
        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);
        sql = SqlClient.ReplaceToken(sql,"criteria", preparedWhere);

        List<Dictionary<string, object>> result = SqlClient.Execute(sql, paramGroup);

        List<UserQuests> userQuests = new List<UserQuests>();

        foreach (Dictionary<string, object> row in result)
        {
            userQuests.Add(new UserQuests(Convert.ToInt32((Int64)row["userId"]), Convert.ToInt32((Int64)row["questId"]), (Status)Convert.ToInt32((Int64)row["id"])));
        }

        return userQuests;
    }

}
