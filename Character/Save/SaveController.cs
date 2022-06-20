using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private PlayerCharacterController playerCharacterController;
    private StatsController statsController;
    private InventoryController inventoryController;
    private AbilityController abilityController;
    private KeybindsController keybindsController;

    Dictionary<KeybindsController.KeyType, Ability> abilityKeys;
    Dictionary<KeybindsController.KeyType, InputAction> keybinds;

    private void Awake()
    {
        playerCharacterController = GetComponent<PlayerCharacterController>();
        statsController = GetComponent<StatsController>();
        inventoryController = GetComponent<InventoryController>();
        abilityController = GetComponent<AbilityController>();
        keybindsController = GetComponent<KeybindsController>();

        playerCharacterController.id = 1;
    }

    private void Start()
    {
        //LoadItems();
        //LoadAbilities();
    }

    private void OnEnable()
    {
        LoadItems();
        LoadAbilities();
    }

    public void LoadAbilities()
    {
        AbilityRepository abilityRepository = new AbilityRepository();
        var abilities = abilityRepository.GetByCriteria(new List<SqlClient.Expr>() { new SqlClient.Cond("id", new long[] {5,7}, SqlClient.OP_IN) });

        Ability longShot = abilities[0];
        Ability explodingShot = abilities[1];
        Debug.Log(longShot.icon);
        playerCharacterController.AddAbility(longShot, KeybindsController.KeyType.RIGHT_CLICK);
        playerCharacterController.AddAbility(explodingShot, KeybindsController.KeyType.ABILITY_2);

        abilityKeys = keybindsController.abilityKeys;
        keybinds = keybindsController.keybinds;

        playerCharacterController.abilityKeys = abilityKeys;
        playerCharacterController.keybinds = keybinds;
    }

    public void LoadItems()
    {
        var criteria = new List<SqlClient.Expr>()
        {
            new SqlClient.Cond("characterId", playerCharacterController.id, SqlClient.OP_EQUAL)
        };

        CharacterItemRepository characterItemRepository = new CharacterItemRepository();
        inventoryController.characterItems = characterItemRepository.GetByCriteria(criteria);
    }

}

