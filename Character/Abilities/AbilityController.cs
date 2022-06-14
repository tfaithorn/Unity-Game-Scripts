using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Used by the player character & NPCs
/// To use an ability you must first:
/// 1. Add an ability with AddAbility
/// 2. Load the ability with LoadAbility
/// </summary>

public class AbilityController : MonoBehaviour
{
    private Ability currentAbility;
    private Character character;
    private TimerManager timerManager;

    public readonly List<Ability> abilitiesAvailable = new List<Ability>();
    public readonly List<Ability> abilitiesLoaded = new List<Ability>(); 

    public bool performingAbility = false;
    public bool allowAbilities = true;

    public delegate void AbilityStarted(Ability ability);
    public event AbilityStarted abilityStartedEvent;

    public delegate void AbilityFinished(Ability ability);
    public event AbilityFinished abilityFinishedEvent;

    private void Awake()
    {
        character = GetComponent<Character>();
        timerManager = gameObject.GetComponent<TimerManager>();
    }

    private void Update()
    {
        if (performingAbility) 
        {
            if (currentAbility != null)
            {
                currentAbility.abilityScript.PerformAbility();

                if ((!currentAbility.hasDuration || currentAbility.duration.finished)
                && !currentAbility.abilityScript.isInfinite)
                {
                    FinishAbility();
                }
            }
        }
    }

    public void AddAbility(Ability ability)
    {
        
        if (!abilitiesAvailable.Exists(x => x.id == ability.id))
        {
            abilitiesAvailable.Add(ability);
        }

        ability.abilityScript.AddAbility(ability);
        return;
    }

    public void LoadAbility(Ability ability)
    {
        //if ability script has not already been added
        if (!gameObject.GetComponent(Type.GetType(ability.className)))
        {
            gameObject.AddComponent(Type.GetType(ability.className));
            ability.abilityScript = (AbilityScript)gameObject.GetComponent(Type.GetType(ability.className));
            ability.abilityScript.LoadAbility(ability);
        }

        if (!abilitiesLoaded.Exists(x => x.id == ability.id))
        {
            abilitiesLoaded.Add(ability);
        }
        
        return;
    }

    public bool AbilityAllowsMovement()
    {
        if (currentAbility != null && currentAbility.abilityScript.allowMovement)
        {
            return true;
        }
        else if (currentAbility == null) {
            return true;
        }
        return false;
    }

    public void ResetAbility()
    {
        //not tested
        currentAbility.duration.durationPassed = 0f;
        currentAbility.abilityScript.ResetAbility();
    }

    public void InterruptAbility()
    {
        //not tested
        performingAbility = false;
        currentAbility.duration.durationPassed = 0f;
        currentAbility.abilityScript.InterruptAbility();
        currentAbility = null;
    }

    public void ReleaseAbility()
    {
        if (currentAbility != null)
        {
            currentAbility.abilityScript.ReleaseAbility();
        }
    }

    public void FinishAbility()
    {
        if (currentAbility.hasDuration)
        {
            timerManager.RemoveTimer(currentAbility.duration);
        }
        
        performingAbility = false;
        currentAbility.abilityScript.FinishAbility();
        abilityFinishedEvent?.Invoke(currentAbility);
        currentAbility = null;
    }

    public bool StartAbility(Ability ability)
    {

        //if you are are casting an interruptable ability finish it
        if (currentAbility != null)
        {
            if (ability.abilityScript.isInterruptable)
            {
                InterruptAbility();
            }
            else
            {
                //otherwise prevent new ability from starting
                return false;
            }
        }

        //add cooldowns & duration to timer list
        if (ability.hasCooldown)
        {

            timerManager.ResetTimer(ability.cooldown);
            timerManager.AddTimer(ability.cooldown);
        }

        if (ability.hasDuration)
        {
            timerManager.ResetTimer(ability.duration);
            timerManager.AddTimer(ability.duration);

        }
        ability.abilityScript.StartAbility();
        performingAbility = true;
        currentAbility = ability;
        //abilityStartedEvent?.Invoke(ability);

        return true;
    }
}
