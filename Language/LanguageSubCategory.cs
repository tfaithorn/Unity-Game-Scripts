using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSubCategory
{
    public long id;
    public string name;
    public string baseIdentifier;

    public LanguageSubCategory(long id, string name, string baseIdentifier)
    {
        this.id = id;
        this.name = name;
        this.baseIdentifier = baseIdentifier;
    }


    public static LanguageSubCategory BuildByType(LanguageCategory.Type categoryType, Dictionary<string, object> row)
    {
        switch (categoryType)
        {
            case LanguageCategory.Type.QUEST:
                if (row.ContainsKey("questCategoryId"))
                {
                    return row["questCategoryId"] != null ? new LanguageSubCategory((long)row["questCategoryId"], (string)row["questCategoryName"], (string)row["questBaseIdentifier"]) : null;
                }
                return null;
            case LanguageCategory.Type.ITEMS:
                if (row.ContainsKey("itemCategoryId"))
                {
                    return row["itemCategoryId"] != null ? new LanguageSubCategory((long)row["itemCategoryId"], (string)row["itemCategoryName"], (string)row["itemBaseIdentifier"]) : null;
                }
                return null;
            case LanguageCategory.Type.STATS:
                if (row.ContainsKey("statsCategoryId"))
                {
                    return row["statsCategoryId"] != null ? new LanguageSubCategory((long)row["statsCategoryId"], (string)row["statsCategoryName"], (string)row["statsBaseIdentifier"]) : null;
                }
                return null;
            case LanguageCategory.Type.SYSTEM:
                if (row.ContainsKey("systemCategoryId"))
                {
                    return row["systemCategoryId"] != null ? new LanguageSubCategory((long)row["systemCategoryId"], (string)row["systemCategoryName"], (string)row["systemBaseIdentifier"]) : null;
                }
                return null;
            default:
                return null;
        }
    }

    public static List<LanguageSubCategory> GetByCriteria(List<SqlClient.Expr> criteria = null)
    {
        string sql = @"SELECT
                            lc.id AS categoryId,
                            lc.name AS categoryName,
                            lc.baseIdentifier AS baseIdentifier,
                            qc.id AS questCategoryId,
                            qc.name AS questCategoryName,
                            qc.baseIdentifier AS questBaseIdentifier,
                            ac.id AS abilityCategoryId,
                            ac.name AS abilityCategoryName,
                            ac.baseIdentifier AS abilityBaseIdentifier,
                            sc.id AS statsCategoryId,
                            sc.name AS statsCategoryName,
                            sc.baseIdentifier AS statsBaseIdentifier,
                            ic.id AS itemCategoryId,
                            ic.name AS itemCategoryName,
                            ic.baseIdentifier AS itemBaseIdentifier,
                            syc.id AS systemCategoryId,
                            syc.name AS systemCategoryName,
                            syc.baseIdentifier AS systemBaseIdentifier
                       FROM languageCategory lc 
                       LEFT JOIN questCategory qc ON lc.id = 1 
                       LEFT JOIN abilityCategory ac ON lc.id = 2
                       LEFT JOIN statsCategory sc ON lc.id = 3 
                       LEFT JOIN itemCategory ic ON lc.id = 4
                       LEFT JOIN systemCategory syc ON lc.id = 5
                       {criteria}";

        var paramGroup = new SqlClient.ParamGroup();
        var preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);
        sql = SqlClient.ReplaceToken(sql, "criteria", preparedWhere);

        var result = SqlClient.Execute(sql, paramGroup);

        var subCategories = new List<LanguageSubCategory>();
        foreach (var row in result)
        {
            LanguageCategory.Type categoryType = LanguageCategory.GetTypeById((long)row["categoryId"]);
            LanguageSubCategory subCategory = BuildByType(categoryType, row);

            if (subCategory != null)
            {
                subCategories.Add(subCategory);
            }
        }

        return subCategories;
    }
}
