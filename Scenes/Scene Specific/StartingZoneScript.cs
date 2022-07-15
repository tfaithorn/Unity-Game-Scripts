using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingZoneScript : MonoBehaviour
{
    //This is being moved to the start screen. The idea is to save the game once, and reload it which will load the scene
    /*
    SaveController saveController;

    private void Awake()
    {
        saveController = SaveController.FindSaveController();

        if (saveController.isNewPlayer)
        {
            saveController.playerCharacterHasBeenInitialised.AddListener(AddStartingItems);
        }
    }

    private void AddStartingItems()
    {
        saveController.playerCharacterMB.inventory.AddToInventory(ItemDatabase.GetItem(1));
        saveController.isNewPlayer = false;
        saveController.playerCharacterHasBeenInitialised.RemoveListener(AddStartingItems);
    }
    */
}
