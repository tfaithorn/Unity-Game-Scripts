using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Language
{
    public long id;
    public string name;

    public Language(long id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public static List<Language> GetByCriteria(List<SqlClient.Expr> criteria = null)
    {
        string sql = @"SELECT * FROM language {criteria} ORDER BY id ASC;";

        var paramGroup = new SqlClient.ParamGroup();

        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);
        sql = SqlClient.ReplaceToken(sql, "criteria", preparedWhere);

        //Dictionary<string, object> preparedParameters = SqlClient.PrepareParameters(criteria);
        List<Dictionary<string, object>> result = SqlClient.Execute(sql, paramGroup);

        List<Language> languages = new List<Language>();
        foreach (Dictionary<string, object> row in result)
        {
            languages.Add(new Language((long)row["id"], (string)row["name"]));
        }

        return languages;
    }
}
