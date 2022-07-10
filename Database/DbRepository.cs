using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DbRepository
{

    public abstract string GetFields();
    public abstract string GetTableJoins();
    public abstract string GetTableName();
    public abstract string GetOrderBy();

    //public abstract List<T> GetByCriteria<T>(List<SqlClient.Expr> criteria = null);
    protected List<Dictionary<string, object>> GetResult(List<SqlClient.Expr> criteria = null)
    {
        string sql = "SELECT {fields} FROM {tableName} {dbJoins} {preparedWhere} ORDER BY {orderBy}";
        var paramGroup = new SqlClient.ParamGroup();
        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);

        sql = SqlClient.ReplaceTokens(sql, new Dictionary<string, object>() {
            {"fields", GetFields()},
            {"dbJoins", GetTableJoins()},
            {"tableName", GetTableName()},
            {"preparedWhere", preparedWhere},
            {"orderBy", GetOrderBy()}
        });

        return SqlClient.Execute(sql, paramGroup);
    }

    protected long Insert(Dictionary<string, object> values, bool returnId = false)
    {
        string sql = @"INSERT INTO {tableName} {values}";

        var paramGroup = new SqlClient.ParamGroup();
        string preparedValues = SqlClient.PrepareInsert(values, paramGroup, returnId);

        sql = SqlClient.ReplaceTokens(sql, new Dictionary<string, object>() {
            {"tableName", GetTableName()},
            {"values", preparedValues}
        });

        var result = SqlClient.Execute(sql, paramGroup);

        if (result != null)
        {
            return (long)result[0]["lastId"];
        }
        else
        {
            return 0;
        }
    }

    protected void Update(Dictionary<string, object> values, List<SqlClient.Expr> criteria)
    {
        string sql = @"
                UPDATE 
                {tableName} 
                {update}
                {criteria}";

        var paramGroup = new SqlClient.ParamGroup();

        string preparedUpdate = SqlClient.PrepareUpdate(values, paramGroup);
        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);

        sql = SqlClient.ReplaceTokens(sql, new Dictionary<string, object>() {
            {"tableName", GetTableName()},
            {"update", preparedUpdate},
            {"criteria", preparedWhere}
        });

        SqlClient.Execute(sql, paramGroup);
    }
}
