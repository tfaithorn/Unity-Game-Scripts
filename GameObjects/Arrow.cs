using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [HideInInspector] public bool pierce = false;
    private float timeInFlight = 0f;
    public float defaultTimeTillFall;
    public List<Transform> ignoreTransforms = new List<Transform>();

    private bool isReleased = false;

    private float currentTimeToFall = 0f;
    private bool isTargetHit = false;
    private Rigidbody rb;

    public delegate void OnColliderHit(Transform transform);
    public event OnColliderHit colliderHitEvent;

    public GameObject fireParticles;

    private float minimumExtent;
    private float partialExtent;
    private float skinWidth = 0.1f;
    private float sqrMinimumExtent;

    Vector3 previousPosition;
    Collider myCollider;
    // collision detection script https://github.com/omgwtfgames/unity-bowerbird/blob/master/Scripts/General/DontGoThroughThings.cs

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ignoreTransforms.Add(transform);

        myCollider = GetComponent<Collider>();



        previousPosition = rb.position;
        minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;
    }

    private void FixedUpdate()
    {
        if (isReleased && !isTargetHit)
        {
            DetectCollison();

            timeInFlight += Time.deltaTime;

            if (timeInFlight >= currentTimeToFall)
            {
                rb.useGravity = true;
            }
        }
    }

    /*
    private void OnCollisionEnter(Collision col)
    {
        colliderHitEvent?.Invoke(col.transform);
        FreezeArrow(col.transform);
    }
    */
    /*
    private void OnTriggerEnter(Collider col)
    {
        colliderHitEvent?.Invoke(col.transform);
        FreezeArrow(col.transform);
        PlaceArrow();
    }
    */

    private void FreezeArrow(Transform targetTransform)
    {
        isTargetHit = true;
        rb.isKinematic = true;
        rb.velocity = new Vector3(0, 0, 0);
        transform.parent = targetTransform;
        timeInFlight = 0f;
    }

    public void ReleaseArrow()
    {
        isReleased = true;
    }

    public void SetTimeToFall(float time)
    {
        currentTimeToFall = time;
    }

    public void ResetArrow()
    {
        isTargetHit = false;
        rb.isKinematic = true;
        rb.velocity = new Vector3(0, 0, 0);
        isReleased = false;
        timeInFlight = 0f;
        fireParticles.SetActive(false);
    }

    /*
    private void PlaceArrow()
    {
        Vector3 dir = (pointPreviousPosition - point.position).normalized;
        float distance = Vector3.Distance(pointPreviousPosition, point.position);
        RaycastHit hit;
        previousHitPoint = pointPreviousPosition;

        Debug.DrawRay(transform.position, -transform.forward);
        
        if (Physics.Raycast(point.position + -point.forward, transform.forward, out hit, 1f))
        {
            Debug.Log("Did it hit?" + hit.transform.name);
            transform.position = hit.point;
            hitPoint = hit.point;

        }
    }
    */

    private void DetectCollison()
    {
        //have we moved more than our minimum extent? 
        Vector3 movementThisStep = rb.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit hitInfo;

            //check for obstructions we might have missed 
            if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude))
            {
                
                if (!ignoreTransforms.Exists(x => x == hitInfo.transform))
                {
                    FreezeArrow(hitInfo.transform);
                    transform.position = hitInfo.point;
                    colliderHitEvent?.Invoke(hitInfo.transform);
                }
                
            }
        }

        previousPosition = rb.position;

    }

    public void ShowFireParticles()
    {
        fireParticles.SetActive(true);
    }




}
