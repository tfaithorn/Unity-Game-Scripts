using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This Script is to move the player.
/// Note: using AddForce is better than velocity, as velocity will prevent ANY physics calculations such as gravity
/// </summary>

public class MovementController : MonoBehaviour
{
    public bool allowMovement = true;

    private KeybindsController keybindsController;
    private PlayerCharacterController playerCharacterController;
    private Rigidbody rb;
    private Transform cameraAnchor;
    private AnimationController animationController;

    public bool isMoving = false;
    private bool isGrounded = false;
    public bool isSlowWalking = false;

    private readonly float speed = 5f;
    private readonly float runSpeed = 5f;

    private float turnSpeed = 10f;

    private Vector2 movementVector;
    float distToGround;

    InputAction movementAction;

    private void Awake()
    {
        keybindsController = GetComponent<KeybindsController>();
        playerCharacterController = GetComponent<PlayerCharacterController>();
        rb = playerCharacterController.characterModelTransform.GetComponent<Rigidbody>();
        animationController = GetComponent<AnimationController>();
        var col = playerCharacterController.characterModelTransform.GetComponent<Collider>();
        distToGround = col.bounds.extents.y;
    }

    private void Start()
    {
        cameraAnchor = playerCharacterController.camAnchor;
        movementAction = keybindsController.keybinds[KeybindsController.KeyType.MOVEMENT];
    }

    private void Update() {
        if (Physics.Raycast(playerCharacterController.characterModelTransform.position, -Vector3.up, distToGround + 0.1f))
        {
            isGrounded = true;
        }
        else {
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        Input();
        Move();
    }

    void Input() {
        movementVector = Vector2.zero;
        movementVector = movementAction.ReadValue<Vector2>();
    }

    void Move() {

        if (!allowMovement)
        {
            return;
        }

        if (!isGrounded)
        {
            PlayFallingAnimation();
            rb.drag = 1;
        }

        if (isGrounded)
        {
            rb.drag = 6;
            float totalMovementSpeed = speed;

            //if movement detected
            if (movementVector != new Vector2(0, 0))
            {
                RotateWithCamera();
                
                isMoving = true;

                if (isSlowWalking)
                {
                    PlaySlowWalkAnimation();
                    totalMovementSpeed *= 0.3f;
                }
                else {
                    PlayRunAnimation();
                }
            }
            else
            {
                isMoving = false;

                //if slow walking is enabled and there is no input. Note: This should probably be moved to playerController using flags
                if (isSlowWalking)
                {
                    PlayLegsIdleAnimation();
                }
            }

            Vector3 movement = new Vector3(movementVector.x, 0f, movementVector.y) * totalMovementSpeed;
            Vector3 cameraDirection = cameraAnchor.TransformDirection(movement);

            rb.AddForce((new Vector3(cameraDirection.x, 0f, cameraDirection.z)) * 10f);
        }
    }

    public void RotateWithCamera() 
    {
        Vector3 camAnchorRotation = cameraAnchor.rotation.eulerAngles;
        playerCharacterController.characterModelTransform.rotation = Quaternion.Slerp(playerCharacterController.characterModelTransform.rotation, Quaternion.Euler(0, camAnchorRotation.y, 0), Time.deltaTime * turnSpeed);
        
    }

    private void PlayRunAnimation()
    {
        animationController.AnimationActionRequest(
            new List<AnimationVariable> { new AnimationVariable("Action", (int)AnimationController.ActionType.RUN) }
            );
    }

    private void PlaySlowWalkAnimation()
    {
        animationController.AnimationActionRequest(
           new List<AnimationVariable> {
               new AnimationVariable("Legs Action", (int)AnimationController.ActionType.WALK)
           }, -1, AnimationController.Layer.LEGS);
    }

    private void PlayLegsIdleAnimation()
    {
        animationController.AnimationActionRequest(
           new List<AnimationVariable> {
               new AnimationVariable("Legs Action", (int)AnimationController.ActionType.IDLE)
           }, -1, AnimationController.Layer.LEGS);
    }

    private void PlayFallingAnimation()
    {
        animationController.AnimationActionRequest(
           new List<AnimationVariable> { new AnimationVariable("Action", (int)AnimationController.ActionType.FALLING) }
           );
    }
}
