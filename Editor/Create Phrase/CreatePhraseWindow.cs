using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Linq;
using System;

public class CreatePhraseWindow : EditorWindow
{
    private EditorWindow callerEditorWindow;

    public delegate void UpdateSucessful();
    public event UpdateSucessful UpdateSucessfulEvent;

    LanguagePhrase languagePhrase;
    Language language;
    List<Language> languages;

    LanguageCategory selectedCategory;
    List<LanguageCategory> categories;
    List<Quest> quests;

    FormDropdownGroup categoryDropdown;
    FormDropdownGroup subCategoryDropdown;
    FormDropdownGroup languageDropdown;
    FormDropdownGroup questDropdown;
    FormDropdownGroup abilitiesDropdown;

    VisualElement root;

    public static void ShowExample()
    {
        CreatePhraseWindow wnd;
        wnd = GetWindow<CreatePhraseWindow>();
        wnd.minSize = new Vector2(550f, 500f);
        wnd.titleContent = new GUIContent("Create Phrase");
    }

    public void Init(LanguagePhrase languagePhrase, Language language, EditorWindow callerEditorWindow)
    {
        this.language = language;
        this.callerEditorWindow = callerEditorWindow;
        this.languagePhrase = languagePhrase;
        this.selectedCategory = languagePhrase.identifier != null ? languagePhrase.identifier.category : null;
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Create Phrase/CreatePhrase.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Resources/EditorStyles.uss");
        root.styleSheets.Add(styleSheet);

        VisualElement formContainer = root.Query<VisualElement>("form-container");

        languages = Language.GetByCriteria();
        categories = new List<LanguageCategory>() {};
        categories.AddRange(LanguageCategory.GetByCriteria());

        UpdateHeader();

        if (language == null)
        {
            formContainer.Add(CreateLanguageDropdown());
        }
        else {
            formContainer.Add(CreateLanguageLabel());
        }

        formContainer.Add(CreateIdentifierBox());
        formContainer.Add(CreateCategoryDropdown());
        formContainer.Add(CreateSubCategoryDropdown());

        VisualElement tempQuestDropdown = CreateQuestDropdown();
        tempQuestDropdown.AddToClassList("hide");
        formContainer.Add(tempQuestDropdown);

        formContainer.Add(CreatePhraseTextBox());
        formContainer.Add(CreateDescriptionTextBox());

        if (languagePhrase == null)
        {
            formContainer.Add(CreateInsertButton());
        }
        else {
            if (languagePhrase.phrase == null)
            {
                formContainer.Add(CreateInsertButton());
            }
            else
            {
                formContainer.Add(CreateUpdateButton());
            }
        }
    }

    void UpdateHeader()
    {
        if (languagePhrase!= null && languagePhrase.phrase != null)
        {
            Label headerLabel = root.Query<Label>("header");
            headerLabel.text = "Update Phrase";
        }
    }

    VisualElement CreateLanguageDropdown()
    {
        List<FormValue> languageDropdownOptions = new List<FormValue>();

        foreach (Language language in languages) {
            languageDropdownOptions.Add(new FormValue(language.name, language.id.ToString()));
        }

        languageDropdown = new FormDropdownGroup ("language-dropdown", languageDropdownOptions);
        languageDropdown.label.text = "Language";
        return languageDropdown;

    }

    VisualElement CreateLanguageLabel()
    {
        FormLabel languageLabel = new FormLabel("form-language-label", "Language:", language != null ? language.name : "");
        return languageLabel;
    }

    VisualElement CreatePhraseTextBox()
    {
        FormTextBox phraseTextBox = new FormTextBox("phrase-textbox", languagePhrase != null ? languagePhrase.phrase : "", true);
        phraseTextBox.label.text = "Phrase:";
        return phraseTextBox;
    }

    VisualElement CreateDescriptionTextBox()
    {
        FormTextBox descriptionTextBox = new FormTextBox("description-textbox", languagePhrase != null ? languagePhrase.identifier.description : "", true);
        descriptionTextBox.label.text = "Phrase \nDescription:";
        return descriptionTextBox;
    }

    VisualElement CreateIdentifierBox()
    {
        FormTextBox formIdentiferTextbox = new FormTextBox("form-identifier-box", languagePhrase != null ? languagePhrase.identifier.name : "");
        formIdentiferTextbox.label.text = "Identifier:";
        return formIdentiferTextbox;
    }

    void ShowSubMenus()
    {
        VisualElement formContainer = root.Query<VisualElement>("form-container");
        LanguageCategory.Type type = LanguageCategory.GetTypeById(Convert.ToInt64(categoryDropdown.GetSelectedValue()));

        formContainer.Query(className: "sub-subcategory-dropdown").ForEach(x =>
        {
            x.AddToClassList("hide");            
        });

        string subCategoryName = "";
        switch (selectedCategory.type) {
            case LanguageCategory.Type.QUEST:
                subCategoryName = "quest-dropdown";
                break;
        }

        VisualElement subsubdropdown = formContainer.Query(subCategoryName);
        subsubdropdown.RemoveFromClassList("hide");
    }

    VisualElement CreateCategoryDropdown()
    {
            List<FormValue> categoriesDropdownOptions = new List<FormValue>();

            foreach (LanguageCategory languageCategory in categories)
            {
                categoriesDropdownOptions.Add(new FormValue(languageCategory.name, languageCategory.id.ToString()));
            }

            categoryDropdown = new FormDropdownGroup("category-dropdown", categoriesDropdownOptions);

            if(selectedCategory != null)
            {
                categoryDropdown.SetSelected(selectedCategory.id.ToString());
            }

            categoryDropdown.label.text = "Category:";
            categoryDropdown.dropdown.RegisterValueChangedCallback(x =>
            {
                selectedCategory = categories.Find(x => x.id.ToString() == categoryDropdown.GetSelectedValue());

                List<FormValue> subcategoriesDropdownOptions = new List<FormValue>();

                subcategoriesDropdownOptions = GetSubCategoryOptions();
                subCategoryDropdown.ReplaceValues(subcategoriesDropdownOptions);

                ShowSubMenus();
            });

            return categoryDropdown;
    }


    private VisualElement CreateQuestDropdown()
    {
        questDropdown = new FormDropdownGroup("quest-dropdown", GetQuestDropdownValues());
        questDropdown.AddToClassList("sub-subcategory-dropdown");

        questDropdown.dropdown.RegisterValueChangedCallback(x =>
        {
            questDropdown.ReplaceValues(GetQuestDropdownValues());
        });

        questDropdown.label.text = "Quests";
        return questDropdown;
    }

    List<FormValue> GetQuestDropdownValues()
    {
        List<FormValue> questDropdownOptions = new List<FormValue>();
        var criteria = new List<SqlClient.Expr>();

        string subCategoryDropdownValue = subCategoryDropdown.GetSelectedValue();

        if (subCategoryDropdownValue != null)
        {
            criteria.Add(new SqlClient.Cond("categoryId", Convert.ToInt64(subCategoryDropdownValue), SqlClient.OP_EQUAL));
        }

        quests = Quest.GetByCriteria(criteria);

        foreach (Quest quest in quests)
        {
            questDropdownOptions.Add(new FormValue(quest.name, quest.id.ToString()));
        }

        return questDropdownOptions;
    }

    private VisualElement CreateUpdateButton()
    {
        Button button = new Button();
        button.text = "Update";
        button.AddToClassList("btn");

        button.AddToClassList("minor-break");
        button.RegisterCallback<MouseUpEvent>(x=> {
            UpdatePhrase();
        });

        return button;
    }

    private VisualElement CreateInsertButton()
    {
        Button button = new Button();
        button.text = "Create";
        button.AddToClassList("btn");
        button.AddToClassList("minor-break");
        button.RegisterCallback<MouseUpEvent>(x => {
            InsertPhrase();
        });

        return button;
    }

    private VisualElement CreateSubCategoryDropdown()
    {
        List<FormValue> subcategoriesDropdownOptions = new List<FormValue>();

        if(selectedCategory != null)
        {
            subcategoriesDropdownOptions = GetSubCategoryOptions();
        }

        subCategoryDropdown = new FormDropdownGroup("sub-category-dropdown", subcategoriesDropdownOptions);

        return subCategoryDropdown;
    }

    public List<FormValue> GetSubCategoryOptions()
    {
        List<LanguageSubCategory> subCategories = new List<LanguageSubCategory>();

        List<SqlClient.Expr> criteria = new List<SqlClient.Expr>();
        criteria.Add(new SqlClient.Cond("categoryId", selectedCategory.id, SqlClient.OP_EQUAL));

        subCategories = LanguageSubCategory.GetByCriteria(criteria);

        List<FormValue> subcategoriesDropdownOptions = new List<FormValue>();

        foreach (LanguageSubCategory subCategory in subCategories)
        {
            subcategoriesDropdownOptions.Add(new FormValue(subCategory.name, subCategory.id.ToString()));
        }

        return subcategoriesDropdownOptions;
    }

    void UpdatePhrase()
    {
        TextField phrase = root.Query<TextField>("phrase-textbox");
        TextField identifierNameTextBox = root.Query<TextField>("form-identifier-box");
        long categoryID = Convert.ToInt64(categoryDropdown.GetSelectedValue());
        TextField description = root.Query<TextField>("description-textbox");


        Dictionary<string, object> identifierValues = new Dictionary<string, object>();

        if (categoryDropdown != null)
        {
            categoryID = long.Parse(categoryDropdown.GetSelectedValue());

            identifierValues.Add("categoryID", categoryID);
        }

        if (languagePhrase.identifier != null)
        {
            var identifierCriteria = new List<SqlClient.Expr>()
            {
                {new SqlClient.Cond("id", languagePhrase.identifier.id, SqlClient.OP_EQUAL)}
            };

            identifierValues.Add("name", identifierNameTextBox.text);
            identifierValues.Add("description", description.text);

            LanguageIdentifier.Update(identifierValues, identifierCriteria);
        }


        Dictionary<string, object> phraseValues = new Dictionary<string, object>()
            {
                {"phrase", phrase.text}
            };

        var phraseCriteria = new List<SqlClient.Expr>() {
            { new SqlClient.Cond("identifierId", languagePhrase.identifier.id, SqlClient.OP_EQUAL)},
            { new SqlClient.Cond("languageId", languagePhrase.languageId, SqlClient.OP_EQUAL)},
        };

        LanguagePhrase.Update(phraseValues, phraseCriteria);

        UpdateSucessfulEvent?.Invoke();

        //close the window
        this.Close();
    }

    void InsertPhrase()
    {
        TextField phrase = root.Query<TextField>("phrase-textbox");
        TextField identifierTextBox = root.Query<TextField>("form-identifier-box");
        TextField description = root.Query<TextField>("description-textbox");

        string identifierValue = identifierTextBox.text;
        string descriptionValue = description.text;

        string subCategorySelectedValue = subCategoryDropdown.GetSelectedValue();

        if (subCategorySelectedValue != null)
        {
            long subCategoryID = long.Parse(subCategoryDropdown.GetSelectedValue());
        }

        long languageIdValue;

        if (languagePhrase != null)
        {
            languageIdValue = languagePhrase.languageId;
        }
        else
        {
            languageIdValue = long.Parse(languageDropdown.GetSelectedValue());
        }

        long categoryID = long.Parse(categoryDropdown.GetSelectedValue());

        //Check if identifier already exists
        List<SqlClient.Expr> criteria = new List<SqlClient.Expr>() {
            new SqlClient.Cond("li.name",identifierValue, SqlClient.OP_EQUAL),
        };

        var result = LanguageIdentifier.GetByCriteria(criteria);

        if (result.Count > 0)
        {
            return;
        }

        var values = new Dictionary<string, object>()
        {
            {"name", identifierValue},
            {"description", descriptionValue},
            {"categoryId", categoryID}
        };

        long identifierId = LanguageIdentifier.Insert(values, true);

        values = new Dictionary<string, object>()
        {
            {"phrase", phrase.text},
            {"identifierId", identifierId},
            {"languageId", languageIdValue}
        };

        LanguagePhrase.Insert(values, false);
        this.Close();
    }
}