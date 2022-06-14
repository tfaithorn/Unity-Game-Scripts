using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schedule : MonoBehaviour
{
    [Serializable]
    public class ScheduleItem
    {
        public Transform PlacementTransform;
        public float environmentTime;
        public int action;
    }

    [SerializeField] public List<ScheduleItem> scheduleList = new List<ScheduleItem>();

    public ScheduleItem GetItem(float curEnvironmentTime)
    {
        ScheduleItem currentItem = null;

        foreach(ScheduleItem scheduleItem in scheduleList)
        {
            if (scheduleItem.environmentTime <= curEnvironmentTime)
            {
                if (currentItem == null)
                {
                    currentItem = scheduleItem;
                }
                else if (scheduleItem.environmentTime > currentItem.environmentTime)
                {
                    currentItem = scheduleItem;
                }
            }
        }

        return currentItem;
    }
}
