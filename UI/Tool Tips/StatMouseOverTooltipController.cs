using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatMouseOverTooltipController : MonoBehaviour
{
    public RectTransform panel;
    public TextMeshProUGUI statDescription;

    public void SetTooltip(string description)
    {
        statDescription.text = description;
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
