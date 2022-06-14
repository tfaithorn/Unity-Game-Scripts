using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentLighting : MonoBehaviour
{
    EnvironmentTime environmentTime;
    public Light globalLightObj;

    void Start()
    {
        //get reference to time component
        environmentTime = this.GetComponent<EnvironmentTime>();

        //add event listener to update lighting
        environmentTime.hourIncreaseEvent.AddListener(UpdateSceneLighting);
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
