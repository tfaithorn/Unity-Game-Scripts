using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Stores game saves in memory at runtime to improve performance for the save/load panels
/// </summary>
public static class SaveManager
{
    private static List<Save> saves;

    public static void LoadSaves()
    {
        var saveRepository = new SaveRepository();
        saves = saveRepository.GetByCriteria();
    }

    public static List<Save> GetSavesByPlayerCharacterId(long playerCharacterId)
    {
        return saves.FindAll(x => x.playerCharacter.id == playerCharacterId);
    }

    public static void AddSave(Save save)
    {
        saves.Add(save);
    }

}
