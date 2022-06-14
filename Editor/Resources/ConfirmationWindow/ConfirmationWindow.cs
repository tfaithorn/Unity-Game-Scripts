using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class ConfirmationWindow : EditorWindow
{
    public Label label;
    public Button yesButton;
    public Button noButton;

    public static void ShowExample()
    {
        ConfirmationWindow wnd = GetWindow<ConfirmationWindow>();
        wnd.titleContent = new GUIContent("ConfirmationWindow");
    }

    private void OnEnable()
    {
        label = new Label();
        yesButton = new Button();
        noButton = new Button();
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Resources/ConfirmationWindow/ConfirmationWindow.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Resources/EditorStyles.uss");
        root.styleSheets.Add(styleSheet);

        VisualElement container = new VisualElement();

        label.AddToClassList("center");

        label.style.marginTop = 20;
        label.style.marginBottom = 20;
        label.style.fontSize = 25;
        root.Add(label);
        
        container.AddToClassList("table-row");
        container.AddToClassList("center");
        container.style.width = 200;

        yesButton.AddToClassList("btn");
        noButton.AddToClassList("btn");

        yesButton.AddToClassList("table-col-small");
        noButton.AddToClassList("table-col-small");

        yesButton.AddToClassList("cell-button");
        noButton.AddToClassList("cell-button");


        yesButton.text = "Yes";
        noButton.text = "No";

        container.Add(yesButton);
        container.Add(noButton);

        root.Add(container);

    }
}