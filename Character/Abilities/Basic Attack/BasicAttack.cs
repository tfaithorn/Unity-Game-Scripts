using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicAttack : AbilityScript
{
    protected InventoryController inventoryController;
    private ItemWeapon.WeaponClass weaponClass;
    private AbilityController abilityController;
    private AbilityPanel abilityPanel;
    private UIController UIController;
    protected KeybindsController keybindsController;
    private Ability rangedAbility;
    private Ability meleeAbility;
    private PlayerCharacter playerCharacter;

    //delegates used by melee & ranged attack
    /*
    public delegate void StartAbilityEvent();
    public event StartAbilityEvent startAbilityEvent;

    public delegate void CastAbilityEvent();
    public event CastAbilityEvent castAbilityEvent;
    */
    private void Awake()
    {
        inventoryController = GetComponent<InventoryController>();
        abilityController = GetComponent<AbilityController>();
        UIController = GetComponent<UIController>();
        abilityPanel = UIController.abilityPanel.GetComponent<AbilityPanel>();
        keybindsController = GetComponent<KeybindsController>();
        playerCharacter = GetComponent<PlayerCharacter>();
    }

    public override void AddAbility(Ability ability) 
    {
        /*
        //replace keybinds with ranged ability and update icon
        List<KeybindsController.KeyType> keys = keybindsController.GetKeysByAbility(ability);
        foreach (var key in keys)
        {
            //keybindsController.AddAbilityKey(key,rangedAbility);
            playerController.AddAbility(rangedAbility, key);
        }
        */
    }

    public override void LoadAbility(Ability ability)
    {
        ItemWeapon weapon = (ItemWeapon)inventoryController.equiptItems[InventoryController.InventorySlot.WEAPON_R];
        if (weapon == null) {
            return;
        }

        weaponClass = weapon.weaponClass;

        switch (weapon.weaponClass)
        {
            case ItemWeapon.WeaponClass.MELEE:
               
                //BasicMeleeAttack basicMeleeAttack = GetComponent<BasicMeleeAttack>();

                //if (!basicMeleeAttack) {
                //    basicMeleeAttack = gameObject.AddComponent<BasicMeleeAttack>();    
                //}
                

                break;
            case ItemWeapon.WeaponClass.RANGED:
                BasicRangedAttack basicRangedAttack = GetComponent<BasicRangedAttack>();

                if (!basicRangedAttack)
                {
                    basicRangedAttack = gameObject.AddComponent<BasicRangedAttack>();
                }

                ability.icon = "basic-ranged-attack-icon";
                ability.hasDuration = false;
                ability.abilityScript = basicRangedAttack;

                Debug.Log("Basic Attack called?");
                abilityPanel.UpdateImagesByAbility(ability);

                basicRangedAttack.abilityRef = ability;

                break;
        }
        
    }
    public override void StartAbility()
    {
        //startAbilityEvent?.Invoke();
    }
    public override void PerformAbility()
    {
        //castAbilityEvent?.Invoke();
    }
    public override void FinishAbility(){}
    public override void ResetAbility(){}

    public override void ReleaseAbility() { }
    public override void InterruptAbility(){}

}
