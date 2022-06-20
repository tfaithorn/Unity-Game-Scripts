using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveService
{
    private readonly SaveRepository saveRepository;

    public SaveService(SaveRepository saveRepository)
    {
        this.saveRepository = saveRepository;
    }

    public void GetHighestSave()
    { 
    
    }
}
