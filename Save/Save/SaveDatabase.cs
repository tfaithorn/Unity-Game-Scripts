using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveDatabase
{
    private static Dictionary<long, Save> saves = new Dictionary<long, Save>();
    static SaveDatabase()
    {
        var saveRepository = new SaveRepository();

        var savesList = saveRepository.GetByCriteria();

        foreach (Save save in savesList)
        {
            saves.Add(save.id, save);
        }
    }

    public static Save GetSave(long saveId)
    {
        return saves[saveId];
    }
}
