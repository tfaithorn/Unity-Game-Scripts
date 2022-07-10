using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AbilityInterface
{
    

    //called when the ability class is added. Called Once
    void LoadAbility(Ability ability);

    //called when the ability is added for use, such as when added to a keybind
    void AddAbility(Ability ability);

    //called once when the ability is performed
    void StartAbility();

    //called each frame the ability is occurring / being held
    void CastAbility();

    //called when the ability finishes
    void FinishAbility();

    //called to reset the parameters of an ability, like when you are doing a basic attack
    void ResetAbility();

    //called when the ability is interrupted
    void InterruptAbility();

    //TODO: Add interface to return tooltip. First requires making tooltip class
}
