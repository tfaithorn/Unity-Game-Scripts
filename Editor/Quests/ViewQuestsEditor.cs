using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class ViewQuestsEditor : EditorWindow
{
    [MenuItem("Window/UI Toolkit/ViewQuestsEditor")]
    public static void ShowExample()
    {
        ViewQuestsEditor wnd = GetWindow<ViewQuestsEditor>();
        wnd.titleContent = new GUIContent("ViewQuestsEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Quests/ViewQuestsEditor.uxml");
        VisualElement mainXml = visualTree.Instantiate();
        root.Add(mainXml);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Resources/EditorStyles.uss");
        root.styleSheets.Add(styleSheet);

        root.Add(CreateQuestCategoryDropdown());

        FormToggle formToggle = new FormToggle();
        formToggle.name = "test-toggle";
        formToggle.dataValue = "Test Retrieve Value";
        root.Add(formToggle);
    }

    private VisualElement CreateQuestCategoryDropdown()
    {
        
   
        
        MultiSelectDropdown multiSelectDropdown = new MultiSelectDropdown("name", false, new List<FormValue>() {new FormValue("Label 1","Value 1"), new FormValue("Label 2", "Value 2") });
        


        return multiSelectDropdown;
    }

}