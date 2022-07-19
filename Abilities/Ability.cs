using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class Ability
{
    public long id;
    public string name;
    public string icon;
    public string abilityScriptName;
    public List<AbilityTags> tags;
    public bool isAllowed;

    public abstract string GetDescription(CharacterMB characterMB);

    public List<ItemWeapon.WeaponType> requiredWeaponTypes = new List<ItemWeapon.WeaponType>();
    public List<ItemWeapon.WeaponClass> requiredWeaponClasses = new List<ItemWeapon.WeaponClass>();

    public bool CalculateRequirements(Dictionary<InventoryController.InventorySlot,Item> equiptItems)
    {
        bool weaponTypeAllowed = CalculateRequiredWeapon(equiptItems);
        bool weaponClassAllowed = CalculateRequiredWeaponClass(equiptItems);

        if (weaponClassAllowed && weaponTypeAllowed)
        {
            return true;
        }
        else 
        {
            return false;
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

    private bool CalculateRequiredWeaponClass(Dictionary<InventoryController.InventorySlot, Item> equiptItems)
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

    /*
    public abstract float GetDuration(CharacterMB characterMB);
    public abstract float GetCooldown(CharacterMB characterMB);
    public abstract bool GetInterruptable(CharacterMB characterMB);
    */
}
