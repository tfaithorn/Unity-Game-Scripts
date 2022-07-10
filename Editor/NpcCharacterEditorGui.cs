using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

[CustomEditor(typeof(NpcCharacterMB))]
public class NpcCharacterEditorGui : Editor
{
    public override void OnInspectorGUI()
    {
        NpcCharacterMB myScript = (NpcCharacterMB)target;

        //prevent prefab from being edited when it has been added to the scene unless it has a guid
        if(PrefabStageUtility.GetCurrentPrefabStage() == null 
            && (myScript.guid == null || myScript.guid == "")
            && myScript.gameObject.activeInHierarchy)
        //if (myScript.gameObject.activeInHierarchy && (myScript.guid == null || myScript.guid == ""))
        {
            if (GUILayout.Button("Generate Guid"))
            {
                myScript.guid = Guid.NewGuid().ToString();
            }
        }
        else
        {
            EditorGUILayout.LabelField("Guid: " + myScript.guid);
            DrawDefaultInspector();
        }
    }
}
