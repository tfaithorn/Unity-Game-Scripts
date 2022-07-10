using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class LanguageEditorWindow : EditorWindow
{
    /*
    VisualElement rootElement;
    VisualElement phrasesSection;
    VisualElement categorySection;
    VisualElement formContainer;

    List<Language> languages = new List<Language>();
    List<LanguageCategory> categories = new List<LanguageCategory>();

    Language selectedLanguage;
    LanguageCategory selectedCategory;

    [MenuItem("Window/Language")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LanguageEditorWindow));
    }

    private void OnEnable()
    {
        rootElement = rootVisualElement;
    }

    private void CreateGUI()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/EditorFormXML.uxml");
        visualTree.CloneTree(rootElement);

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Resources/EditorStyles.uss");
        rootElement.styleSheets.Add(styleSheet);

        Label header = rootElement.Query<Label>("header");
        header.text = "Language Settings";

        languages = Language.GetByCriteria();
        selectedLanguage = languages[0];

        LanguageCategory allOption = new LanguageCategory(0, "All", LanguageCategory.GetTypeById(0), "");
        selectedCategory = allOption;
        categories = new List<LanguageCategory>() { allOption };
        categories.AddRange(LanguageCategory.GetByCriteria());

        VisualElement languagesSection = rootElement.Query<VisualElement>("languages-section");
        VisualElement categorySection = rootElement.Query<VisualElement>("category-section");

        categorySection.Add(CreateCategoryDropdown());
        languagesSection.Add(CreateLanguagesDropdown());
        languagesSection.Add(CreateNewPhraseButton());

        CreateTableHeaders();
        PopulatePhrases();
    }

    VisualElement CreateLanguagesDropdown()
    {

        var languageOptions = GetLanguageOptions();

        List<FormValue> languageDropdownOptions = new List<FormValue>();

        foreach (Language language in languages)
        {
            languageDropdownOptions.Add(new FormValue(language.name, language.id.ToString()));
        }

        FormDropdownGroup languageDropdown = new FormDropdownGroup("language-dropdown", languageDropdownOptions);
        languageDropdown.label.text = "Languages";
        languageDropdown.dropdown.RegisterValueChangedCallback((x) => {
            selectedLanguage = languages.Find(y => y.name == x.newValue);
            PopulatePhrases();
        });

        return languageDropdown;
    }

    public void PopulatePhrases()
    {
        phrasesSection = rootVisualElement.Query<VisualElement>("phrases-table");
        if (phrasesSection == null)
        {
            return;
        }

        List<SqlClient.Expr> criteria = new List<SqlClient.Expr>();
        Dictionary<string, object> addedJoins = new Dictionary<string, object>();

        if (selectedLanguage != null)
        {
            addedJoins.Add("languageId", selectedLanguage.id);
        }

        if (selectedCategory != null)
        {
            if (selectedCategory.id != 0)
            {
                criteria.Add(new SqlClient.Cond("categoryId", selectedCategory.id, SqlClient.OP_EQUAL));
            }
        }

        List<LanguagePhrase> phrases = LanguagePhrase.GetByCriteria(criteria, addedJoins.Count > 0 ? addedJoins : null);

        phrasesSection.Clear();

        int i = 0;
        foreach (LanguagePhrase phrase in phrases)
        {
            VisualElement phrasesRow = new VisualElement();
            phrasesRow.AddToClassList("table-row");

            if (i % 2 == 0) {
                phrasesRow.AddToClassList("table-background-1");
            } else {
                phrasesRow.AddToClassList("table-background-2");
            }

            VisualElement categoryCol = new VisualElement();
            categoryCol.AddToClassList("table-column");;

            TextElement categoryTextElement = new TextElement();
            categoryTextElement.AddToClassList("table-cell");
            categoryTextElement.text = phrase.identifier.category != null ? phrase.identifier.category.name : "<Category Missing>";
            categoryCol.Add(categoryTextElement);

            TextElement identifierTextElement = new TextElement();
            identifierTextElement.text = phrase.identifier != null ? phrase.identifier.name : "";
            identifierTextElement.AddToClassList("table-cell");

            VisualElement identifierCol = new VisualElement();
            identifierCol.AddToClassList("table-column");

            identifierCol.Add(identifierTextElement);

            VisualElement phraseCol = new VisualElement();
            phraseCol.AddToClassList("table-column");

            TextElement phraseTextElement = new TextElement();
            phraseTextElement.text = phrase.phrase != null ? phrase.phrase : "";
            phraseTextElement.AddToClassList("table-cell");

            phraseCol.Add(phraseTextElement);

            VisualElement descriptionCol = new VisualElement();
            descriptionCol.AddToClassList("table-column");

            TextElement descriptionTextElement = new TextElement();
            descriptionTextElement.text = phrase != null ? phrase.identifier.description : "";
            descriptionTextElement.AddToClassList("table-cell");

            descriptionCol.Add(descriptionTextElement);


            VisualElement editbuttonCol = new VisualElement();
            editbuttonCol.AddToClassList("table-column");
            editbuttonCol.AddToClassList("table-col-small");

            Button editCreateButton = new Button();
            editCreateButton.AddToClassList("cell-button");
            editCreateButton.AddToClassList("btn");
            editCreateButton.AddToClassList("btn-primary");

            if (phrase.languageId == 0)
            {
                editCreateButton.text = "Create";
            }
            else {
                editCreateButton.text = "Edit";
            }

            editCreateButton.RegisterCallback<MouseUpEvent>(x =>
            {
                CreatePhraseWindow createPhraseWindow = ScriptableObject.CreateInstance<CreatePhraseWindow>();
                createPhraseWindow.Init(phrase, selectedLanguage, this);

                createPhraseWindow.UpdateSucessfulEvent += UpdateSuccessfulEventCallback;

                createPhraseWindow.Show();
            });

            editbuttonCol.Add(editCreateButton);

            VisualElement deleteButtonCol = new VisualElement();
            deleteButtonCol.AddToClassList("table-column");
            deleteButtonCol.AddToClassList("table-col-small");

            if (phrase.identifier != null)
            {
                Button deleteButton = new Button();
                deleteButton.AddToClassList("btn");
                deleteButton.AddToClassList("cell-button");
                deleteButton.text = "Delete";

                deleteButton.RegisterCallback<MouseUpEvent>(x => {
                    ConfirmationWindow window = ScriptableObject.CreateInstance<ConfirmationWindow>();

                    window.label.text = "Are you sure?";

                    window.yesButton.RegisterCallback<MouseUpEvent>((x) => {
                        this.DeletePhrase(phrase.identifier);
                        window.Close();
                        PopulatePhrases();
                    });

                    window.noButton.RegisterCallback<MouseUpEvent>((x) => {
                        window.Close();
                    });

                    window.Show();
                });

                deleteButtonCol.Add(deleteButton);
            }

            phrasesRow.Add(categoryCol);
            phrasesRow.Add(identifierCol);
            phrasesRow.Add(phraseCol);
            phrasesRow.Add(descriptionCol);
            phrasesRow.Add(editbuttonCol);
            phrasesRow.Add(deleteButtonCol);
            phrasesSection.Add(phrasesRow);

            i++;
        }
    }

    private void UpdateSuccessfulEventCallback()
    {
        PopulatePhrases();
    }

    VisualElement CreateTableHeaders()
    {
        VisualElement tableHeadersSection = rootVisualElement.Query<VisualElement>("table-headers");
        tableHeadersSection.AddToClassList("table-header-row");

        //Add table headers
        VisualElement headerRow = new VisualElement();
        headerRow.AddToClassList("table-row");

        Label headerLabel1 = new Label();
        headerLabel1.text = "Category";
        headerLabel1.AddToClassList("table-column");
        headerLabel1.AddToClassList("table-cell");

        headerRow.Add(headerLabel1);

        Label headerLabel2 = new Label();
        headerLabel2.AddToClassList("table-column");
        headerLabel2.AddToClassList("table-cell");
        headerLabel2.text = "Identifier";

        headerRow.Add(headerLabel2);

        Label headerLabel3 = new Label();
        headerLabel3.AddToClassList("table-column");
        headerLabel3.AddToClassList("table-cell");
        headerLabel3.text = "Phrase";
        headerRow.Add(headerLabel3);

        Label headerLabel4 = new Label();
        headerLabel4.AddToClassList("table-column");
        headerLabel4.AddToClassList("table-cell");
        headerLabel4.text = "Description";
        headerRow.Add(headerLabel4);


        VisualElement header4 = new VisualElement();
        header4.AddToClassList("table-column");
        header4.AddToClassList("table-col-small");
        headerRow.Add(header4);

        VisualElement header5 = new VisualElement();
        header5.AddToClassList("table-column");
        header5.AddToClassList("table-col-small");
        headerRow.Add(header5);


        tableHeadersSection.style.height = 50;
        tableHeadersSection.Add(headerRow);

        return tableHeadersSection;
    }
    VisualElement CreateNewPhraseButton()
    {
        Button button = new Button();
        button.AddToClassList("btn");
        button.AddToClassList("btn-small");
        button.AddToClassList("right");
        button.AddToClassList("new-phrase-button");
        button.AddToClassList("btn-long");
        button.AddToClassList("btn-purple");
        button.text = "Add New Phrase";
        button.RegisterCallback<MouseUpEvent>(x =>
        {
            CreatePhraseWindow window = ScriptableObject.CreateInstance<CreatePhraseWindow>();
            window.Show();
        });

        return button;
    }

    VisualElement CreateCategoryDropdown()
    {

        List<FormValue> languageCategoriesDropdownOptions = new List<FormValue>();

        foreach (LanguageCategory languageCategory in categories)
        {
            languageCategoriesDropdownOptions.Add(new FormValue(languageCategory.name, languageCategory.id.ToString()));
        }

        FormDropdownGroup categoriesDropdownField = new FormDropdownGroup("categories-dropdown", languageCategoriesDropdownOptions);
        categoriesDropdownField.label.text = "Catergories";
        categoriesDropdownField.dropdown.RegisterValueChangedCallback((x) => {
            selectedCategory = categories.Find(x => x.id.ToString() == categoriesDropdownField.GetSelectedValue());
            PopulatePhrases();
        });

        return categoriesDropdownField;
    }

    private void OnFocus()
    {
        PopulatePhrases();
    }

    List<string> GetLanguageOptions()
    {
        List<string> languageOptions = new List<string>();

        foreach (Language language in languages)
        {
            languageOptions.Add(language.name);
        }

        return languageOptions;
    }

    private void DeletePhrase(LanguageIdentifier languageIdentifier)
    {
        var criteria = new List<SqlClient.Expr>()
        {
            new SqlClient.Cond("id",languageIdentifier.id, SqlClient.OP_EQUAL)
        };

        LanguageIdentifier.Delete(criteria);
    }
    */
}