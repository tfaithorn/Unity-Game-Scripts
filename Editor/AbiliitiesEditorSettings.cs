using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class AbiliitiesEditorSettings : EditorWindow
{
    class Ability { 
        private int id;
        private string name;
        private string className;

        public Ability(int id, string name, string className)
        {
            this.id = id;
            this.name = name;
            this.className = className;
        }

    }

    List<Ability> abilities = new List<Ability>();


    [MenuItem("Window/Ability Settings")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AbiliitiesEditorSettings));
    }

    private void CreateGUI()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/EditorFormXML.uxml");
        visualTree.CloneTree(rootVisualElement);

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Resources/EditorStyles.uss");
        rootVisualElement.styleSheets.Add(styleSheet);

        Label header = rootVisualElement.Query<Label>("header");
        header.text = "Ability Settings";

        var dropdownField = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Resources/FormDropdown.uxml");

        

    }
}