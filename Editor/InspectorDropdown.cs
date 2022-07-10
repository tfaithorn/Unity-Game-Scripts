using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(LabelPhrase))]
public class InspectorDropdown : Editor
{
    /*
    List<LanguageCategory> languageCategories;
    List<string> languageCategoriesString;
    List<LanguageSubCategory> languageSubCategories;
    List<string> languageSubcategoriesString;
    List<LanguageIdentifier> languageIdentifiers;
    List<string> languageIdentifierString;

    int categoryIndex = 0;
    int subcategoryIndex = 0;
    int identifierIndex = 0;

    LabelPhrase script;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        script = (LabelPhrase)target;

        languageCategories = LanguageCategory.GetByCriteria();
        languageCategoriesString = languageCategories.Select(x => x.name).ToList();

        GUIContent listLabel = new GUIContent("Select a Category");

        if (script.categoryId != 0)
        {
            categoryIndex = languageCategories.FindIndex(x => x.id == script.categoryId);
        }

        EditorGUI.BeginChangeCheck();
        int newCategoryIndex = EditorGUILayout.Popup(listLabel, categoryIndex, languageCategoriesString.ToArray());

        if (EditorGUI.EndChangeCheck())
        {
            script.categoryId = languageCategories[newCategoryIndex].id;
            PopulateSubcategoryList();
        }

        PopulateSubcategoryList();
        CreateSubCategoryDropdown();

        PopulateIdentifierList();
        CreateIdentifierDropdown();

    }

    public void CreateSubCategoryDropdown()
    {
        GUIContent subcategoryLabel = new GUIContent("Select a Subcategory");
        subcategoryLabel = new GUIContent("Select a Subcategory");
        EditorGUI.BeginChangeCheck();
        subcategoryIndex = EditorGUILayout.Popup(subcategoryLabel, subcategoryIndex, languageSubcategoriesString.ToArray());

        if (EditorGUI.EndChangeCheck())
        {
            script.subcategoryId = languageSubCategories[subcategoryIndex].id;
            PopulateIdentifierList();
        }
    }

    public void PopulateSubcategoryList()
    {
        List<SqlClient.Expr> criteria = new List<SqlClient.Expr>()
            {
                {new SqlClient.Cond("categoryId",script.categoryId,SqlClient.OP_EQUAL)}
            };

        languageSubCategories = LanguageSubCategory.GetByCriteria(criteria);
        languageSubcategoriesString = languageSubCategories.Select(x => x.name).ToList();
    }

    public void PopulateIdentifierList()
    {
        List<SqlClient.Expr> criteria = new List<SqlClient.Expr>()
            {
                {new SqlClient.Cond("categoryId", script.categoryId, SqlClient.OP_EQUAL)},
                {new SqlClient.Cond("subcategoryId", script.subcategoryId, SqlClient.OP_EQUAL)}
            };

        languageIdentifiers = LanguageIdentifier.GetByCriteria(criteria);
        languageIdentifierString = languageIdentifiers.Select(x => x.name).ToList();
    }

    public void CreateIdentifierDropdown()
    {
        GUIContent identifierLabel = new GUIContent("Select an identifier");

        identifierIndex = languageIdentifiers.FindIndex(x => x.name == script.name);

        EditorGUI.BeginChangeCheck();
        identifierIndex = EditorGUILayout.Popup(identifierLabel, identifierIndex, languageIdentifierString.ToArray());
        if (EditorGUI.EndChangeCheck())
        {
            Debug.Log("id set?");
            script.identifierName = languageIdentifiers[identifierIndex].name;
            PopulateIdentifierList();
        }

    }
    */

}

