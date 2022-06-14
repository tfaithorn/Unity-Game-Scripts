using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class LanguageController
{
    public static Dictionary<string, string> phrases;
    private static long currentLanguageID = 1;

    static LanguageController() 
    {
        phrases = new Dictionary<string, string>();
        List<SqlClient.Expr> criteria = new List<SqlClient.Expr>() 
        {
            { new SqlClient.Cond("languageId", currentLanguageID, SqlClient.OP_EQUAL) } 
        };
        List<LanguagePhrase> result = LanguagePhrase.GetByCriteria(criteria);

        foreach (LanguagePhrase row in result)
        {
            phrases.Add(row.identifier.name,row.phrase);
        }
    }
    public static string GetPhrase(string name)
    {
        return phrases[name];
    }
}
