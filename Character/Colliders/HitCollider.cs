using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnHitEvent(Character character);

public class HitCollider : MonoBehaviour, iHitInterface
{
    public event OnHitEvent OnHit;
    
    private void OnTriggerEnter(Collider col)
    {
        Hittable hittable = col.GetComponent<Hittable>();
        if (hittable)
        {
            Debug.Log("Collider hit?");
            OnHit?.Invoke(hittable.character);
        }
    }

}
