using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LanguageCategory
{
    public enum Type
    {
        NONE = 0,
        QUEST = 1,
        ABILITIES = 2,
        STATS = 3,
        ITEMS = 4,
        SYSTEM = 5
    }

    public long id;
    public string name;
    public Type type;
    public string baseIdentifier;

    public LanguageCategory(long id, string name, Type type, string baseIdentifier)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.baseIdentifier = baseIdentifier;
    }

    public static List<LanguageCategory> GetByCriteria(List<SqlClient.Expr> criteria = null)
    {
        string sql = @"SELECT * FROM languageCategory {criteria}";

        var paramGroup = new SqlClient.ParamGroup();
        string preparedCriteria = SqlClient.PrepareWhere(criteria, paramGroup);

        sql = SqlClient.ReplaceToken(sql, "criteria", preparedCriteria);

        List<Dictionary<string, object>> result = SqlClient.Execute(sql, paramGroup);

        List<LanguageCategory> categories = new List<LanguageCategory>();

        foreach (Dictionary<string, object> row in result)
        {
            LanguageCategory languageCategory = new LanguageCategory((long)row["id"], (string)row["name"], (Type)Convert.ToInt32((long)row["id"]), (string)row["baseIdentifier"]);
            categories.Add(languageCategory);
        }

        return categories;
    }

    public static Type GetTypeById(long id)
    {
        switch (Convert.ToInt32(id)) {
            case 0:
                return Type.NONE;
            case 1:
                return Type.QUEST;
            case 2:
                return Type.ABILITIES;
            case 3:
                return Type.STATS;
            case 4:
                return Type.ITEMS;
            default:
                return Type.SYSTEM;
        }
    }
}
