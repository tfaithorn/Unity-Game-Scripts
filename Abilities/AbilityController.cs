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
    private AbilityInstance currentAbilityInstance;
    private CharacterMB characterMB;

    public readonly Dictionary<Ability, AbilityInstance> abilities = new Dictionary<Ability, AbilityInstance>();
    public List<AbilityInstance> abilitiesList = new List<AbilityInstance>();

    public bool performingAbility = false;
    public bool allowAbilities = true;

    public delegate void AbilityStarted(Ability ability);
    public event AbilityStarted abilityStartedEvent;

    public delegate void AbilityFinished(Ability ability);
    public event AbilityFinished abilityFinishedEvent;

    private void Awake()
    {
        characterMB = GetComponent<CharacterMB>();
    }

    public void AddAbility(Ability ability)
    {
        if (!abilities.ContainsKey(ability))
        {
            var abilityInstance = new AbilityInstance(ability);
            abilities.Add(ability, abilityInstance);
            abilitiesList.Add(abilityInstance);
        }
        
        return;
    }

    public void LoadAbility(Ability ability)
    {
        //if ability script has not already been added
        if (!gameObject.GetComponent(Type.GetType(ability.abilityScriptName)))
        {
            var abilityInstance = abilities[ability];
            abilityInstance.abilityScript = (AbilityScript)gameObject.AddComponent(Type.GetType(ability.abilityScriptName));
            abilityInstance.abilityScript.LoadAbility(ability);

            abilityInstance.cooldown.endTime = abilityInstance.abilityScript.GetCooldown(characterMB);
            abilityInstance.duration.endTime = abilityInstance.abilityScript.GetDuration(characterMB);
        }

        abilities[ability].isLoaded = true;
        
        return;
    }

    public bool AbilityAllowsMovement()
    {
        if (currentAbilityInstance != null && currentAbilityInstance.abilityScript.allowMovement)
        {
            return true;
        }
        else if (currentAbilityInstance == null) {
            return true;
        }
        return false;
    }

    public void ResetAbility()
    {
        //not tested
        currentAbilityInstance.duration.durationPassed = 0f;
        currentAbilityInstance.abilityScript.ResetAbility();
    }

    public void InterruptAbility()
    {
        currentAbilityInstance.interruptAbility = true;
        performingAbility = false;
        currentAbilityInstance.duration.durationPassed = 0f;
        currentAbilityInstance.abilityScript.InterruptAbility();
        currentAbilityInstance = null;
    }

    public void ReleaseAbility()
    {
        if (currentAbilityInstance != null)
        {
            currentAbilityInstance.abilityScript.ReleaseAbility();
        }
    }

    public void FinishAbility()
    {
        performingAbility = false;
        currentAbilityInstance.abilityScript.FinishAbility();
        currentAbilityInstance = null;
    }

    public bool StartAbility(Ability ability)
    {
        //if you are are casting an interruptable ability finish it
        if (performingAbility)
        {
            if (currentAbilityInstance.abilityScript.isInterruptable)
            {
                InterruptAbility();
            }
            else
            {
                return false;
            }
        }

        var abilityInstance = abilities[ability];
        currentAbilityInstance = abilities[ability];

        //add cooldown & duration timers
        StartCoroutine(StartCooldownTimer(abilityInstance));
        StartCoroutine(StartDurationTimer(abilityInstance));

        abilities[ability].abilityScript.StartAbility();

        performingAbility = true;

        return true;
    }

    IEnumerator StartDurationTimer(AbilityInstance abilityInstance)
    {

        while (abilityInstance.duration.durationPassed < abilityInstance.duration.endTime)
        {
            //if ability is interrupted return without calling FinishAbility
            if (abilityInstance.interruptAbility)
            {
                yield break;
            }

            abilityInstance.duration.durationPassed += Time.deltaTime;
            yield return null;
        }
        FinishAbility();
        abilityInstance.duration.durationPassed = 0;
    }

    IEnumerator StartCooldownTimer(AbilityInstance abilityInstance)
    {
        while (abilityInstance.cooldown.durationPassed < abilityInstance.cooldown.endTime)
        {
            abilityInstance.cooldown.durationPassed += Time.deltaTime;
            yield return null;
        }

        abilityInstance.cooldown.durationPassed = 0;
    }
}
