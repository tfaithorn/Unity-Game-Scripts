using System.Collections;
using System.Collections.Generic;
using System;

public class Timer
{
    public Guid guid;
    public float durationPassed;
    public float endTime;
    public bool finished;
    public bool remove;
    public bool pauseTimer;

    public delegate void OnTimerFinished();
    public event OnTimerFinished finishedEvent;
    public Timer(float endTime)
    {
        this.guid = Guid.NewGuid();
        this.durationPassed = 0;
        this.finished = false;
        this.remove = false;
        this.endTime = endTime;
        this.pauseTimer = false;
    }

    public void TimerFinished()
    {
        finishedEvent?.Invoke();
    }
}
