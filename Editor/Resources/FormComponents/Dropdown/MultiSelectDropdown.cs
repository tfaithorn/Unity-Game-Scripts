using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class MultiSelectDropdown : VisualElement
{
    public List<MultiSelectionOption> multiSelectionOptions;
    MultiOptionsContainer optionsContainer;
    private bool isMultiSelect;
    public MultiSelectDropdown(string name, bool isMultiSelect, List<FormValue> formValues)
    {
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Resources/FormComponents/FormCommon.uss");
        this.AddToClassList("multi-select-toggle");
        this.styleSheets.Add(styleSheet);

        optionsContainer = new MultiOptionsContainer();
        optionsContainer.AddToClassList("multi-select-options-container");
        optionsContainer.AddToClassList("hide");
        optionsContainer.styleSheets.Add(styleSheet);

        foreach (FormValue formValue in formValues)
        {
            MultiSelectionOption newOption = new MultiSelectionOption();

            newOption.Add(CreateTickContainer());

            Label dropdownLabel = new Label();
            dropdownLabel.text = formValue.name;
            newOption.Add(dropdownLabel);

            newOption.value = formValue.value;
            newOption.AddToClassList("multi-select-option");

            newOption.RegisterCallback<MouseUpEvent>(x => {
                MultiSelectionOption target = (MultiSelectionOption)x.currentTarget;

                if (!isMultiSelect)
                {
                    //untick all toggled options
                    optionsContainer.Query<MultiSelectionOption>(className: "multi-select-option").ForEach(e =>
                    {
                        e.ticked = false;
                        Image image = e.Query<Image>();
                        image.AddToClassList("hide");
                    });
                }
                Image image = target.Query<Image>();

                image.ToggleInClassList("hide");
                
                target.ticked = !target.ticked;
            });

            optionsContainer.Add(newOption);
        }

        this.RegisterCallback<MouseUpEvent>( (x) =>{
            if (!optionsContainer.hasBeenInit)
            {
                this.parent.Add(optionsContainer);
                optionsContainer.Init(this);
            }

            optionsContainer.style.top = (this.layout.y + this.layout.height);
            optionsContainer.style.left = (this.layout.x);

            optionsContainer.ToggleInClassList("hide");
            optionsContainer.ToggleDropdown();
        });
    }

    List<string> GetSelectedValues()
    {
        List<string> values = new List<string>();

        this.parent.Query<MultiSelectionOption>(className: "multi-select-option").ForEach((x) => {

            if (x.ticked)
            {
                values.Add(x.value);   
            }
        });

        return values;
    }

    public class MultiSelectionOption : VisualElement
    {
        public MultiSelectionOption()
        {
            this.AddToClassList("multi-select-option");
        }

        public bool ticked { get; set; }
        public string value { get; set; }
        public Label label { get; set; }
        public new class UxmlFactory : UxmlFactory<MultiSelectionOption, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription value = new UxmlStringAttributeDescription { name = "string-attr", defaultValue = "default_value" };
            UxmlBoolAttributeDescription ticked = new UxmlBoolAttributeDescription { name = "bool-attr", defaultValue = false };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as MultiSelectionOption;

                ate.Clear();

                ate.ticked = ticked.GetValueFromBag(bag, cc);
                ate.Add(new Toggle("Toggle") { value = ate.ticked });

                ate.value = value.GetValueFromBag(bag, cc);
                ate.Add(new TextField("String") { value = ate.value });
            }
        }
    }

    private VisualElement CreateTickImage()
    {
        Image tickImage = new Image();
        tickImage.image = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Scripts/Editor/Resources/Images/editor-tick.png");

        tickImage.AddToClassList("multi-select-tick-image");
        tickImage.AddToClassList("hide");

        tickImage.StretchToParentSize();
        return tickImage;
    }

    private VisualElement CreateTickContainer()
    {
        VisualElement tickContainer = new VisualElement();
        tickContainer.name = "tick-column";
        tickContainer.AddToClassList("multi-select-option-tick-container");
        tickContainer.Add(CreateTickImage());

        return tickContainer;
    }

    public void ToggleOption()
    { 
        
    }
}
