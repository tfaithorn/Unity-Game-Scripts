using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
public class FormToggleGroup : VisualElement
{
    public Label label;
    public List<Toggle> Toggles;

    public FormToggleGroup(string name, List<FormToggleOption> formToggleOptions)
    {
        label = this.Query<Label>(null, "form-label");

    }
}
