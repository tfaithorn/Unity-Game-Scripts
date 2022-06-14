using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EnvironmentTime : MonoBehaviour
{
    [SerializeField] private float currentMinutes;
    [SerializeField] private int currentHour;

    [SerializeField] private float hourInterval = 10;
    [SerializeField] private float dayInterval = 25;
    [SerializeField] private float speedMultiplier = 1;

    public UnityEvent hourIncreaseEvent;

    // Start is called before the first frame update
    void Start()
    {
        if (hourIncreaseEvent == null)
            hourIncreaseEvent = new UnityEvent();
    }

    void FixedUpdate()
    {
        currentMinutes += Time.deltaTime * speedMultiplier;
        
        if (currentMinutes > hourInterval)
        {
            currentHour++;
            currentMinutes = 0;
            hourIncreaseEvent.Invoke();
        }

        if (currentHour == 24)
        {
            currentHour = 0;
        }
    }


    public int GetHour()
    {
        return currentHour;
    }

    public float GetHourMinutes()
    {
        return (float)currentHour + (currentMinutes / hourInterval);
    }


}
