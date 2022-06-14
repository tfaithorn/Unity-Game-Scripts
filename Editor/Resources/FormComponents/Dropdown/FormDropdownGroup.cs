using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;

public class FormDropdownGroup :VisualElement
{
    public DropdownField dropdown;
    public Label label;
    public List<FormValue> formDropdownOptions;

    UxmlStringAttributeDescription dataValue;
    public FormDropdownGroup(string name, List<FormValue> choices, int defaultChoice = 0)
    {
        this.name = name;

        VisualTreeAsset formTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Resources/FormComponents/Dropdown/FormDropdown.uxml");
        formTree.CloneTree(this);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Resources/FormComponents/FormCommon.uss");
  
        ReplaceValues(choices, defaultChoice);

        label = this.Query<Label>(null, "form-label");
        this.styleSheets.Add(styleSheet);
    }

    public string GetSelectedValue()
    {
        if(dropdown.index == -1)
        {
            return null;
        }

        return formDropdownOptions[dropdown.index].value;
    }

    public void ReplaceValues(List<FormValue> choices, int defaultChoice = 0)
    {
        formDropdownOptions = choices;

        dropdown = this.Query<DropdownField>(null, "form-dropdown");
        dropdown.name = name;
        dropdown.choices = formDropdownOptions.Select(x => x.name).ToList();

        if (choices.Count > 0)
        {
            dropdown.value = choices[defaultChoice].name;
        }
        else
        {
            dropdown.SetValueWithoutNotify("");
        }
    }

    public void SetSelected(string value)
    {
        this.dropdown.index = formDropdownOptions.FindIndex(x => x.value == value);
    }
}
