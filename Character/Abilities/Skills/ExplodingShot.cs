using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplodingShot : AbilityScript
{
    private MovementController movementController;
    private UIController uIController;
    private Reticle reticle;
    private Ability ability;
    private AnimationController animationController;
    private TimerManager timerManager;
    private PoolingHelper arrowPoolingHelper;
    private InventoryController inventoryController;
    private StatsController myStatsController;
    private KeybindsController keybindsController;

    private GameObject arrowObj;
    private float chargeSpeed = 0.5f;
    private Timer chargeTimer;
    private Arrow arrowComponent;
    private PlayerCharacterController playerCharacterController;
    private ProjectorHelper projectorHelper;

    private void Awake()
    {
        uIController = GetComponent<UIController>();
        reticle = uIController.reticle.GetComponent<Reticle>();
        animationController = GetComponent<AnimationController>();
        timerManager = GetComponent<TimerManager>();
        arrowPoolingHelper = gameObject.AddComponent<PoolingHelper>();
        inventoryController = GetComponent<InventoryController>();
        playerCharacterController = GetComponent<PlayerCharacterController>();
        myStatsController = GetComponent<StatsController>();
        movementController = GetComponent<MovementController>();
        keybindsController = GetComponent<KeybindsController>();
    }

    private void Start()
    {
        GameObject arrow = Resources.Load<GameObject>("Prefabs/Projectiles/Arrow");
        arrowPoolingHelper.Initialize(arrow, 20, "Exploding Arrow Pool");

        projectorHelper = gameObject.AddComponent<ProjectorHelper>();
        projectorHelper.Initialize("Exploding Shot Projector");
    }

    public override void LoadAbility(Ability ability)
    {

    }

    public override void AddAbility(Ability ability)
    {
        this.ability = ability;
    }

    public override void StartAbility()
    {
        ability.abilityScript.isInfinite = true;
        PlayRangedAnimation();
        PrepareArrow();
    }

    public override void FinishAbility()
    {
        reticle.ChangePosition(Reticle.PositionType.RESTING, 0f);
        reticle.DisableReticleCircle();

        animationController.ClearAnimationState();
        movementController.isSlowWalking = false;
    }

    public override void ResetAbility(){}

    public override void InterruptAbility(){}

    public override void PerformAbility() 
    {
        movementController.RotateWithCamera();
    }

    public override void ReleaseAbility()
    {
        ReleaseArrow();
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

    private void PrepareArrow()
    {
        arrowObj = arrowPoolingHelper.RequestItem();
        chargeTimer = new Timer(chargeSpeed);
        chargeTimer.finishedEvent += BowCharged;
        timerManager.AddTimer(chargeTimer);

        reticle.ChangePosition(Reticle.PositionType.ZOOMED, chargeSpeed);

        projectorHelper.SetImage("Projectors/Green Projector");
        projectorHelper.Show();

        keybindsController.lookEvent += PlaceProjector;

        movementController.isSlowWalking = true;

        //ignore physics with player
        Physics.IgnoreCollision(playerCharacterController.characterModelTransform.GetComponent<Collider>(), arrowObj.GetComponent<Collider>());

        //place at player's hand
        

        arrowObj.transform.position = inventoryController.weaponRBone.position;
        arrowObj.transform.rotation = inventoryController.weaponRBone.rotation;
        arrowObj.transform.parent = inventoryController.weaponRBone;
        
        //Add force to arrow in direction
        arrowObj.SetActive(true);

        Collider arrowCollider = arrowObj.GetComponent<BoxCollider>();
        float arrowColliderOffset = -arrowCollider.bounds.size.z;
        arrowObj.transform.localPosition += new Vector3(0, 0, arrowColliderOffset);


        arrowComponent = arrowObj.GetComponent<Arrow>();
        arrowComponent.ResetArrow();
        arrowComponent.ShowFireParticles();
        arrowComponent.colliderHitEvent += DetectCharacter;
    }

    private void PlaceProjector(InputAction.CallbackContext context)
    {
        Vector3 crosshairPoint = reticle.crossHairHitPoint;
        projectorHelper.Place(crosshairPoint);
    }


    private void BowCharged()
    {
        reticle.ChangeReticleAlpha(1f);
        reticle.EnableReticleCircle();
    }

    private void DetectCharacter(Transform transform)
    {
        Character hitCharacter = transform.GetComponent<Character>() ?? transform.GetComponentInParent<Character>();

        if (hitCharacter)
        {
            ApplyDamage(hitCharacter);

        }

        //regardless of whether you hit a character unsubscribe event
        arrowComponent.colliderHitEvent -= DetectCharacter;
    }

    private void ApplyDamage(Character character)
    {
        StatsController enemyStatsController = character.statsController;

        DamageRequest damageRequest = new DamageRequest();
        damageRequest.AddDamage(StatsController.DamageType.PHYSICAL, 10f);

        myStatsController.DealDamage(damageRequest, enemyStatsController);
    }

    private void ReleaseArrow()
    {
        reticle.ChangeReticleAlpha(0.5f);
        reticle.DisableReticleCircle();

        projectorHelper.Hide();
        keybindsController.lookEvent -= PlaceProjector;

        Rigidbody arrowRb = arrowObj.GetComponent<Rigidbody>();
        Arrow arrowComponent = arrowObj.GetComponent<Arrow>();

        arrowComponent.ignoreTransforms.Add(playerCharacterController.characterModelTransform);
        arrowRb.isKinematic = false;
        arrowObj.transform.parent = null;

        //attach damage event


        arrowComponent.ReleaseArrow();

        ability.abilityScript.isInfinite = false;
        Vector3 direction;

        if (reticle.targetFound)
        {
            direction = (reticle.crossHairHitPoint - arrowObj.transform.position).normalized;
        }
        else
        {
            direction = (reticle.crossHairRay.GetPoint(100000.0f) - arrowRb.transform.position).normalized;
        }

        //add force relative to time charged

        //control arrow force & damage based on how long it was held (not used yet)
        float force = Mathf.Pow(((chargeTimer.durationPassed / chargeTimer.endTime) * 2), 5);
        force = 30f;

        arrowRb.AddForce(direction * force, ForceMode.Impulse);

        // arrowPoolingHelper.ReturnToPoolAfter(arrowObj,10f);
    }


}
