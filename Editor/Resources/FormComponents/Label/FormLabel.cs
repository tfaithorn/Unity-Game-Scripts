using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class FormLabel: VisualElement
{
    public Label labelLeft;
    public Label labelRight;

    public FormLabel(string name, string leftValue, string rightValue)
    {
        VisualTreeAsset formTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Resources/FormComponents/Label/FormLabel.uxml");
        formTree.CloneTree(this);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Resources/FormComponents/FormCommon.uss");
        this.styleSheets.Add(styleSheet);

        labelLeft = this.Query<Label>(null, "form-label");
        labelLeft.text = leftValue;

        labelRight = this.Query<Label>(null, "form-label-right");
        labelRight.text = rightValue;
    }
}