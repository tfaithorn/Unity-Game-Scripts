using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LanguagePhrase
{
    public string phrase;
    public long languageId;
    public LanguageIdentifier identifier;

    private const string errorInvalidLanguage = "Invalid Language.";
    private const string errorIdentifierExists = "That identifier is already being used.";
    private const string errorEmpty = "No values provided";

    public LanguagePhrase(string phrase, long languageId, LanguageIdentifier identifier)
    {
        this.phrase = phrase;
        this.languageId = languageId;
        this.identifier = identifier;
    }

    public static List<LanguagePhrase> GetByCriteria(List<SqlClient.Expr> criteria = null, Dictionary<string, object> addedParameters = null)
    {

        string sql =
            @"SELECT
                li.id AS identifierId,
                li.name AS identifierName,
                li.description AS description,
                lp.phrase AS phrase,
                lp.languageId AS languageId,
                lc.id AS categoryId,
                lc.name AS categoryName,
                lc.baseIdentifier AS categoryBaseIdentifier,
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
            FROM languageIdentifier li
            LEFT JOIN languagePhrase lp ON lp.identifierId = li.id {languageJoins}
            LEFT JOIN languageCategory lc ON lc.id = li.categoryId
            LEFT JOIN languageIdentifierStats lis ON lis.languageIdentifierId = li.id AND lc.id = 3
            LEFT JOIN statsCategory sc ON sc.id = lis.statsCategoryId
            LEFT JOIN languageIdentifierSystem lisy ON lisy.languageIdentifierId = li.id AND lc.id = 5
            LEFT JOIN systemCategory syc ON syc.id = lisy.systemCategoryId
            LEFT JOIN languageIdentifierQuest liq ON liq.languageIdentifierId = li.id AND lc.id = 1
            LEFT JOIN questCategory qc ON qc.id = liq.questCategoryId
            LEFT JOIN languageIdentifierItem lii ON lii.languageIdentifierId = li.id AND lc.id = 4
            LEFT JOIN itemCategory ic ON ic.id = lii.itemCategoryID
            LEFT JOIN languageIdentifierAbility lia ON lia.languageIdentifierId = li.id AND lc.id = 2
            LEFT JOIN abilityCategory ac ON ac.id = lia.abilityCategoryId
            {criteria}
            ORDER BY 
                lc.name ASC,
                li.name ASC;";

        SqlClient.ParamGroup paramGroup = new SqlClient.ParamGroup();

        if (addedParameters != null)
        {
            string languageToken = paramGroup.RequestParameter(addedParameters["languageId"]);
            string languageJoins = "AND languageId = "+languageToken;
            sql = SqlClient.ReplaceToken(sql, "languageJoins", languageJoins);
        }
        else
        {
            sql = SqlClient.ReplaceToken(sql, "languageJoins", "");
        }

        List<LanguagePhrase> phrases = new List<LanguagePhrase>();

        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);
        sql = SqlClient.ReplaceToken(sql, "criteria", preparedWhere);

        List<Dictionary<string, object>> result = SqlClient.Execute(sql, paramGroup);

        foreach (Dictionary<string, object> row in result)
        {
            LanguageCategory.Type categoryType = row["categoryId"] != null ? LanguageCategory.GetTypeById((long)row["categoryId"]) : LanguageCategory.Type.NONE;
            LanguageCategory category = categoryType != LanguageCategory.Type.NONE ? new LanguageCategory((long)row["categoryId"], (string)row["categoryName"], categoryType, (string)row["categoryBaseIdentifier"]) : null;
            LanguageSubCategory subcategory = categoryType != LanguageCategory.Type.NONE ? LanguageSubCategory.BuildByType(categoryType, row) : null ;
            LanguageIdentifier identifier = new LanguageIdentifier((long)row["identifierId"], (string)row["identifierName"]);
            identifier.category = category;
            identifier.subcategory = subcategory;
            identifier.description = (string)row["description"];

            phrases.Add(
                new LanguagePhrase(
                    (string)row["phrase"],
                    row["languageId"] != null ? (long)row["languageId"] : 0,
                    identifier));
        }

        return phrases;
    }

    public static ResponseMessage Update(Dictionary<string,object> values, List<SqlClient.Expr> criteria)
    {

        if (values.Count == 0)
        {
            return new ResponseMessage(false, errorEmpty);
        }


        bool containsKey = false;

        foreach (SqlClient.Cond cond in criteria)
        {
            if (cond.field == "languageId")
            {
                containsKey = true;
            }
        }

        if (!containsKey)
        {
            return new ResponseMessage(false, errorInvalidLanguage);
        }

        if (values.ContainsKey("identifier"))
        {
            if (IsIdentifierTaken((string)values["identifier"], (long)values["id"]))
            {
                return new ResponseMessage(false, errorIdentifierExists);
            }
         }

        string sql = @"
            UPDATE languagePhrase
            {update}
            {criteria}";

        SqlClient.ParamGroup paramGroup = new SqlClient.ParamGroup();

        string preparedWhere = SqlClient.PrepareWhere(criteria, paramGroup);
        string preparedUpdate = SqlClient.PrepareUpdate(values, paramGroup);

        sql = SqlClient.ReplaceToken(sql, "criteria", preparedWhere);
        sql = SqlClient.ReplaceToken(sql, "update", preparedUpdate);

        SqlClient.Execute(sql, paramGroup);

       return new ResponseMessage(true, "Updated Sucessfully");
    }

    public static bool IsIdentifierTaken(string identifier, long id)
    {
        //Check if identifier is used by another phrase
        List<SqlClient.Expr> identifierCriteria = new List<SqlClient.Expr>() {
                new SqlClient.Cond("name",identifier, SqlClient.OP_EQUAL),
                new SqlClient.Cond("id",id, SqlClient.OP_NOTEQUAL),
            };

        var result = LanguageIdentifier.GetByCriteria(identifierCriteria);

        if (result.Count > 0)
        {
            return true;
        }

        return false;
    }

    public static long Insert(Dictionary<string, object> values, bool returnId)
    {
        var paramGroup = new SqlClient.ParamGroup();

        string sql = "INSERT INTO languagePhrase {insert};";

        string preparedInsert = SqlClient.PrepareInsert(values, paramGroup, returnId);
        sql = SqlClient.ReplaceToken(sql, "insert",preparedInsert);
        var result = SqlClient.Execute(sql, paramGroup);

        if (returnId)
        {
            return (long)result[0]["lastId"];
        }
        
        return 0;
        
    }

    public static void DELETE(List<SqlClient.Expr> criteria)
    {
        string sql = "DELETE FROM languagePhrase WHERE {criteria}";
    }
}
