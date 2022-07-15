using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityScript : MonoBehaviour
{
    //flags to set to manipulate ability behaviour at runtime
    public bool isChanneled;
    public bool isInterruptable;
    public bool allowMovement;
    public bool isInfinite;
    public bool stopAbility;
    public bool isPerformed;
    public bool isHeld;
    public bool showAbility;
    public bool hasCooldown;

    public AbilityScript() {
        this.isChanneled = false;
        this.hasCooldown = false;
        this.isInfinite = false;
        this.allowMovement = true;
        this.stopAbility = false;
        this.isPerformed = true;
        this.showAbility = true;
        this.isInterruptable = false;
    }

    public abstract float GetDuration(CharacterMB characterMB);
    public abstract float GetCooldown(CharacterMB characterMB);
    public abstract bool GetInterruptable(CharacterMB characterMB);

    /// <summary>
    /// Called when they ability is loaded on the gameobject
    /// </summary>
    /// <param name="ability"></param>
    public virtual void LoadAbility(Ability ability){ }

    /// <summary>
    /// Called the first frame an ability is started
    /// </summary>
    public virtual void StartAbility() { }

    /// <summary>
    /// Called each frame the ability is being held or cast
    /// </summary>
    public virtual void PerformAbility() { }

    /// <summary>
    /// Called the frame an ability finishes 
    /// </summary>
    public virtual void FinishAbility() { }

    /// <summary>
    /// Called to reset the parameters of an ability like when you are doing a combo and need to reset to the first move
    /// </summary>
    public virtual void ResetAbility() { }

    /// <summary>
    /// called when an ability is interrupted
    /// </summary>

    public virtual void InterruptAbility() { }

    /// <summary>
    /// Called when a key is released
    /// </summary>
    public virtual void ReleaseAbility() { }
}
