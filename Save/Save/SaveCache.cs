using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveCache
{
    private static Dictionary<long, Save> saves = new Dictionary<long, Save>();
    static SaveCache()
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

    public static void AddSave(Save save)
    {
        saves[save.id] = save;
    }
}
