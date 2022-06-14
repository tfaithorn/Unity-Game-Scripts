using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSlopes : MonoBehaviour
{
    private Rigidbody rb;
    public MovementController movementController;

    bool nullifyGravity = false;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision col) {

        var tags = col.gameObject.GetComponent<CustomTags>();

        if (tags)
        {

            if (tags.HasTag(CustomTags.TagTypes.SLOPE))
            {
                nullifyGravity = true;

            }
        }
    }

    private void OnCollisionExit(Collision col)
    {
        var tags = col.gameObject.GetComponent<CustomTags>();

        if (tags)
        {

            if (tags.HasTag(CustomTags.TagTypes.SLOPE))
            {
                nullifyGravity = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (nullifyGravity)
        {
            rb.AddForce(-Physics.gravity);
        }
    }

}
