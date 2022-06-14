using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic ranged attack with a bow and arrow
/// </summary>
public class BasicRangedAttack : AbilityScript
{
    public Ability abilityRef;

    private TimerManager timerManager;
    private Reticle reticle;
    private PoolingHelper arrowPoolingHelper;
    private AnimationController animationController;
    private PlayerCharacter playerCharacter;
    private MovementController movementController;
    private UIController uIController;
    private HitColliderHelper hitColliderHelper;
    private StatsController myStatsController;
    private InventoryController inventoryController;

    private GameObject arrowObj;

    private float chargeSpeed = 0.5f;
    private Timer chargeTimer;
    private Arrow arrowComponent;

    public void Awake()
    {
        myStatsController = GetComponent<StatsController>();
        animationController = GetComponent<AnimationController>();
        inventoryController = GetComponent<InventoryController>();
        uIController = GetComponent<UIController>();
        timerManager = GetComponent<TimerManager>();
        movementController = GetComponent<MovementController>();
        playerCharacter = GetComponent<PlayerCharacter>();
        reticle = uIController.reticle.GetComponent<Reticle>();
        arrowPoolingHelper = gameObject.AddComponent<PoolingHelper>();
        hitColliderHelper = new HitColliderHelper();
    }

    private void Start()
    {
        GameObject arrow = Resources.Load<GameObject>("Prefabs/Projectiles/Arrow");
        arrowPoolingHelper.Initialize(arrow, 20, "Basic Ranged Attack Pool");
    }

    public override void LoadAbility(Ability ability) 
    {
        abilityRef = ability;
    }
    public override void StartAbility()
    {
        abilityRef.abilityScript.isInfinite = true;
        PlayRangedAnimation();
        PrepareArrow();
    }

    public override void AddAbility(Ability ability)
    {

    }

    public override void PerformAbility()
    {
        movementController.RotateWithCamera();
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
        reticle.DisableReticleCircle();

        animationController.ClearAnimationState();
        movementController.isSlowWalking = false;
    }

    private void PrepareArrow()
    {
        arrowObj = arrowPoolingHelper.RequestItem();
        chargeTimer = new Timer(chargeSpeed);
        chargeTimer.finishedEvent += BowCharged;
;
        timerManager.AddTimer(chargeTimer);

        reticle.ChangePosition(Reticle.PositionType.ZOOMED, chargeSpeed);
        movementController.isSlowWalking = true;

        //ignore physics with player
        Physics.IgnoreCollision(playerCharacter.characterModelTransform.GetComponent<Collider>(), arrowObj.GetComponent<Collider>());

        //place at player's hand
        arrowObj.transform.position = inventoryController.weaponRBone.position;
        arrowObj.transform.rotation = inventoryController.weaponRBone.rotation;
        arrowObj.transform.parent = inventoryController.weaponRBone;

        //Add force to arrow in direction
        arrowObj.SetActive(true);

        arrowComponent = arrowObj.GetComponent<Arrow>();
        arrowComponent.ResetArrow();
        arrowComponent.colliderHitEvent += DetectCharacter;
    }

    private void ReleaseArrow()
    {
        reticle.ChangeReticleAlpha(0.5f);
        reticle.DisableReticleCircle();

        Rigidbody arrowRb = arrowObj.GetComponent<Rigidbody>();
        Arrow arrowComponent = arrowObj.GetComponent<Arrow>();

        arrowComponent.ignoreTransforms.Add(playerCharacter.characterModelTransform);
        arrowRb.isKinematic = false;
        arrowObj.transform.parent = null;

        //attach damage event
        

        arrowComponent.ReleaseArrow();

        abilityRef.abilityScript.isInfinite = false;
        Vector3 direction;
        
        if (reticle.targetFound)
        {
            direction = (reticle.crossHairHitPoint - arrowObj.transform.position).normalized;
        }
        else {
            direction = (reticle.crossHairRay.GetPoint(100000.0f) - arrowRb.transform.position).normalized;
        }

        //add force relative to time charged

        //control arrow force & damage based on how long it was held (not used yet)
        float force = Mathf.Pow(((chargeTimer.durationPassed / chargeTimer.endTime) * 2),5);
        force = 30f;

        arrowRb.AddForce(direction * force, ForceMode.Impulse);

       // arrowPoolingHelper.ReturnToPoolAfter(arrowObj,10f);

    }

    private void BowCharged()
    {
        reticle.ChangeReticleAlpha(1f);
        reticle.EnableReticleCircle();
    }

    private void ApplyDamage(Character character)
    {
        StatsController enemyStatsController = character.statsController;

        DamageRequest damageRequest = new DamageRequest();
        damageRequest.AddDamage(StatsController.DamageType.PHYSICAL, 10f);

        myStatsController.DealDamage(damageRequest,enemyStatsController);

    }

    public override void InterruptAbility()
    {
        Debug.Log("Interrupt Ability called?");
        reticle.ChangePosition(Reticle.PositionType.RESTING, 0f);
        reticle.DisableReticleCircle();
        animationController.ClearAnimationState();
        movementController.isSlowWalking = false;
    }

    public override void ResetAbility()
    {

    }

    public override void ReleaseAbility() 
    {
        Debug.Log("Release Ability Called?");
        ReleaseArrow();
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
}
