using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcCharacterMB : CharacterMB
{
    private Schedule schedule;
    private EnvironmentTime environmentTime;

    private Schedule.ScheduleItem currentScheduleItem;
    private NavMeshAgent agent;
    private PersistentScripts persistentScripts;

    protected override void Awake()
    {
        base.Awake();
        this.inventory = GetComponent<Inventory>();

        var gb = GameObject.FindGameObjectWithTag(Constants.persistentScriptsTagName);

        if (gb)
        {
            persistentScripts = gb.GetComponent<PersistentScripts>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();

        //Set up environment references
        if (persistentScripts)
        {
            environmentTime = persistentScripts.GetComponent<EnvironmentTime>();

            if (environmentTime)
            {
                schedule = GetComponent<Schedule>();
                if (schedule != null)
                {
                    environmentTime.hourIncreaseEvent.AddListener(CheckSchedule);
                    CheckSchedule();
                }
            }
        }
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
        if (currentScheduleItem != null)
        {
            StartCoroutine(PerformSchedule());
        }
    }

    private bool CheckIfInScene()
    {
        /*
        //1.check "saveCharacter"  to see if the player is loaded for them. If there is data recorded they have been loaded before
        //2.if there is data, check whether enough time has passed for the npc to reset
        //3.if enough time hasn't passed, do not load them, let the save controller take care of it

        SaveCharacterRepository saveCharacterRepository = new SaveCharacterRepository();

        //todo: update with recursive query for save
        var criteria = new List<SqlClient.Expr>() {
            new SqlClient.Cond("characterId", this.id, SqlClient.OP_EQUAL)
        };

        var saveCharacters = saveCharacterRepository.GetByCriteria(criteria);
        var sceneController = SceneController.FindSceneController();

        //check whether the character has been loaded before
        if (saveCharacters.Count != 0) {
            //character has been loaded for the first time


        } 
        else if (saveCharacters.Count != 0 && saveCharacters[0].sceneId == sceneController.currentSceneZone.id)
        { 
            
        }
        */
        return true;
    }

    private bool CheckIfScheduleChanged()
    {
        return true;
    }
}
