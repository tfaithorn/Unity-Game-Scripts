using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    public Camera cam;
    public Transform cameraAnchor;
    private KeybindsController keybindsController;

    private bool isFollowPlayer = false;
    private bool moveAnchorToPosition = false;
    private Vector3 newAnchorPosition;

    private float minZDistance = 10f;
    private float maxZDistance = 100f;

    private float zStep = 0.5f;

    private float initialYRotation = 0f;
    private float initialXRotation = 70f;
    private float rotationSpeed = 4f;
    private float zoomSpeed = 2f;

    public float camZDistance = 40f;
    private float camYDistance = 2f;

    public float rotationX;
    public float rotationY;

    private bool allowZoom = true;
    private bool allowLook = true;

    private bool shakeCamera = false;
    private float shakeX = 0f;
    private float shakeY = 0f;
    private float previousShakeMagnitudeX = 0f;
    private float previousShakeMagnitudeY = 0f;
    private float shakeMagnitudeX;
    private float shakeMagnitudeY;
    private float shakeMagnitude;
    private Timer shakeMagnitudeTimer;



    public readonly float zoomShootingDistance = 10f;
    private bool zoomOverTime = false;
    private float zoomOverTimeDistance = 0f;
    private float zoomOverTimePassed = 0f;
    private float zoomOverTimeDuration = 0f;
    private float zoomInitialDistance = 0f;

    private LayerMask terrainLayerMask = 1 << 6;

    //The camera moves on the Z & Y axis and the anchor rotates

    private void Awake()
    {
        keybindsController = GetComponent<KeybindsController>();
    }

    private void Start()
    {
        keybindsController.lookEvent += CameraInput;

        //RotateAnchor(initialXRotation, initialYRotation);

        //middle mouse scroll
        keybindsController.keybinds[KeybindsController.KeyType.ZOOM].performed += context => {

            if (!allowZoom)
            {
                return;
            }

            if (context.ReadValue<float>() > 0)
            {
                if ((camZDistance - zStep) > minZDistance)
                {
                    camZDistance -= zStep * zoomSpeed;
                }
                else {
                    camZDistance = minZDistance;
                }

            } else {
                if ((camZDistance + zStep) < maxZDistance)
                {
                    camZDistance += zStep * zoomSpeed;
                }
            }
            UpdateCamDistance();
        };
    }

    void FixedUpdate()
    {
        EvaluateZoomOverTime();
        EvaluateShakeCamera();
        UpdateCamDistance();
    }

    void RotateAnchor(float rotX, float rotY) {

        rotationX += rotX * rotationSpeed;
        rotationY += rotY * rotationSpeed;

        Quaternion desiredRotation = Quaternion.Euler(rotationY, rotationX, 0);
        Quaternion currentRotation = cameraAnchor.rotation;

        cameraAnchor.rotation = desiredRotation;
    }

    void MoveInwards() {

        if (camZDistance - zStep > minZDistance)
        {
            camZDistance -= zStep;
            UpdateCamDistance();
            if (DetectObstruction())
            {
                MoveInwards();
            }
        }
    }

    bool DetectObstruction()
    {
        RaycastHit hit;
        float dist = Vector3.Distance(cam.transform.position, cameraAnchor.transform.position);

        if (Physics.Raycast(cam.transform.position, cameraAnchor.TransformDirection(Vector3.forward), out hit, dist, terrainLayerMask))
        {
            Debug.DrawRay(cam.transform.position, cameraAnchor.TransformDirection(Vector3.forward), Color.yellow);

            return true;

        }
        return false;
    }

    void UpdateCamDistance()
    {
        Vector3 newPos = new Vector3(shakeX, camYDistance + shakeY, -camZDistance);
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, newPos, Time.deltaTime * 30);
    }

    private void EvaluateZoomOverTime()
    {
        if (zoomOverTime)
        {
            zoomOverTimePassed += Time.deltaTime;
            camZDistance = Mathf.Lerp(zoomInitialDistance, zoomOverTimeDistance, zoomOverTimePassed / zoomOverTimeDuration);

            if (camZDistance == zoomOverTimeDistance) {
                zoomOverTime = false;
            }
        }
    }

    private void EvaluateShakeCamera()
    {
        if (shakeCamera) {
            //move camera on x & y axis relative to anchor
            shakeMagnitudeTimer.durationPassed += Time.deltaTime;

            if (shakeMagnitudeTimer.durationPassed <= shakeMagnitudeTimer.endTime)
            {
                //move to random position
                shakeX = Mathf.Lerp(previousShakeMagnitudeX, shakeMagnitudeX, shakeMagnitudeTimer.durationPassed / shakeMagnitudeTimer.endTime);
                shakeY = Mathf.Lerp(previousShakeMagnitudeY, shakeMagnitudeY, shakeMagnitudeTimer.durationPassed / shakeMagnitudeTimer.endTime);
            }
            else {
                //re-roll magnitude & restart timer
                previousShakeMagnitudeX = shakeMagnitudeX;
                previousShakeMagnitudeY = shakeMagnitudeY;

                shakeMagnitudeTimer.durationPassed = 0f;
                shakeMagnitudeX = Random.Range(-shakeMagnitude, shakeMagnitude);
                shakeMagnitudeY = Random.Range(-shakeMagnitude, shakeMagnitude);
            }
        }
    }

    /// <summary>
    /// Zooms the camera to a set distance. Used by long distance ranged shot
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public float SetZoomOverTime(float distance, float duration)
    {
        zoomOverTimePassed = 0f;
        zoomOverTimeDistance = distance;
        zoomOverTimeDuration = duration;
        zoomInitialDistance = camZDistance;
        zoomOverTime = true;

        return camZDistance;
    }

    /// <summary>
    /// Creates a wobbling effect for the camera.
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="variance"></param>
    public void EnableShakeCamera(float duration, float magnitude, bool diminish = false)
    {
        shakeCamera = true;
        shakeMagnitude = magnitude;
        shakeMagnitudeTimer = new Timer(duration);
        shakeMagnitudeX = Random.Range(-magnitude, magnitude);
        shakeMagnitudeY = Random.Range(-magnitude, magnitude);
        previousShakeMagnitudeX = 0f;
        previousShakeMagnitudeY = 0f;
    }
    public void DisableShakeCamera()
    {
        shakeCamera = false;
        shakeX = 0f;
        shakeY = 0f;
        previousShakeMagnitudeX = 0f;
        previousShakeMagnitudeY = 0f;
    }

    public void CameraInput(InputAction.CallbackContext context)
    {
        if (!allowLook)
        {
            return;
        }

        Vector2 input = context.ReadValue<Vector2>();

        float rotX = input.x * Mathf.Deg2Rad;
        float rotY = input.y * Mathf.Deg2Rad;
        RotateAnchor(rotX, rotY);
    }

    public void InitializeCamera(float rotationX, float rotationY, float camZDistance)
    {
        //cameraAnchor.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        this.rotationX = rotationX;
        this.rotationY = rotationY;
        this.camZDistance = camZDistance;
    }
}

