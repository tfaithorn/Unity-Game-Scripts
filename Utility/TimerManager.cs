using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    List<Timer> timers = new List<Timer>();
    List<Timer> newTimers = new List<Timer>();
    bool isNewTimers = false;

    // Update is called once per frame
    void Update()
    {
        EvaluateTimers();
        EvaluateNewTimers();
    }

    public void RemoveTimer(Timer timer)
    {
        timer.remove = true;
    }

    public void AddTimer(Timer timer)
    {
        if (!newTimers.Any(x => x.guid == timer.guid))
        {
            isNewTimers = true;
            newTimers.Add(timer);
        }
    }

    public Timer RequestTimer(float endTime)
    {
        Timer newTimer = new Timer(endTime);
        newTimers.Add(newTimer);
        isNewTimers = true;
        return newTimer;
    }

    public void UpdateTimer(Timer timer, float durationPassed, float endTime)
    {
        timer.durationPassed = durationPassed;
        timer.endTime = endTime;
    }

    public void PauseTimer(Timer timer)
    {
        timer.pauseTimer = true;
    }

    public void UnPauseTimer(Timer timer)
    {
        timer.pauseTimer = false;
    }

    private void EvaluateTimers()
    {
        //increase each timer
        for (int i = timers.Count -1; i >= 0; i--)
        {
            if (timers[i].remove)
            {
                RemoveTimerFromList(i);
                continue;
            }

            if (!timers[i].finished && !timers[i].pauseTimer)
            {
                timers[i].durationPassed += Time.deltaTime;

                if (timers[i].durationPassed > timers[i].endTime)
                {
                    timers[i].finished = true;
                    timers[i].TimerFinished();
                }
            }
        }
    }

    public void ResetTimer(Timer timer)
    {
        timer.durationPassed = 0f;
        timer.finished = false;
        timer.remove = false;
    }

    private void EvaluateNewTimers()
    {
        //add new timers after current timers are evaluated
        if (isNewTimers)
        {
            foreach (Timer timer in newTimers)
            {
                timers.Add(timer);
            }

            newTimers.Clear();
            isNewTimers = false;
        }
    }

    private void RemoveTimerFromList(int i)
    {
        Timer timer = timers[i];
        timers.RemoveAt(i);
    }
}
