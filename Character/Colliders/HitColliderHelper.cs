using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void OnRegisterHit(CharacterMB character);
public class HitColliderHelper
{
    //Todo: Look at swapping out HitCollider of interface with OnHit
    public event OnRegisterHit OnRegisterHit;
    private List<string> targetsHit = new List<string>();
    private List<HitCollider> hitColliders;

    public void ActivateHitColliders(List<HitCollider> hitColliders)
    {
        this.hitColliders = hitColliders;
        foreach (HitCollider hitCollider in hitColliders)
        {
            hitCollider.gameObject.SetActive(true);
            //callback to weapon on hit
            hitCollider.OnHit += RegisterHit;
        }
    }
    public void DeactivateHitColliders()
    {
        foreach (HitCollider hitCollider in hitColliders)
        {
            hitCollider.OnHit -= RegisterHit;
            hitCollider.gameObject.SetActive(false);
        }
    }

    private void RegisterHit(CharacterMB enemy)
    {
        //ensure a hit hasn't been registered twice against the same character
        if (!targetsHit.Exists(x => x == enemy.guid.ToString()))
        {
            targetsHit.Add(enemy.guid.ToString());
            OnRegisterHit.Invoke(enemy);
            //ApplyDamage(enemy.statsController);
        }
    }

    public void ResetTargetList()
    {
        targetsHit = new List<string>();
    }

    public void SetHitColliders(HitColliderManager hitColliderManager)
    {
        hitColliders = hitColliderManager.hitColliders;
    }
}
