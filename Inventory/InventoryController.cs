using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryController : Inventory
{
    public Transform weaponRBone;
    public Transform weaponLBone;

    private UIController uIController;
    private AnimationController animationController;
    private AbilityController abilityController;

    private GameObject leftWeaponGameObject;
    private GameObject rightWeaponGameObject;

    [System.NonSerialized] public bool allowDualWield = true;
    public enum InventorySlot {
        NONE,
        WEAPON_R = 1,
        WEAPON_L = 2,
        HEAD = 3,
        CHEST = 4,
        LEGS = 5,
        HANDS = 6,
        FEET = 7,
        RING1 = 8,
        RING2 = 9,
        NECK = 10
    }

    public Dictionary<InventorySlot, Item> equiptItems = new Dictionary<InventorySlot, Item>() {
        {InventorySlot.WEAPON_R, null},
        {InventorySlot.WEAPON_L, null},
        {InventorySlot.HEAD, null},
        {InventorySlot.CHEST, null},
        {InventorySlot.LEGS, null},
        {InventorySlot.HANDS, null},
        {InventorySlot.FEET, null},
        {InventorySlot.RING1, null},
        {InventorySlot.RING2, null},
        {InventorySlot.NECK, null}
    };

    private void Awake()
    {
        abilityController = GetComponent<AbilityController>();
        character = GetComponent<CharacterMB>();
        uIController = GetComponent<UIController>();
        animationController = GetComponent<AnimationController>();
    }

    public void EquiptItem(Item item, InventorySlot inventorySlot = InventorySlot.NONE)
    {
        /*
        //check if you are moving an item to a slot it is already equipt in
        if (inventorySlot != InventorySlot.NONE && equiptItems[inventorySlot] != null)
        {
            if (item == equiptItems[inventorySlot])
            {
                return false;
            }
        }\
        //check if slot is valid
        if (inventorySlot != InventorySlot.NONE && !CheckIfSlotIsValid(item, inventorySlot))
        {
            return false;
        }

        //check if space exists in inventory if item is equipt
        if (inventorySlot == InventorySlot.NONE)
        {
            inventorySlot = CalculateInventorySlotForItem(item);
        }

        if (item is ItemWeapon)
        {
            //if you are moving weapon from right to left or left to right
            if (CheckForSlotSwapped(item, inventorySlot))
            {
                return true;
            }

            //safe to remove item from inventory, please proceed
            item.position = -1;
            this.RemoveFromInventory(item);

            //unequipt items. If unable to unequipt item return false
            if (!CheckIfUnequiptIsRequired(item, inventorySlot))
            {
                return false;
            }

            ItemWeapon itemWeapon = (ItemWeapon)item;
            ItemWeapon rightHandItem = (ItemWeapon)equiptItems[InventorySlot.WEAPON_R];
            ItemWeapon leftHandItem = (ItemWeapon)equiptItems[InventorySlot.WEAPON_L];

            EquiptWeapon(itemWeapon, inventorySlot);
        }


        if (item is ItemArmour)
        {
            //note need to check if slot is valid

            EquiptArmour((ItemArmour)item, inventorySlot);

            ItemArmour itemArmour = (ItemArmour)item;
            switch (itemArmour.armourType) {
                case ItemArmour.ArmourType.CHEST:
                    EquiptArmour(itemArmour, InventorySlot.CHEST);
                    break;
                case ItemArmour.ArmourType.FEET:
                    EquiptArmour(itemArmour, InventorySlot.FEET);
                    break;
                case ItemArmour.ArmourType.HANDS:
                    EquiptArmour(itemArmour, InventorySlot.HANDS);
                    break;
                case ItemArmour.ArmourType.HEAD:
                    EquiptArmour(itemArmour, InventorySlot.HEAD);
                    break;
                case ItemArmour.ArmourType.LEGS:
                    EquiptArmour(itemArmour, InventorySlot.LEGS);
                    break;
                case ItemArmour.ArmourType.RINGS:
                    EquiptArmour(itemArmour, InventorySlot.RING1);
                    break;
                case ItemArmour.ArmourType.NECK:
                    EquiptArmour(itemArmour, InventorySlot.NECK);
                    break;
            }
        }
        return true;
        */
    }

    private void EquiptWeapon(ItemWeapon itemWeapon, InventorySlot slot)
    {
        GameObject itemPrefab = Resources.Load<GameObject>(Constants.prefabsPath +"/"+ itemWeapon.prefabPath);
        animationController.SetWeaponClass(itemWeapon.weaponClass);

        itemPrefab = Instantiate(itemPrefab);
        leftWeaponGameObject = itemPrefab;

        if (itemWeapon.weaponClass == ItemWeapon.WeaponClass.MELEE)
        {
            itemPrefab.transform.position = itemPrefab.transform.position + weaponRBone.position;
            itemPrefab.transform.parent = weaponRBone;

            HitColliderManager weaponColliderManager  = itemPrefab.GetComponent<HitColliderManager>();
            itemWeapon.hitColliders = weaponColliderManager.hitColliders;
        }

        if (itemWeapon.weaponClass == ItemWeapon.WeaponClass.RANGED)
        {
            itemPrefab.transform.position = itemPrefab.transform.position + weaponLBone.position;
            itemPrefab.transform.rotation = weaponLBone.rotation;
            itemPrefab.transform.parent = weaponLBone;
        }

        equiptItems[slot] = itemWeapon;
        

        CheckAbilityRequirements();
    }

    public bool UnEquiptItem(Item item)
    {
        /*
        if (CalculateFreeSpace() == 0)
        {
            return false;
        }

        InventorySlot slotToRemove = InventorySlot.NONE;

        foreach (var itemVar in equiptItems)
        {
            if (item == itemVar.Value)
            {
                slotToRemove = itemVar.Key;

                if (item is ItemWeapon)
                {
                    UnEquiptWeapon((ItemWeapon)item, itemVar.Key);
                }
            }
        }

        if (slotToRemove != InventorySlot.NONE)
        {
            equiptItems[slotToRemove] = null;
            AddToInventory(item);
            return true;
        }
        */
        return false;
    }

    private void UnEquiptWeapon(ItemWeapon itemWeapon, InventorySlot slot)
    {
        /*
        //special case for bows because the object is placed in your left-hand
        if (itemWeapon.weaponType == ItemWeapon.WeaponType.BOW)
        {
            Destroy(leftWeaponGameObject);
            return;
        }

        if (slot == InventorySlot.WEAPON_L)
        {
            Destroy(leftWeaponGameObject);
        }

        if (slot == InventorySlot.WEAPON_R)
        {
            Destroy(rightWeaponGameObject);
        }
        */
    }

    private void EquiptArmour(ItemArmour itemArmour, InventorySlot slot)
    {
        equiptItems[slot] = itemArmour;
    }
    public void CheckAbilityRequirements()
    {
        List<Ability> abilitiesAvailable = abilityController.abilitiesAvailable;

        //disable abilities if they cannot be used with weapon
        foreach (Ability ability in abilitiesAvailable)
        {
            ability.CalculateRequirements(equiptItems);
        }
    }

    public InventorySlot GetEquiptSlot(Item item)
    {
        foreach (var equiptItem in equiptItems)
        {
            if (equiptItem.Value == item)
            {
                return equiptItem.Key;
            }
        }

        return InventorySlot.NONE;
    }

    private InventorySlot CalculateInventorySlotForItem(Item item)
    {
        if (item is ItemWeapon)
        {
            return InventorySlot.WEAPON_R;
        }

        if (item is ItemArmour)
        {
            ItemArmour itemArmour = (ItemArmour)item;
            switch (itemArmour.armourType) {
                case ItemArmour.ArmourType.CHEST:
                    return InventorySlot.CHEST;
                case ItemArmour.ArmourType.FEET:
                    return InventorySlot.FEET;
                case ItemArmour.ArmourType.HANDS:
                    return InventorySlot.HANDS;
                case ItemArmour.ArmourType.HEAD:
                    return InventorySlot.HEAD;
                case ItemArmour.ArmourType.LEGS:
                    return InventorySlot.LEGS;
                case ItemArmour.ArmourType.NECK:
                    return InventorySlot.NECK;
                case ItemArmour.ArmourType.RINGS:
                    if (equiptItems[InventorySlot.RING1] == null)
                    {
                        return InventorySlot.RING1;
                    }
                    else if (equiptItems[InventorySlot.RING2] == null)
                    {
                        return InventorySlot.RING2;
                    } else {
                        return InventorySlot.RING1;
                    }
            }
        }

        //Todo: make this work
        return InventorySlot.NONE;
    }

    private bool CheckIfSlotIsValid(Item item, InventorySlot inventorySlot)
    {
        if (item is ItemWeapon)
        {
            ItemWeapon itemWeapon = (ItemWeapon)item;
            switch (inventorySlot)
            { 
                case InventorySlot.WEAPON_R:
                    return true;
                case InventorySlot.WEAPON_L:
                    if (
                        (itemWeapon.weaponType == ItemWeapon.WeaponType.ONE_HAND && allowDualWield)
                        || itemWeapon.weaponType == ItemWeapon.WeaponType.OFF_HAND)
                    {
                        return true;
                    }
                    break;
            }
        }

        if (item is ItemArmour)
        {
            ItemArmour itemArmour = (ItemArmour)item;

            switch (inventorySlot)
            {
                case InventorySlot.CHEST:
                    if (itemArmour.armourType == ItemArmour.ArmourType.CHEST)
                    {
                        return true;
                    }
                    break;
                case InventorySlot.FEET:
                    if (itemArmour.armourType == ItemArmour.ArmourType.FEET)
                    {
                        return true;
                    }
                    break;
                case InventorySlot.HANDS:
                    if (itemArmour.armourType == ItemArmour.ArmourType.HANDS)
                    {
                        return true;
                    }
                    break;
                case InventorySlot.HEAD:
                    if (itemArmour.armourType == ItemArmour.ArmourType.HEAD)
                    {
                        return true;
                    }
                    break;
                case InventorySlot.LEGS:
                    if (itemArmour.armourType == ItemArmour.ArmourType.LEGS)
                    {
                        return true;
                    }
                    break;
                case InventorySlot.NECK:
                    if (itemArmour.armourType == ItemArmour.ArmourType.NECK)
                    {
                        return true;
                    }
                    break;
                case InventorySlot.RING1:
                    if (itemArmour.armourType == ItemArmour.ArmourType.RINGS)
                    {
                        return true;
                    }
                    break;
                case InventorySlot.RING2:
                    if (itemArmour.armourType == ItemArmour.ArmourType.RINGS)
                    {
                        return true;
                    }
                    break;
            }
        }

        return false;
    }

    private bool IsReplacingItem(Item item, InventorySlot inventorySlot)
    {
        if(equiptItems[inventorySlot] != null)
        {
            return true;
        }

        return false;
    }

    private bool CheckForSlotSwapped(Item item, InventorySlot inventorySlot)
    {
        if (item is ItemWeapon)
        {
            ItemWeapon itemWeapon = (ItemWeapon)item;
            ItemWeapon rightHandItem = (ItemWeapon)equiptItems[InventorySlot.WEAPON_R];
            ItemWeapon leftHandItem = (ItemWeapon)equiptItems[InventorySlot.WEAPON_L];

            //moving from right to left
            if (inventorySlot == InventorySlot.WEAPON_L
                && item == rightHandItem)
            {
                //if left-hand is occupied
                if (leftHandItem != null)
                {
                    ItemWeapon tempRight = rightHandItem;

                    UnEquiptWeapon(leftHandItem, InventorySlot.WEAPON_L);
                    UnEquiptWeapon(rightHandItem, InventorySlot.WEAPON_R);

                    EquiptWeapon(tempRight, InventorySlot.WEAPON_L);
                    EquiptWeapon(leftHandItem, InventorySlot.WEAPON_R);

                    return true;
                }

                //if left-hand is not occuppied
                if (equiptItems[InventorySlot.WEAPON_L] == null)
                {
                    UnEquiptWeapon(rightHandItem, InventorySlot.WEAPON_R);
                    equiptItems[InventorySlot.WEAPON_R] = null;

                    EquiptWeapon(rightHandItem, InventorySlot.WEAPON_L);
                    return true;
                }

            }

            //moving from left to right
            if (inventorySlot == InventorySlot.WEAPON_R
                && item == leftHandItem)
            {
                Debug.Log("Moving from left to right?");
                //if right-hand is occupied
                if (rightHandItem != null)
                {
                    ItemWeapon tempLeft = leftHandItem;

                    UnEquiptWeapon(leftHandItem, InventorySlot.WEAPON_L);
                    UnEquiptWeapon(rightHandItem, InventorySlot.WEAPON_R);

                    EquiptWeapon(tempLeft, InventorySlot.WEAPON_R);
                    EquiptWeapon(rightHandItem, InventorySlot.WEAPON_L);

                    return true;
                }

                //if right-hand is not occuppied
                if (equiptItems[InventorySlot.WEAPON_R] == null)
                {
                    UnEquiptWeapon(leftHandItem, InventorySlot.WEAPON_L);
                    equiptItems[InventorySlot.WEAPON_L] = null;

                    EquiptWeapon(leftHandItem, InventorySlot.WEAPON_R);
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckIfUnequiptIsRequired(Item item, InventorySlot inventorySlot)
    {
        //Note: This function assumes the check for swapping has already been completed
        if (item is ItemWeapon)
        {
            ItemWeapon itemWeapon = (ItemWeapon)item;
            ItemWeapon rightHandItem = (ItemWeapon)equiptItems[InventorySlot.WEAPON_R];
            ItemWeapon leftHandItem = (ItemWeapon)equiptItems[InventorySlot.WEAPON_L];

            //if you are equipting an item in the left-hand slot and the right-hand is invalid
            if (allowDualWield
                && rightHandItem != null
                && itemWeapon.weaponType == ItemWeapon.WeaponType.ONE_HAND
                && rightHandItem.weaponType != ItemWeapon.WeaponType.ONE_HAND
                && inventorySlot == InventorySlot.WEAPON_L)
            {
                if (!UnEquiptItem(rightHandItem))
                    return false;
            }
            else if (itemWeapon.requiresTwoHands && leftHandItem != null && rightHandItem != null)
            {
                if (!UnEquiptItem(leftHandItem) || !UnEquiptItem(rightHandItem))
                {
                    return false;
                }
            }
            else if (itemWeapon.requiresTwoHands && leftHandItem != null)
            {
                Debug.Log("Got able to unequipt left hand?");
                if (!UnEquiptItem(leftHandItem))
                {
                    return false;
                }
            }
            else if (inventorySlot == InventorySlot.WEAPON_R && rightHandItem != null)
            {
                if (!UnEquiptItem(rightHandItem))
                    return false;
            }

            else if (inventorySlot == InventorySlot.WEAPON_L && leftHandItem != null)
            {
                if (!UnEquiptItem(leftHandItem))
                    return false;
            }
        }

        return true;
    }

}
