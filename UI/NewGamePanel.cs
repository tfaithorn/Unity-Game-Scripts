using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewGamePanel : MonoBehaviour
{
    public TMP_InputField nameField;
    private SceneController sceneController;
    public PlayerCharacterMB playerCharacterMB;

    private void Awake()
    {
        sceneController = SceneController.FindSceneController();
    }

    public void StartGame()
    {
        var saveController = SaveController.FindSaveController();
        var name = nameField.text;

        List<Item> items = new List<Item>();
        List<Ability> abilities = new List<Ability>();

        //set default items
        List<ItemInstance> itemInstance = new List<ItemInstance>()
        {
            new ItemInstance(ItemCache.GetItem(1), (int)InventoryController.InventorySlot.WEAPON_R, 1)
        };

        List<AbilityInstance> abilityInstances = new List<AbilityInstance>()
        {
            new AbilityInstance(AbilityCache.GetAbility(1))
        };
        
        abilityInstances[0].isLoaded = true;

        Dictionary<KeybindsController.KeyType, Ability> abilityKeys = new Dictionary<KeybindsController.KeyType, Ability>();
        abilityKeys.Add(KeybindsController.KeyType.ABILITY_1, AbilityCache.GetAbility(1));
        saveController.CreateNewPlayer(name, itemInstance, abilityInstances, abilityKeys);
    }
}
