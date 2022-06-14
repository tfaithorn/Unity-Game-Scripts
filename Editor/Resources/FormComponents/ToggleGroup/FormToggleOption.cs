using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormToggleOption
{
    public string name;
    public string value;
    public bool ticked;

    public FormToggleOption(string name, string value, bool ticked)
    {
        this.name = name;
        this.value = value;
        this.ticked = ticked;
    }
}
