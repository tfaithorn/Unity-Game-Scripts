using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasteryController : MonoBehaviour
{

    [SerializeField] private List<MasteryInstance> masteryInstances = new List<MasteryInstance>();
    private List<TalentInstance> talentInstances = new List<TalentInstance>();

    public List<TalentInstance> GetTalentInstances()
    {
        return talentInstances;
    }

    public List<MasteryInstance> GetMasteryInstances()
    {
        return masteryInstances;
    }

    public void AddMastery(Mastery mastery, int position)
    {
        masteryInstances.Add(new MasteryInstance(mastery, position));
    }
}
