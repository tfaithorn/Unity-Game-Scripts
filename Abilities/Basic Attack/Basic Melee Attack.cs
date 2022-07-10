using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeAttack : BasicAttack
{
    private Ability abilityRef;
    //private InventoryController inventoryController;
    private HitColliderHelper hitWeaponGroup;
    private PlayerCharacterMB character;
    private AnimationController animationController;
    //private InventoryController inventoryController;

    private void Awake()
    {
        character = (PlayerCharacterMB)GetComponent<CharacterMB>();
    }

    private void Start()
    {
        animationController = character.animationController;
        

        //callback to tell when collider has been hit
        hitWeaponGroup = new HitColliderHelper();
        hitWeaponGroup.OnRegisterHit += ApplyDamage;
    }

    private new void AddAbility(Ability ability)
    {
        abilityRef = ability;
    }

    private new void StartAbility()
    {
        ItemWeapon weapon = (ItemWeapon)inventoryController.equiptItems[InventoryController.InventorySlot.WEAPON_R];
        List<HitCollider> hitColliders = weapon.hitColliders;

        abilityRef.abilityScript.isInfinite = false;
        hitWeaponGroup.ActivateHitColliders(hitColliders);
        PlayMeleeAnimation(abilityRef);
    }

    private void ApplyDamage(CharacterMB enemyCharacter)
    {
        character.statsController.DealDamage(BuildDamage(), enemyCharacter.statsController);
    }

    private DamageRequest BuildDamage()
    {
        DamageRequest damageRequest = new DamageRequest();
        damageRequest.isMelee = true;
        damageRequest.AddDamage(StatsController.DamageType.PHYSICAL, 5f);
        return damageRequest;
    }

    private void PlayMeleeAnimation(Ability ability)
    {
        animationController.AnimationActionRequest(
           new List<AnimationVariable> {
               new AnimationVariable("Action", (int)AnimationController.ActionType.ABILITY),
               new AnimationVariable("Ability", 1),
               new AnimationVariable("Phase", 1)
           }
        );
    }

    private new void FinishAbility()
    {
        hitWeaponGroup.DeactivateHitColliders();
    }

    private new void CastAbility()
    {
        Debug.Log("Test Cast");
    }

}