using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameClock : MonoBehaviour
{
    public Text clockTextBox;
    private EnvironmentTime envTime;
    void Start()
    {
        GameObject environmentObj = GameObject.Find("Environment");
        envTime = environmentObj.GetComponent<EnvironmentTime>();
        UpdateClock();
        envTime.hourIncreaseEvent.AddListener(UpdateClock);
    }

    private void UpdateClock()
    {
        clockTextBox.text = envTime.GetHour().ToString();
    }
}
