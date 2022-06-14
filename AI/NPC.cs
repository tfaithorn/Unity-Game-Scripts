using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private Animator animator;
    private Schedule schedule;
    private EnvironmentTime environmentTime;

    private Schedule.ScheduleItem currentScheduleItem;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = this.GetComponent<Animator>();

        //get environment references
        GameObject environmentObj = GameObject.Find("Environment");
        if (environmentObj)
        {
            environmentTime = environmentObj.GetComponent<EnvironmentTime>();
        }

        //get current schedule item
        if (GetComponent<Schedule>())
        {
            schedule = GetComponent<Schedule>();
            environmentTime.hourIncreaseEvent.AddListener(CheckSchedule);
            CheckSchedule();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    void WalkToPoint(Vector3 point)
    {
        animator.SetInteger("Action", 2);

        agent.destination = point;

    }

    IEnumerator PerformSchedule()
    {

        if (currentScheduleItem.PlacementTransform.position != this.transform.position)
        {
            WalkToPoint(currentScheduleItem.PlacementTransform.position);
        }

        //wait until the path has been calculated
        while (agent.pathPending)
        {
            yield return new WaitForFixedUpdate();
        }

        //wait until the agent has completed its destination
        while (agent.remainingDistance != agent.stoppingDistance)
        {
            yield return new WaitForFixedUpdate();
        }
        
    }

    void CheckSchedule()
    {
        currentScheduleItem = schedule.GetItem(environmentTime.GetHourMinutes());
        if (currentScheduleItem != null) {
            //Debug.Log("Time:" + currentScheduleItem.action + " Action:" + currentScheduleItem.action);
            StartCoroutine(PerformSchedule());
        }
        
    }

}

