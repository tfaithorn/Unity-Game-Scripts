using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private PlayerCharacter playerCharacter;
    private StatsController statsController;
    private InventoryController inventoryController;
    private AbilityController abilityController;
    private KeybindsController keybindsController;

    Dictionary<KeybindsController.KeyType, Ability> abilityKeys;
    Dictionary<KeybindsController.KeyType, InputAction> keybinds;

    private void Awake()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        statsController = GetComponent<StatsController>();
        inventoryController = GetComponent<InventoryController>();
        abilityController = GetComponent<AbilityController>();
        keybindsController = GetComponent<KeybindsController>();

        playerCharacter.id = 1;
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
        //load abilities for testing
        /*
        Ability basicAttack = Ability.GetByCriteria(new List<SqlClient.Expr>(){new SqlClient.Cond("id",(long)1,SqlClient.OP_EQUAL)});

        playerCharacter.AddAbility(basicAttack, KeybindsController.KeyType.LEFT_CLICK);
      
        playerCharacter.AddAbility(basicAttack, KeybindsController.KeyType.ABILITY_1);
        */
        Ability longShot = Ability.GetByCriteria(new List<SqlClient.Expr>() { new SqlClient.Cond("id", 5, SqlClient.OP_EQUAL)});
        playerCharacter.AddAbility(longShot, KeybindsController.KeyType.RIGHT_CLICK);

        Ability explodingShot = Ability.GetByCriteria(new List<SqlClient.Expr>() { new SqlClient.Cond("id", 7, SqlClient.OP_EQUAL)});
        playerCharacter.AddAbility(explodingShot, KeybindsController.KeyType.ABILITY_2);

        abilityKeys = keybindsController.abilityKeys;
        keybinds = keybindsController.keybinds;

        playerCharacter.abilityKeys = abilityKeys;
        playerCharacter.keybinds = keybinds;

    }

    public void LoadItems()
    {
        var criteria = new List<SqlClient.Expr>()
        {
            new SqlClient.Cond("characterId", playerCharacter.id, SqlClient.OP_EQUAL)
        };

        inventoryController.characterItems = CharacterItem.GetByCriteria(criteria);
    }

}

