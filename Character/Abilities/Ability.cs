using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Ability
{
    public long id;
    public string name;
    public string icon;

    public Timer duration;
    public bool hasDuration;
    public bool durationOnCooldown;

    public Timer cooldown;
    public bool hasCooldown;
    public bool onCooldown;

    public string className;
    public AbilityScript abilityScript;

    public List<AbilityTags> tags;
    public bool isAllowed;

    public List<ItemWeapon.WeaponType> requiredWeaponTypes = new List<ItemWeapon.WeaponType>();
    public List<ItemWeapon.WeaponClass> requiredWeaponClasses = new List<ItemWeapon.WeaponClass>();

    public Ability(long id, string name, string className) {
        this.id = id;
        this.name = name;
        this.className = className;
    }

    public void AddDuration(float duration)
    {
        if (duration > 0)
        {
            Timer newTimer = new Timer(duration);
            this.duration = newTimer;
            this.hasDuration = true;
        }
    }

    public void AddCooldown(float duration)
    {
        if (duration > 0)
        {
            Timer newTimer = new Timer(duration);
            this.cooldown = newTimer;
            this.hasDuration = true;
        }
    }

    public void CalculateRequirements(Dictionary<InventoryController.InventorySlot,Item> equiptItems)
    {
        bool weaponTypeAllowed = CalculateRequiredWeapon(equiptItems);
        bool weaponClassAllowed = CalculateRequiredClass(equiptItems);

        if (weaponClassAllowed && weaponTypeAllowed)
        {
            isAllowed = true;
        }
        else 
        {
            isAllowed = false;
        }
    }

    private bool CalculateRequiredWeapon(Dictionary<InventoryController.InventorySlot, Item> equiptItems)
    {
        int numberOfTypesRequired = requiredWeaponTypes.Count;

        //check if contains allowed weapon type
        if (numberOfTypesRequired == 0)
        {
             return true;
        }
        else
        {
            foreach (ItemWeapon.WeaponType weaponType in requiredWeaponTypes)
            {
                bool weaponTypeFound = false;

                //if a match for the weapon type cannot be found return false
                foreach (var item in equiptItems)
                {
                    if (item.Value is ItemWeapon)
                    {
                        ItemWeapon wItem = (ItemWeapon)item.Value;
                        if (wItem.weaponType == weaponType)
                        {
                            weaponTypeFound = true;
                        }
                    }
                }

                if (!weaponTypeFound)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool CalculateRequiredClass(Dictionary<InventoryController.InventorySlot, Item> equiptItems)
    {
        int numberOfClassesRequired = requiredWeaponClasses.Count;

        //check if contains allowed weapon class
        if (numberOfClassesRequired == 0)
        {
            return true;
        }
        else
        {
            foreach (ItemWeapon.WeaponClass weaponClass in requiredWeaponClasses)
            {
                bool weaponClassFound = false;

                //if a match for the weapon class cannot be found return false
                foreach (var item in equiptItems)
                {
                    if (item.Value is ItemWeapon)
                    {
                        ItemWeapon wItem = (ItemWeapon)item.Value;
                        if (wItem.weaponClass == weaponClass)
                        {
                            weaponClassFound = true;
                        }
                    }
                }

                if (!weaponClassFound)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
