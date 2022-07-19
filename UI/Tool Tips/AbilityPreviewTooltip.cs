using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityPreviewTooltip : MonoBehaviour
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;

    public void SetValues(string name, string description)
    {
        this.name.text = name;
        this.description.text = description;
    }    
}
