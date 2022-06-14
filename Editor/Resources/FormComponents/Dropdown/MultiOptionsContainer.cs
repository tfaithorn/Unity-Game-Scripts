using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class MultiOptionsContainer : VisualElement
{
    public bool expanded = false;
    public bool hasBeenInit = false;
    public VisualElement dropdownLabel;

    public void Init(VisualElement dropdownLabel)
    {
        VisualElement root = GetRoot();
        this.dropdownLabel = dropdownLabel;

        root.RegisterCallback<MouseDownEvent>( x => {
            if (FindInHierarchy((VisualElement)x.target, dropdownLabel) == null && FindInHierarchy((VisualElement)x.target, this) == null)
            {
                this.AddToClassList("hide");
                expanded = false;
            }
        });

        hasBeenInit = true;
    }

    public VisualElement GetRoot()
    {
        bool foundEnd = false;
        VisualElement ele = this.parent;
        while (!foundEnd)
        {
            if (ele.parent != null)
            {
                ele = ele.parent;
            }
            else
            {
                foundEnd = true;
            }
        }
        
        return ele;
    }

    public VisualElement FindInHierarchy(VisualElement clickedElement, VisualElement target)
    {
        if (clickedElement == target)
        {
            return this;
        }

        bool foundTarget = false;
        VisualElement ele = clickedElement;

        while (!foundTarget)
        {
            if (ele == null)
            {
                return null;
            } else if (ele.parent == target)
            {
                return target;
            } else {
                ele = ele.parent;
            }
        }

        return null;
    }

    public void ToggleDropdown()
    {
        expanded = !expanded;
    }

}
