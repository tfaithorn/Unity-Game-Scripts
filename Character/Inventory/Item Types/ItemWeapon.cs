using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : Item
{
    public enum WeaponType
    {
        NONE = 0,
        ONE_HAND = 1,
        TWO_HAND = 2,
        SPEAR = 3,
        OFF_HAND = 5,
        BOW = 6
    }
    public enum WeaponClass
    {
        NONE = 0,
        MELEE = 1,
        RANGED = 2,
        OFF_HAND = 3
    }

    public float damage;
    public float speed;
    public string prefabPath;
    public WeaponClass weaponClass;
    public WeaponType weaponType;
    public List<HitCollider> hitColliders;
    public bool requiresTwoHands;

    public ItemWeapon(Item item) : base(item)
    {

    }

    public void SetWeaponProperties()
    {
        if (weaponType == WeaponType.BOW)
        {
            this.weaponClass = WeaponClass.RANGED;
        }

        if (weaponType == WeaponType.ONE_HAND
            || weaponType == WeaponType.TWO_HAND
            || weaponType == WeaponType.SPEAR)
        {
            this.weaponClass = WeaponClass.MELEE;
        }

        if (weaponType == WeaponType.TWO_HAND
            || weaponType == WeaponType.BOW
            || weaponType == WeaponType.SPEAR)
        {
            this.requiresTwoHands = true;
        }
        else
        {
            this.requiresTwoHands = false;
        }
    }
}
