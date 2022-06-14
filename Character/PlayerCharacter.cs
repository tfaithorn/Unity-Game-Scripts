using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : Character
{
    public KeybindsController keybindsController;
    public Transform cam;
    public Transform camAnchor;
    public AbilityController abilityController;
    public InventoryController inventoryController;
    public AnimationController animationController;
    private MovementController movementController;
    private bool allowAbilities = false;
    private bool allowMovement = false;
    private bool allowIdle = false;

    public Dictionary<KeybindsController.KeyType, Ability> abilityKeys;
    public Dictionary<KeybindsController.KeyType, InputAction> keybinds;

    private Ability abilityHeld;

    InputAction abilityLeftClickAction;
    InputAction abilityRightClickAction;
    InputAction ability1Action;
    InputAction ability2Action;
    InputAction ability3Action;
    InputAction ability4Action;
    InputAction ability5Action;
    InputAction ability6Action;
    InputAction ability7Action;
    InputAction ability8Action;
    InputAction ability9Action;
    InputAction ability10Action;


    private void Awake()
    {
        animationController = GetComponent<AnimationController>();
        movementController = GetComponent<MovementController>();
        abilityController = GetComponent<AbilityController>();
        statsController = GetComponent<StatsController>();
        keybindsController = GetComponent<KeybindsController>();
        inventoryController = GetComponent<InventoryController>();
    }

    private void Start()
    {
        abilityKeys = keybindsController.abilityKeys;
        keybinds = keybindsController.keybinds;
    }

    private void Update()
    {
        Init();
        CheckStatuses();
        CheckAbilities();
        CheckMovement();
        Idle();
    }

    private void Init()
    {
        allowAbilities = true;
        allowMovement = true;
        allowIdle = true;
    }

    private void CheckStatuses()
    {
        List<Buff.StatusType> statusTypes = statsController.GetStatuses();

        if (statusTypes.Exists(x =>
        x == Buff.StatusType.FEAR
        || x == Buff.StatusType.FREEZE
        || x == Buff.StatusType.KNOCKDOWN
        || x == Buff.StatusType.STUN))
        {
            allowAbilities = false;
            allowMovement = false;
        }

        if (statusTypes.Exists(x => x == Buff.StatusType.ROOT))
        {
            allowMovement = false;
        }
    }

    private void CheckAbilities()
    {
        abilityController.allowAbilities = allowAbilities;

        if (!abilityController.AbilityAllowsMovement())
        {
            allowMovement = false;
        }

        if (abilityController.performingAbility)
        {
            allowIdle = false;
        }
    }

    private void CheckMovement()
    {
        movementController.allowMovement = allowMovement;

        if (movementController.isMoving)
        {
            allowIdle = false;
        }
    }

    private void Idle()
    {
        if (allowIdle)
        {
            PlayIdleAnimation();
        }
    }

    private void PlayIdleAnimation()
    {
        animationController.AnimationActionRequest(
               new List<AnimationVariable> { new AnimationVariable("Action", (int)AnimationController.ActionType.IDLE) }
          );

    }
    public void AddAbility(Ability ability, KeybindsController.KeyType keyType)
    {
        keybindsController.AddAbilityKey(keyType, ability);
        abilityController.LoadAbility(ability);
        abilityController.AddAbility(ability);
    }

    public void StartedKeyPress(Ability ability)
    {
        if (ability == null || abilityController == null)
        {
            return;
        }

        if (abilityController.StartAbility(ability))
        {
            Debug.Log("Gets to held?");
            abilityHeld = ability;
        }
    }

    public void EndedKeyPress(KeybindsController.KeyType keyType)
    {

        if (abilityKeys[keyType] == abilityHeld)
        {
            abilityController.ReleaseAbility();
            abilityHeld = null;
        }
        
    }

}
