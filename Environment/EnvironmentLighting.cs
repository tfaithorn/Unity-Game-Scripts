using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script placed on a lighting gameobject to make it controlled by the environment's time.
/// Note: Requires the lighting gameobject have the 'Lighting Source' tag.
/// </summary>
public class EnvironmentLighting : MonoBehaviour
{
    private PersistentScripts persistentScripts;
    private EnvironmentTime environmentTime;
    private Light globalLightObj;

    private void Awake()
    {
        var gb = GameObject.FindGameObjectWithTag(Constants.persistentScriptsTagName);

        if (gb) {
            persistentScripts = gb.GetComponent<PersistentScripts>();
        }

        this.globalLightObj = GetComponent<Light>();
    }

    void Start()
    {
        //get reference to time component
        if (persistentScripts)
        {
            environmentTime = persistentScripts.GetComponent<EnvironmentTime>();

            if (environmentTime != null)
            {
                //add event listener to update lighting
                environmentTime.hourIncreaseEvent.AddListener(UpdateSceneLighting);
            }
        }
    }

    void UpdateSceneLighting()
    {
        float currentHour = environmentTime.GetHour();
        if (currentHour >= 4 && currentHour <= 6)
        {
            globalLightObj.intensity = 0.3f;
        }
        else if (currentHour >= 7 && currentHour <= 16)
        {
            globalLightObj.intensity = 0.5f;
        }
        else if (currentHour >= 17 && currentHour <= 20)
        {
            globalLightObj.intensity = 0.2f;
        }
        else if (currentHour >= 21 && currentHour <= 23)
        {
            globalLightObj.intensity = 0.1f;
        }
    }
}
