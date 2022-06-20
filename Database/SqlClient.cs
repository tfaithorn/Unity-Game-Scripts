using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.Linq;

/// <summary>
/// Used for all interactions with the database
/// </summary>
public static class SqlClient
{
    private const string dbName = "URI=file:database.db";

    public const string OP_EXPR = "expr";
    public const string OP_EQUAL = "=";
    public const string OP_NOTEQUAL = "!=";
    public const string OP_AND = "AND";
    public const string OP_OR = "OR";
    public const string OP_NOT = "NOT";
    public const string OP_IN = "IN";
    public const string OP_NOTIN = "NOT IN";
    public const string OP_IS = "IS";
    public const string OP_ISNOT = "IS NOT";
    public const string OP_LT = "<";
    public const string OP_GT = ">";
    public const string OP_LTE = "<=";
    public const string OP_GTE = ">=";
    public const string OP_BETWEEN = "BETWEEN";
    public const string OP_LIKE = "LIKE";
    public const string OP_NOTLIKE = "NOT LIKE";

    public enum Function { 
        CURRENT_TIMESTAMP
    }

    private const string parameterSymbol = "@";

    /// <summary>
    /// Base class for database conditions
    /// </summary>
    public class Expr
    {

    }

    /// <summary>
    /// Single database condition
    /// </summary>
    public class Cond : Expr
    {
        public string field;
        public object value;
        public string operation;

        public Cond(string field, object value, string operation)
        {
            this.field = field;
            this.value = value;
            this.operation = operation;
        }
    }

    /// <summary>
    /// Group of database conditions
    /// </summary>
    public class CondGroup : Expr
    {
        public string operation;
        public List<Cond> conditions;

        public CondGroup(string operation, List<Cond> conditions)
        {
            this.operation = operation;
            this.conditions = conditions;
        }
    }

    public class ParamGroup
    {
        public int counter;
        public Dictionary<string, object> parameters;

        public ParamGroup()
        {
            parameters = new Dictionary<string, object>();
            counter = 0;
        }

        public string RequestParameter(object value)
        {
            counter++;

            string param = "@param" + counter;
            parameters.Add(param, value);
            return param;
        }
    }

    public static List<Dictionary<string, object>> Execute(string sql, ParamGroup parameters = null)
    {
        List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;

                Debug.Log(sql);

                //Prepare optional parameters
                if(parameters != null)
                {
                    foreach (var item in parameters.parameters)
                    {
                        command.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                using IDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(
                            reader.GetName(i),
                            reader.GetValue(i) == System.DBNull.Value ? null : reader.GetValue(i)
                        );
                    }

                    result.Add(row);
                }

                reader.Close();
            }

            connection.Close();
        }
        return result;
    }

    public static string ReplaceToken(string sql, string token, object value)
    {
        return ReplaceTokens(sql, new Dictionary<string, object>() { { token, value } });
    }
    public static string ReplaceTokens(string sql, Dictionary<string, object> tokens)
    {
        foreach (var item in tokens)
        {
            sql = sql.Replace("{" + item.Key + "}", item.Value.ToString());
        }
        return sql;
    }

    public static string PrepareWhere(List<Expr> conditions, ParamGroup paramGroup)
    {

        if (conditions == null || conditions.Count == 0) {
            return "";
        }

        string sql = " WHERE ";

        bool firstRow = true;

        foreach (Expr expr in conditions)
        {
            if (!firstRow)
            {
                sql += " AND";
            }

            if (expr is Cond)
            {
                Cond condition = (Cond)expr;

                string value = "";

                //if value is a function bypass paramgroup
                if (condition.value is Function)
                {
                    value = GetFunctionText((Function)condition.value);
                }
                if (condition.operation == OP_IN)
                {
                    value = CreateInCondition(condition, paramGroup);
                }
                else
                {
                    value = paramGroup.RequestParameter(condition.value);
                }
                
                sql += " " + condition.field + " " + condition.operation + " " + value;
            }

            //Note: Has not yet been tested
            if (expr is CondGroup)
            {
                sql += " (";
                CondGroup condGroup = (CondGroup)expr;
                bool condGroupCondFirstRow = true;
                foreach (Cond condition in condGroup.conditions)
                {
                    if (!condGroupCondFirstRow)
                    {
                        sql += " AND";
                    }

                    sql += " " + condition.field + " " + condition.operation + " " + paramGroup.RequestParameter(condition.value);
                    condGroupCondFirstRow = false;
                }

                sql += " )";
            }
            firstRow = false;
        }
        return sql;
    }

    public static string PrepareInsert(Dictionary<string, object> values, ParamGroup paramGroup, bool returnId = false)
    {
        if (values.Count == 0)
        {
            return "";
        }

        string fields = "";
        string sqlValues = "";

        int i = 0;

        foreach (var item in values)
        {
            fields += item.Key;
            sqlValues += paramGroup.RequestParameter(item.Value);

            if (i != values.Count- 1)
            {
                fields += ", ";
                sqlValues += ", ";
            }

            i++;
        }

        string sql = "(" + fields + " ) VALUES (" + sqlValues + ");";

        if (returnId)
        {
            sql += "SELECT Last_Insert_Rowid() AS lastId";
        }

        return sql;
    }

    public static string PrepareUpdate(Dictionary<string, object> values, ParamGroup paramGroup)
    {
        if (values.Count == 0)
        {
            return "";
        }

        string sql = " SET";

        int itemCount = values.Count;
        int i = 0;

        foreach (var item in values)
        {
            sql += " " + item.Key + " = ";

            //if it is a function bypass paramgroup
            if (item.Value is Function)
            {
                sql += GetFunctionText((Function)item.Value);
            }
            else
            {
                sql += paramGroup.RequestParameter(item.Value);
            }

            if (i < itemCount - 1)
            {
                sql += ",";
            }

            i++;
        }

        return sql;
    }

    public static Dictionary<string, object> CombineParameters(Dictionary<string,object> dict1, Dictionary<string, object> dict2)
    {
        Dictionary<string,object> newDict = new Dictionary<string, object>();

        foreach (var item in dict1)
        {
            newDict.Add(item.Key, item.Value);
        }

        foreach (var item in dict2)
        {
            newDict.Add(item.Key, item.Value);
        }

        return newDict;
    }

    private static string CreateInCondition(Cond condition, ParamGroup paramGroup)
    {
        string value = "(";
        int inIndex = 1;
        var objArray = condition.value as Array;

        foreach (var val in objArray)
        {
            value += paramGroup.RequestParameter(val);

            if (inIndex != objArray.Length)
            {
                value += ",";
            }

            inIndex++;
        }

        value += ")";

        return value;
    }

    private static string GetFunctionText(Function funct)
    {
        switch (funct) {
            case Function.CURRENT_TIMESTAMP:
                return "CURRENT_TIMESTAMP";
            default:
                return "";
        }
    }
}
