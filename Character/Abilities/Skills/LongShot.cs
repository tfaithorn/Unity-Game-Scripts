using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A long ranged appear. Similar to a sniper. Movement is disabled while sniping
/// </summary>
public class LongShot : AbilityScript
{
    private TimerManager timerManager;
    private Reticle reticle;
    private PoolingHelper arrowPoolingHelper;
    private AnimationController animationController;
    private PlayerCharacter playerCharacter;
    private Transform cameraTransform;
    private Transform cameraAnchor;
    private RiggingManager riggingManager;
    private MovementController movementController;
    private Ability abilityRef;
    private UIController uIController;
    private HitColliderHelper hitColliderHelper;
    private CameraScript cameraScript;
    private InventoryController inventoryController;

    private bool pollForKeyRelease = false;
    private bool isCasting = false;
    private bool keyReleased = false;
    private GameObject arrowObj;
    private Arrow arrowComponent;
    private float previousZoomDistance;

    private float chargeSpeed = 1f;
    private Timer chargeTimer;
    private bool chargeComplete;

    public void Awake()
    {
        animationController = GetComponent<AnimationController>();
        riggingManager = GetComponent<RiggingManager>();
        inventoryController = GetComponent<InventoryController>();
        uIController = GetComponent<UIController>();
        timerManager = GetComponent<TimerManager>();
        movementController = GetComponent<MovementController>();
        playerCharacter = GetComponent<PlayerCharacter>();
        reticle = uIController.reticle.GetComponent<Reticle>();
        arrowPoolingHelper = gameObject.AddComponent<PoolingHelper>();
        cameraScript = GetComponent<CameraScript>();
        hitColliderHelper = new HitColliderHelper();
        inventoryController = GetComponent<InventoryController>();
    }

    private void Start()
    {
        GameObject arrow = Resources.Load<GameObject>("Prefabs/Projectiles/Arrow");
        cameraTransform = playerCharacter.cam;
        cameraAnchor = playerCharacter.camAnchor;
        arrowPoolingHelper.Initialize(arrow, 20, "Long Shot Pool");
    }

    private void Update()
    {
        /*
        if (pollForKeyRelease)
        {
            isCasting = false;
        }
        */
    }

    private void LateUpdate()
    {
        /*
        if (!isCasting && pollForKeyRelease)
        {
            ReleaseArrow();
            pollForKeyRelease = false;
        }
        */
    }

    public override void StartAbility()
    {
        this.allowMovement = false;
        this.isInfinite = true;
        PlayRangedAnimation();
        PrepareArrow();
        pollForKeyRelease = true;
    }

    public override void LoadAbility(Ability ability)
    { 
    
    }

    public override void AddAbility(Ability ability)
    {
        abilityRef = ability;
    }

    public override void PerformAbility()
    {
        movementController.RotateWithCamera();
        isCasting = true;

    }

    private void PlayRangedAnimation()
    {
        animationController.AnimationActionRequest(
           new List<AnimationVariable> {
               new AnimationVariable("Action", (int)AnimationController.ActionType.ABILITY),
               new AnimationVariable("Ability", 1),
               new AnimationVariable("Hands Action", (int)AnimationController.ActionType.ABILITY)
           }, -1, AnimationController.Layer.HANDS);
    }

    public override void FinishAbility()
    {
        reticle.ChangePosition(Reticle.PositionType.RESTING, 0f);
        animationController.ClearAnimationState();
        movementController.isSlowWalking = false;
        this.allowMovement = true;

        this.isInfinite = false;
        abilityRef.duration.durationPassed = abilityRef.duration.endTime;
    }

    private void PrepareArrow()
    {
        arrowObj = arrowPoolingHelper.RequestItem();

        chargeTimer = new Timer(chargeSpeed);

        chargeTimer.finishedEvent += BowCharged;

        chargeComplete = false;
        timerManager.AddTimer(chargeTimer);

        previousZoomDistance = cameraScript.SetZoomOverTime(cameraScript.zoomShootingDistance, 0.01f);
        reticle.ChangePosition(Reticle.PositionType.ZOOMED, chargeSpeed);
        cameraScript.EnableShakeCamera(2f, 0.5f);

        //ignore physics with player
        Physics.IgnoreCollision(playerCharacter.characterModelTransform.GetComponent<Collider>(), arrowObj.GetComponent<Collider>());

        //place at player's hand
        arrowObj.transform.position = inventoryController.weaponRBone.position;
        arrowObj.transform.rotation = inventoryController.weaponRBone.rotation;
        arrowObj.transform.parent = inventoryController.weaponRBone;

        //Add force to arrow in direction
        arrowObj.SetActive(true);

        arrowComponent = arrowObj.GetComponent<Arrow>();
        arrowComponent.ignoreTransforms.Add(playerCharacter.characterModelTransform);
        arrowComponent.ResetArrow();
        arrowComponent.colliderHitEvent += DetectCharacter;

    }

    private void ReleaseArrow()
    {
        RestoreReticleCircle();
        RestoreCameraPos();

        if (!chargeComplete) {
            timerManager.RemoveTimer(chargeTimer);
            arrowPoolingHelper.ReturnItem(arrowObj);
            this.isInfinite = false;
            abilityRef.duration.durationPassed = abilityRef.duration.endTime;
            Debug.Log("Premature release?");
            return;
        }

        Rigidbody arrowRb = arrowObj.GetComponent<Rigidbody>();

        arrowRb.isKinematic = false;
        arrowObj.transform.parent = null;
        arrowComponent.ReleaseArrow();
        this.isInfinite = false;
        abilityRef.duration.durationPassed = abilityRef.duration.endTime;
        Vector3 direction;

        if (reticle.targetFound)
        {
            direction = (reticle.crossHairHitPoint - arrowObj.transform.position).normalized;
        }
        else
        {
            direction = (reticle.crossHairRay.GetPoint(100000.0f) - arrowRb.transform.position).normalized;
        }

        arrowRb.AddForce(direction * 40f, ForceMode.Impulse);
    }

    private void BowCharged()
    {
        reticle.ChangeReticleAlpha(1f);
        reticle.EnableReticleCircle();
        chargeComplete = true;

    }

    public override void ResetAbility()
    {

    }

    public override void ReleaseAbility() {
        ReleaseArrow();
    }

    public override void InterruptAbility()
    {
        RestoreReticleCircle();
        RestoreCameraPos();
    }

    private void RestoreCameraPos()
    {
        cameraScript.SetZoomOverTime(previousZoomDistance, 0.01f);
        cameraScript.DisableShakeCamera();
    }

    private void RestoreReticleCircle()
    {
        reticle.ChangeReticleAlpha(0.5f);
        reticle.DisableReticleCircle();
    }

    private void DetectCharacter(Transform transform)
    {
        Character hitCharacter = transform.GetComponent<Character>() ?? transform.GetComponentInParent<Character>();

        if (hitCharacter)
        {
            Debug.Log("Character Hit");
            ApplyDamage(hitCharacter);
        }
    }

    private void ApplyDamage(Character hitCharacter)
    {
        Debug.Log("Damage Applying!");

    }
}
