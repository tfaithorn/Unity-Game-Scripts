using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Stores game saves in memory at runtime to improve performance for the save/load panels
/// </summary>
public static class SaveManager
{
    /*
    private static List<Save> saves;

    public static void LoadSaves()
    {
        var saveRepository = new SaveRepository();
        saves = saveRepository.GetByCriteria();
    }

    public static List<Save> GetSavesByPlayerCharacterId(long playerCharacterId)
    {
        var playerSaves = saves.FindAll(x => x.playerCharacter.id == playerCharacterId);
        playerSaves = playerSaves.OrderByDescending(x => x.createdAt).ToList();
        return playerSaves;
    }

    public static void UpdateSave(Save save)
    {
        var existingSave = saves.Find(x => x.id == save.id);
        existingSave.createdAt = save.createdAt;
        existingSave.name = save.name;
        existingSave.playerCharacter = save.playerCharacter;
    }

    public static void AddSave(Save save)
    {
        saves.Add(save);
    }
    */
}
