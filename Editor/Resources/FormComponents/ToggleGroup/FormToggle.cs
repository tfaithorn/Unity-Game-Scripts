using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
//using System.Linq;

public class FormToggle : Toggle
{
    public string dataValue { get; set; }
    public new class UxmlFactory : UxmlFactory<FormToggle, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription dataValue = new UxmlStringAttributeDescription { name = "string-attr", defaultValue = "default_value" };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var ate = ve as FormToggle;

            ate.Clear();

            ate.dataValue = dataValue.GetValueFromBag(bag, cc);
            ate.Add(new TextField("String") { value = ate.dataValue });

        }
    }
}
