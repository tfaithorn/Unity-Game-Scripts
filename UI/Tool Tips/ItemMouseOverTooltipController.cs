using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemMouseOverTooltipController : MonoBehaviour
{
    public RectTransform panel;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    public void SetItem(string name, string description)
    {
        itemName.text = name;
        itemDescription.text = description;
    }

    public void Activate()
    {
        panel.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        panel.gameObject.SetActive(false);
    }
}
