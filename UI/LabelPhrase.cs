using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif



public class LabelPhrase : MonoBehaviour
{
    [HideInInspector]
    public List<string> list1;

    #if UNITY_EDITOR
    [HideInInspector]
    public LanguageCategory languageCategory;
    [SerializeField, HideInInspector]
    public long categoryId = 0;
    [SerializeField, HideInInspector]
    public long subcategoryId = 0;
#endif
    [SerializeField]
    public string identifierName;

    private void Start()
    {
        TextMeshProUGUI label = GetComponent<TextMeshProUGUI>();

        if (identifierName != null)
        {
            string phrase = LanguageController.GetPhrase(identifierName);
            label.text = phrase;
        } else 
        {
            label.text = "<Identifier Missing>";
        }
    }

}
