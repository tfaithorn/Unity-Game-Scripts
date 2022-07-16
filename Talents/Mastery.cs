using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mastery
{
    public long id;
    public string name;
    public string previewImage;
    public string description;
    protected List<TalentInstance> talentInstances;

}
