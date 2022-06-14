using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LanguageIdentifier
{

    public long id;
    public LanguageCategory category;
    public LanguageSubCategory subcategory;
    public string name;
    public string description;

    private const string errorIdentifierExists = "That identifier is already being used.";

    public LanguageIdentifier(long id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public static void Update(Dictionary<string, object> values, List<SqlClient.Expr> criteria)
    {
        string sql = @"
                UPDATE languageIdentifier 
                {update}
                {criteria}";

        var paramGroup = new SqlClient.ParamGroup();

        string preparedUpdate = SqlClient.PrepareUpdate(values, paramGroup);
        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);

        sql = SqlClient.ReplaceToken(sql, "update", preparedUpdate);
        sql = SqlClient.ReplaceToken(sql, "criteria", preparedWhere);

        SqlClient.Execute(sql, paramGroup);
    }

    public static List<LanguageIdentifier> GetByCriteria(List<SqlClient.Expr> criteria = null)
    {
        string sql = @"SELECT
                            li.id AS id,
                            li.name AS name,
                            li.description AS description,
                            li.categoryId AS categoryId,
                            lc.name AS categoryName,
                            lc.baseIdentifier AS baseIdentifier,
                            CASE 
                                WHEN li.categoryId = 1 THEN qc.id
                                WHEN li.categoryId = 2 THEN ac.id
                                WHEN li.categoryId = 3 THEN sc.id
                                WHEN li.categoryId = 4 THEN syc.id
                            END AS subcategoryId,
                            sc.id AS statsCategoryId,
                            sc.name AS statsCategoryName,
                            sc.baseIdentifier AS statsBaseIdentifier,
                            syc.id AS systemCategoryId,
                            syc.name AS systemCategoryName,
                            syc.baseIdentifier AS systemBaseIdentifier
                       FROM languageIdentifier li
                       LEFT JOIN languageCategory lc ON lc.id = li.categoryId
                       LEFT JOIN languageIdentifierQuest liq ON liq.languageIdentifierId = li.id
                       LEFT JOIN questCategory qc ON qc.id = liq.questCategoryId
                       LEFT JOIN languageIdentifierAbility lia ON lia.languageIdentifierId = li.id
                       LEFT JOIN abilityCategory ac ON ac.id = lia.abilityCategoryId
                       LEFT JOIN languageIdentifierStats lis ON lis.languageIdentifierId = li.id
                       LEFT JOIN statsCategory sc ON sc.id = sc.id = lis.statsCategoryId
                       LEFT JOIN languageIdentifierSystem lisy ON lisy.languageIdentifierId = li.id
                       LEFT JOIN systemCategory syc ON syc.id = lisy.systemCategoryId
                       {criteria}";

        var paramGroup = new SqlClient.ParamGroup();

        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);
        sql = SqlClient.ReplaceToken(sql,"criteria", preparedWhere);
        List<Dictionary<string,object>> result = SqlClient.Execute(sql, paramGroup);

        List<LanguageIdentifier> languageIdentifiers = new List<LanguageIdentifier>();

        foreach (Dictionary<string,object> row in result)
        {
            

            LanguageCategory.Type categoryType = LanguageCategory.GetTypeById((long)row["categoryId"]);
            LanguageSubCategory subcategory = LanguageSubCategory.BuildByType(categoryType,row);

            LanguageCategory category = null;

            if (row["categoryId"] != null)
            {
                category = new LanguageCategory((long)row["categoryId"], (string)row["categoryName"], categoryType, (string)row["baseIdentifier"]);
            }
            
            LanguageIdentifier languageIdentifier = new LanguageIdentifier((long)row["id"], (string)row["name"]);
            languageIdentifier.category = category;
            languageIdentifier.subcategory = subcategory;
            languageIdentifier.description = (string)row["description"];

            languageIdentifiers.Add(languageIdentifier);
        }

        return languageIdentifiers;
    }

    public static long Insert(Dictionary<string,object> values, bool returnId)
    {
        var paramGroup = new SqlClient.ParamGroup();
        string sql = @"INSERT INTO languageIdentifier {insert};";
        string preparedInsert = SqlClient.PrepareInsert(values, paramGroup, returnId);

        sql = SqlClient.ReplaceToken(sql, "insert", preparedInsert);
        var result = SqlClient.Execute(sql, paramGroup);

        return (long)result[0]["lastId"];
    }

    public static void Delete(List<SqlClient.Expr> criteria = null)
    {
        string sql = "DELETE FROM languageIdentifier {criteria}";

        SqlClient.ParamGroup paramGroup = new SqlClient.ParamGroup();
        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);
        sql = SqlClient.ReplaceToken(sql, "criteria", preparedWhere);

        SqlClient.Execute(sql,paramGroup);
    }
}
