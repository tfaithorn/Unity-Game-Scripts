using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class FormTextBox : VisualElement
{
    public Label label;
    public TextField textField;
    private List<string> undoStack = new List<string>();
    private List<string> redoStack = new List<string>();

    public FormTextBox(string name, string value, bool makeLarge = false)
    {
        VisualTreeAsset formTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Resources/FormComponents/TextBox/FormTextBox.uxml");
        formTree.CloneTree(this);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Resources/FormComponents/FormCommon.uss");

        label = this.Query<Label>(null, "form-label");

        textField = this.Query<TextField>(null, "form-text-field");
        if (makeLarge)
        {
            label.AddToClassList("form-label-textbox-large");
            textField.AddToClassList("form-textbox-large");
            textField.multiline = true;
            EditorStyles.textField.wordWrap = true;
        }

        //Unity doesn't let you use keyboard shortcuts in textfields and this is the workaround
        textField.RegisterCallback<KeyDownEvent>(x => {
            
            if (x.keyCode == KeyCode.Space)
            {
                AddToUndoStack();
            }

            if (x.ctrlKey && x.keyCode == KeyCode.Z && x.shiftKey)
            {
                if (redoStack.Count > 0)
                {
                    textField.SetValueWithoutNotify(redoStack[redoStack.Count - 1]);
                    RemoveFromRedoStack();
                }
            } 
            else if (x.ctrlKey && x.keyCode == KeyCode.Z)
            {
                if (undoStack.Count >= 2)
                {
                    AddToRedoStack();
                    textField.SetValueWithoutNotify(undoStack[undoStack.Count - 2]);
                    RemoveFromUndoStack();
                }
            }
        });

        textField.name = name;
        textField.value = value;
        AddToUndoStack();
        this.styleSheets.Add(styleSheet);
    }

    private void RemoveFromUndoStack()
    {
        undoStack.RemoveAt(undoStack.Count-1);
    }
    
    private void AddToUndoStack()
    {
        undoStack.Add(this.textField.text);
        redoStack.Clear();
    }

    private void AddToRedoStack()
    {
        redoStack.Add(this.textField.text);
    }

    private void RemoveFromRedoStack()
    {
        redoStack.RemoveAt(redoStack.Count - 1);
    }

}
