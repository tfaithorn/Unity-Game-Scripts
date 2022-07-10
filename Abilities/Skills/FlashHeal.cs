using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashHeal : MonoBehaviour, AbilityInterface
{
    public void LoadAbility(Ability ability) { }
    public void AddAbility(Ability ability){  }
    public void StartAbility(){
        Debug.Log("Flash heal started?");
    }
    public void CastAbility(){}
    public void FinishAbility(){}
    public void InterruptAbility(){ }
    public void ResetAbility() { }
}
