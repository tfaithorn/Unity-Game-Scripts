using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    /// <summary>
    /// This class will work similar to Weapon. 
    /// The gameobject will be activated when a character is blocking
    /// When a weapon collides with it their attack will be interrupted
    /// </summary>
    /// <param name="col"></param>

    private void OnTriggerEnter(Collider col)
    {

    }
}
