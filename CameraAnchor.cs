using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to place invisible anchor for the camera behind the player
/// </summary>
public class CameraAnchor : MonoBehaviour
{
    public PlayerCharacterMB playerCharacterController;
    private Transform characterModelTransform;
    private float yOffset;
    private float zOffset;

    private void Start() {
        characterModelTransform = playerCharacterController.characterModelTransform;
        yOffset = 2f;
        zOffset = 0.1f;
    }

    private void FixedUpdate()
    {

        //follow the character model without being parented so that the rotation can remain independent for both
        transform.position = characterModelTransform.position + characterModelTransform.TransformDirection(new Vector3(0, yOffset, zOffset));
    }
}
