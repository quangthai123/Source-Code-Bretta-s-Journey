using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    public static Player Instance { get; private set; }
    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerRunState runState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDoubleJumpState doubleJumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerShieldState shieldState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerAirDashState airDashState { get; private set; }
    public PlayerHealState healState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerCrouchState crouchState { get; private set; }
    public PlayerEnterCrouchState enterCrouchState { get; private set; }
    public PlayerExitCrouchState exitCrouchState { get; private set; }
    public PlayerLandingState landingState { get; private set; }
    public PlayerLadderState ladderState { get; private set; }
    public PlayerLedgeGrabState ledgeGrabState { get; private set; }
    public PlayerLedgeClimbState ledgeClimbState { get; private set; }
    public PlayerHurtState hurtState { get; private set; }
    public PlayerStrongHurtState strongHurtState { get; private set; }
    public PlayerKnockoutState knockoutState { get; private set; }
    public PlayerParryState parryState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerDeathState deathState { get; private set; }
    public PlayerReviveState reviveState { get; private set; }
    public PlayerMagicState magicState { get; private set; }
    public PlayerPickupState pickupState { get; private set; }
    public PlayerFallingState fallingState { get; private set; }
    public PlayerHeavyLandingState heavyLandingState { get; private set; }
    public PlayerEndDashState endDashState { get; private set; }
    public PlayerEnterRunState enterRunState { get; private set; }
    public PlayerBreakRunState breakRunState { get; private set; }
    public PlayerLightGroundedState lightGroundedState { get; private set; }
    public PlayerExitParryState exitParryState { get; private set; }
    public PlayerLadderOutState ladderOutState { get; private set; }    
    public PlayerLaddderInState laddderInState { get; private set; }
    public PlayerToLedgeGrabState toLedgeGrabState { get; private set; }
    public PlayerChargingState chargingState { get; private set; }
    public PlayerChargedAttackState chargedAttackState { get; private set; }
    public PlayerDashAttackState dashAttackState { get; private set; }
    #endregion
    [SerializeField] protected Transform groundCheckPos1;
    [SerializeField] protected Transform groundCheckPos2;
    [Header("Dash Infor")]
    public float dashSpeed;
    public float dashDuration; // original 0.3f // update 0.45f
    public float dashCooldown; // original 0.8f // update 0.4f
    public bool endDashAttack = false;
    [Header("Shield Infor")]
    public float shieldDuration; // original 0.2f // update 0.5f
    public bool isShielding = false;
    [Header("Heal Infor")]
    public float healingDuration; // original 1.8f // update 0.9f
    public bool cantBeHurtWhileHealing = false;
    [Space]
    [Header("Jump Infor")]
    public float jumpDuration;
    public float jumpForce;
    public float jumpMinTime;
    [Header("Wall Jump Infor")]
    public Vector2 wallJumpForce;
    public GameObject wallSlideCol;
    public float allowWallJumpUpMaxTime;
    public float allowWallJumpUpMinTime;
    [HideInInspector] public bool doubleJumped;
    [Header("Attack Infor")]
    //[SerializeField] Transform attackPointPosForCombo4;
    [HideInInspector] public bool hitEnemy;
    [HideInInspector] public bool canHaveNextCombo = false;
    [HideInInspector] public bool gotNextCombo = false;
    [SerializeField] private float combo1_2AttackPosX;
    [SerializeField] private float combo3_4AttackPosX;
    [SerializeField] private float combo1_2AttackRadius;
    [SerializeField] private float combo3_4AttackRadius;
    [SerializeField] private float chargedAttackPosX;
    [SerializeField] private float chargedAttackRadius;
    public float allowComboTime;
    [SerializeField] private float spawnDashShadowCooldown;
    [SerializeField] private bool startDashShadowCoroutine = false;
    //public Transform attack1FxPos; // New Game: 2f // plus 0.15f with x Axis after per level up
    //public Transform attack2FxPos; // 2.3f
    //public Transform attack3FxPos; // .23f
    //public Transform attack4FxPos_Left; // 2.28f
    public Transform attack4FxPos; // -2.28f
    [Header("Ceilling Collision Infor")]
    [SerializeField] private Transform ceillingCheckPos1;
    [SerializeField] private Transform ceillingCheckPos2;
    [SerializeField] private float ceillingCheckDistance;
    [HideInInspector] public GameObject normalCol;
    [HideInInspector] public GameObject dashCol;
    [HideInInspector] public float dashTimer;
    [Header("Landing Infor")]
    public float landingCheckDistance;
    [Header("Slope infor")]
    public PhysicsMaterial2D normalPhysicMat;
    public PhysicsMaterial2D onSlopePhysicMat;
    public bool onSlope = false;
    public Vector2 slopeMoveDir;
    [SerializeField] private float slopeAngleWithUpAxis;
    [SerializeField] private LayerMask whatIsSlopeGround;
    [SerializeField] private float slopeCheckDistance;
    private Vector2 slopeNormalAxis;
    [Header("Ladder Infor")]
    public bool canLadder = false;
    public float ladderMoveSpeed;
    public float offSetYOnLadderIn;
    [Header("Ledge Infor")]
    public Transform ledgeCheckPos;
    public Transform wallCheckPosForEdge;
    [SerializeField] private float ledgeCheckRadius;
    public bool canGrabLedge = true;
    private Coroutine dashShadowCoroutine;
    [Header("Effect Infor")]
    public Transform leftEffectPos;
    public Transform rightEffectPos;
    public Transform centerEffectPos;
    public Transform shieldEffectPos;
    public Transform attackEffectPos;
    public Transform hitEffectPos;
    [Header("Knockback")]
    public Vector2 currentKnockbackDir;
    [SerializeField] protected float currentKnockbackDuration;
    [SerializeField] protected List<Vector2> knockbackDirs;
    [SerializeField] protected List<float> knockbackDurations;
    public bool isKnocked;
    public bool isCooldownHurt;
    public float knockOutDuration;
    private EntityFx entityFx;

    [Header("Parry Infor")]
    public float parryDuration;
    public float strongParryDuration;
    public float pushBackSpeed;
    [HideInInspector] public bool isStrongStrike;
    public float spawnParryEffectCooldown;
    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public PlayerStatsWithItems playerStatsWithItems;
    public Transform playerSoul;
    public Transform playerFakeSoul;

    [HideInInspector] public bool canPickup = false;
    [HideInInspector] public float currentItemPosX;
    public bool pickupedItem = false;
    public Vector2 offSetClimbedUp;

    public bool checkStandOnLedge = false;
    public Vector2 spawnSoulPos;
    protected override void Awake()
    {
        //Time.timeScale = .1f;
        base.Awake();
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        runState = new PlayerRunState(this, stateMachine, "Run");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        fallState = new PlayerFallState(this, stateMachine, "Fall");
        shieldState = new PlayerShieldState(this, stateMachine, "Shield");
        attackState = new PlayerAttackState(this, stateMachine, "IsAttacking");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        endDashState = new PlayerEndDashState(this, stateMachine, "EndDash");
        airDashState = new PlayerAirDashState(this, stateMachine, "AirDash");
        healState = new PlayerHealState(this, stateMachine, "Heal");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "Walled");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        crouchState = new PlayerCrouchState(this, stateMachine, "Crouch");
        enterCrouchState = new PlayerEnterCrouchState(this, stateMachine, "EnterCrouch");
        exitCrouchState = new PlayerExitCrouchState(this, stateMachine, "ExitCrouch");
        landingState = new PlayerLandingState(this, stateMachine, "Landing");
        ladderState = new PlayerLadderState(this, stateMachine, "Ladder");
        ledgeGrabState = new PlayerLedgeGrabState(this, stateMachine, "LedgeGrab");
        ledgeClimbState = new PlayerLedgeClimbState(this, stateMachine, "LedgeClimb");
        hurtState = new PlayerHurtState(this, stateMachine, "Hurt");
        strongHurtState = new PlayerStrongHurtState(this, stateMachine, "StrongHurt");
        knockoutState = new PlayerKnockoutState(this, stateMachine, "Knockout");
        parryState = new PlayerParryState(this, stateMachine, "Parry");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        deathState = new PlayerDeathState(this, stateMachine, "Dead");
        reviveState = new PlayerReviveState(this, stateMachine, "Revive");
        magicState = new PlayerMagicState(this, stateMachine, "Magic");
        doubleJumpState = new PlayerDoubleJumpState(this, stateMachine, "DoubleJump");
        pickupState = new PlayerPickupState(this, stateMachine, "Pickup");
        fallingState = new PlayerFallingState(this, stateMachine, "Falling");
        heavyLandingState = new PlayerHeavyLandingState(this, stateMachine, "HeavyLanding");

        enterRunState = new PlayerEnterRunState(this, stateMachine, "EnterRun");
        breakRunState = new PlayerBreakRunState(this, stateMachine, "BreakRun");
        lightGroundedState = new PlayerLightGroundedState(this, stateMachine, "LightGrounded");
        exitParryState = new PlayerExitParryState(this, stateMachine, "ExitParry");
        ladderOutState = new PlayerLadderOutState(this, stateMachine, "LadderOut");
        laddderInState = new PlayerLaddderInState(this, stateMachine, "LadderIn");
        toLedgeGrabState = new PlayerToLedgeGrabState(this, stateMachine, "ToLedgeGrab");
        chargingState = new PlayerChargingState(this, stateMachine, "Charged");
        chargedAttackState = new PlayerChargedAttackState(this, stateMachine, "ChargedAttack");
        dashAttackState = new PlayerDashAttackState(this, stateMachine, "DashAttack");

        playerStats = GetComponent<PlayerStats>();
        playerStatsWithItems = GetComponent<PlayerStatsWithItems>();
    }

    protected void Start()
    {
        stateMachine.Initialize(idleState);
        dashCol = transform.Find("Dash Col No Trigger").gameObject;
        normalCol = transform.Find("Col No Trigger").gameObject;
        entityFx = GetComponent<EntityFx>();
    }
    public void ChangeAttackRangeOnCombo1_2()
    {
        attackPointPos.localPosition = new Vector2(combo1_2AttackPosX, 0f);
        attackRangeRadius = combo1_2AttackRadius;
    }
    public void ChangeAttackRangeOnCombo3_4()
    {
        attackPointPos.localPosition = new Vector2(combo3_4AttackPosX, 0f);
        attackRangeRadius = combo3_4AttackRadius;
    }
    public void ChangeAttackRangeOnChargedAttack()
    {
        attackPointPos.localPosition = new Vector2(chargedAttackPosX, .85f);
        attackRangeRadius = chargedAttackRadius;
    }
    public void ChangeAttackRangeOnDashAttack()
    {
        attackPointPos.localPosition = new Vector2(2.4f, 0f);
        attackRangeRadius = 1.4f;
    }
    protected override void Update()
    {
        base.Update();
        checkStandOnLedge = CheckStandOnLedge();

        //if (Input.GetKeyDown(KeyCode.L))
        //    playerStats.GetDamageStat(1000);

        stateMachine.currentState.Update();
        HandleSlopeMoveDir();
        onSlope = CheckSlope();
        if (stateMachine.currentState != reviveState)
            isDead = playerStats.currentHealth == 0;
    }
    public void StartSpawnDashShadowFx()
    {
        startDashShadowCoroutine = true;
        dashShadowCoroutine = StartCoroutine(SpawnDashShadow());
    }
    public void StopSpawnDashShadowFx()
    {
        startDashShadowCoroutine = false;
        StopCoroutine(dashShadowCoroutine);
    }
    private void FixedUpdate()
    {
        stateMachine.currentState.FixedUpdate();
    }
    private void LateUpdate()
    {
        stateMachine.currentState.LateUpdate();
    }
    protected override void FlipController()
    {
        //if (isKnocked && stateMachine.currentState != enterRunState && stateMachine.currentState != runState) // for run after be hit
        //    return;
        if (isKnocked) // for run after be hit
            return;
        if (!CheckGrounded())
            base.FlipController();
        else
        {
            if (stateMachine.currentState == landingState)
                return;
            if (InputManager.Instance.moveDir.x == 0)
            {
                if (Input.GetAxisRaw("Horizontal") < 0 && facingDir == 1)
                    Flip();
                if (Input.GetAxisRaw("Horizontal") > 0 && facingDir == -1)
                    Flip();
            }
            else
            {
                if (InputManager.Instance.moveDir.x < 0 && facingDir == 1)
                    Flip();
                if (InputManager.Instance.moveDir.x > 0 && facingDir == -1)
                    Flip();
            }
        }

    }
    private IEnumerator SpawnDashShadow()
    {
        while (startDashShadowCoroutine)
        {
            PlayerEffectSpawner.instance.Spawn("dashShadowFx", transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnDashShadowCooldown);
        }
    }
    //public bool CheckLedge() => Physics2D.OverlapCircle(ledgeCheckPos.position, ledgeCheckRadius, whatIsGround) && !CheckLedgeGround();
    public bool CheckLedge()
    {
        if (!canGrabLedge || CheckLedgeGround())
            return false;
        Collider2D ledge = Physics2D.OverlapCircle(ledgeCheckPos.position, ledgeCheckRadius, whatIsGround);
        if(ledge != null)
        {
            if(ledge.GetComponent<LedgeToGrab>() != null)
            {
                Vector2 ledgePos = ledge.transform.position;
                if (facingDir == 1)
                    transform.position = new Vector2(ledgePos.x - ledge.GetComponent<LedgeToGrab>().offSetForGrabPos.x - .6f,
                        ledgePos.y + ledge.GetComponent<LedgeToGrab>().offSetForGrabPos.y - .6f);
                else
                    transform.position = new Vector2(ledgePos.x + ledge.GetComponent<LedgeToGrab>().offSetForGrabPos.x + .6f,
                        ledgePos.y + ledge.GetComponent<LedgeToGrab>().offSetForGrabPos.y - .6f);
                return true;
            }
        }
        return false;
    }
    public bool CheckLedgeGround() => Physics2D.Raycast(wallCheckPosForEdge.position, Vector2.right * facingDir, wallCheckDistance + 1f, whatIsGround);
    public bool CheckCeilling()
    {
        return Physics2D.Raycast(ceillingCheckPos1.position, Vector2.up, ceillingCheckDistance, whatIsGround) ||
            Physics2D.Raycast(ceillingCheckPos2.position, Vector2.up, ceillingCheckDistance, whatIsGround);
    }
    public bool CheckSlope()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, slopeCheckDistance, whatIsSlopeGround) && slopeAngleWithUpAxis < 30f;
    }
    public bool CheckGrounded()
    {
        return Physics2D.Raycast(groundCheckPos1.position, Vector2.down, groundCheckDistance, whatIsGround)
            || Physics2D.Raycast(groundCheckPos2.position, Vector2.down, groundCheckDistance, whatIsGround) || CheckSlope();
    }
    public bool CheckJumpOnSlope() => Physics2D.Raycast(groundCheckPos1.position, Vector2.down, groundCheckDistance, whatIsSlopeGround)
            || Physics2D.Raycast(groundCheckPos2.position, Vector2.down, groundCheckDistance, whatIsSlopeGround);
    public bool CheckGroundedWhileHurtOrParry() => Physics2D.Raycast(new Vector2(groundCheckPos1.position.x - facingDir * .5f, groundCheckPos1.position.y), Vector2.down, slopeCheckDistance, whatIsGround);
    public bool CheckStandOnLedge() => ((Physics2D.Raycast(groundCheckPos1.position, Vector2.down, groundCheckDistance + 1f, whatIsGround)
            && !Physics2D.Raycast(groundCheckPos2.position, Vector2.down, groundCheckDistance + 1f, whatIsGround)) || (!Physics2D.Raycast(groundCheckPos1.position, Vector2.down, groundCheckDistance + 1f, whatIsGround)
            && Physics2D.Raycast(groundCheckPos2.position, Vector2.down, groundCheckDistance + 1f, whatIsGround))) && !CheckSlope() && !Physics2D.Raycast(groundCheckPos1.position, Vector2.down, groundCheckDistance + 1f, whatIsSlopeGround)
        && !Physics2D.Raycast(groundCheckPos2.position, Vector2.down, groundCheckDistance + 1f, whatIsSlopeGround);
    public bool CheckGetOutSlope() => (Physics2D.Raycast(groundCheckPos1.position - new Vector3(.2f * facingDir, 0f, 0f), Vector2.down, groundCheckDistance + 1f, whatIsSlopeGround)
        && !Physics2D.Raycast(groundCheckPos2.position + new Vector3(.2f * facingDir, 0f, 0f), Vector2.down, groundCheckDistance + 1f, whatIsSlopeGround) || (!Physics2D.Raycast(groundCheckPos1.position - new Vector3(.2f * facingDir, 0f, 0f), Vector2.down, groundCheckDistance + 1f, whatIsSlopeGround) &&
        Physics2D.Raycast(groundCheckPos2.position + new Vector3(.2f * facingDir, 0f, 0f), Vector2.down, groundCheckDistance + 1f, whatIsSlopeGround)));
    private void HandleSlopeMoveDir()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, slopeCheckDistance, whatIsSlopeGround);
        if (hit)
        {
            slopeAngleWithUpAxis = Vector2.Angle(Vector2.up, hit.normal);
            Debug.DrawRay(hit.point, hit.normal, Color.red);
            Debug.DrawRay(hit.point, Vector2.Perpendicular(hit.normal), Color.green);
            slopeMoveDir = Vector2.Perpendicular(hit.normal).normalized;
        }
    }
    public void SetFinishCurrentAnimation()
    {
        stateMachine.currentState.SetFinishAnimation();
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(ceillingCheckPos1.position, new Vector2(ceillingCheckPos1.position.x, ceillingCheckPos1.position.y + ceillingCheckDistance));
        Gizmos.DrawLine(ceillingCheckPos2.position, new Vector2(ceillingCheckPos2.position.x, ceillingCheckPos2.position.y + ceillingCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - slopeCheckDistance));
        Gizmos.DrawWireSphere(ledgeCheckPos.position, ledgeCheckRadius);
        Gizmos.DrawLine(wallCheckPosForEdge.position, new Vector2(wallCheckPosForEdge.position.x + facingDir * (wallCheckDistance + 1f), wallCheckPosForEdge.position.y));
        Gizmos.DrawLine(groundCheckPos1.position, new Vector2(groundCheckPos1.position.x, groundCheckPos1.position.y - groundCheckDistance));
        Gizmos.DrawLine(groundCheckPos2.position, new Vector2(groundCheckPos2.position.x, groundCheckPos2.position.y - groundCheckDistance));
        Gizmos.DrawLine(centerEffectPos.position, new Vector2(centerEffectPos.position.x, centerEffectPos.position.y - landingCheckDistance));
    }
    public void DoDamageEnemy(int attackWeight, float _damage)
    {
        bool hitSomething = false;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPointPos.position, attackRangeRadius, opponentLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponentInParent<Enemy>() != null)
            {
                SpawnAttackImpactFx(hit, attackWeight);
                hitSomething = true;
                hitEnemy = true;
                playerStats.GetManaByAttack();
                hit.GetComponentInParent<Enemy>().GetDamage(attackWeight, _damage);
            }
            else if (hit.GetComponentInParent<PlayerSoulController>() != null)
            {
                SpawnAttackImpactFx(hit, attackWeight);
                Debug.Log("Attack soul");
                hitSomething = true;
                hit.GetComponentInParent<PlayerSoulController>().GetDamage(transform, attackWeight, _damage);
            }
        }
        Collider2D[] hitBreakableObjects = Physics2D.OverlapCircleAll(attackPointPos.position, attackRangeRadius, LayerMask.GetMask("BreakbleObjects"));
        foreach (Collider2D hit in hitBreakableObjects)
        {
            if (hit.GetComponent<BreakableObjects>() != null)
            {
                SpawnAttackImpactFx(hit, attackWeight);
                hitSomething = true;
                hit.GetComponent<BreakableObjects>().BeAttackByPlayer();
            }
        }
        //AttackOtherSideByCombo4(attackWeight, _damage, ref hitSomething);
        CreateShakeFxByAttack(attackWeight, hitSomething);
    }

    private void SpawnAttackImpactFx(Collider2D hit, int attackWeight)
    {
        Vector2 closestPoint = hit.ClosestPoint(attackPointPos.position);
        Vector2 spawnFxPos = new Vector2(closestPoint.x + facingDir * .75f, closestPoint.y);
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        switch (attackWeight)
        {
            case 1:
                PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attackImpactFx, spawnFxPos, Quaternion.Euler(0f, 0f, 50f));
                PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attackImpactFx2, spawnFxPos, randomRotation);
                break;
            case 2:
                PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attackImpactFx, spawnFxPos, Quaternion.identity);
                PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attackImpactFx2, spawnFxPos, Quaternion.identity);
                break;
            default:
                PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attackImpactFx, spawnFxPos, Quaternion.Euler(0f, 0f, -50f));
                PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attackImpactFx2, spawnFxPos, randomRotation);
                break;
        }
    }
    private static void CreateShakeFxByAttack(int attackWeight, bool hitSomething)
    {
        if (hitSomething)
        {
            switch (attackWeight)
            {
                case 0:
                    float rdX = Random.Range(-.1f, .1f);
                    float rdY = Random.Range(-.1f, .1f);
                    GameManager.Instance.CreateScreenShakeFx(new Vector2(rdX, rdY)); break;
                case 1:
                    float rdX1 = Random.Range(-.2f, .2f);
                    float rdY1 = Random.Range(-.2f, .2f);
                    GameManager.Instance.CreateScreenShakeFx(new Vector2(rdX1, rdY1)); break;
                case 2:
                    float rdX2 = Random.Range(-.4f, .4f);
                    float rdY2 = Random.Range(-.4f, .4f);
                    GameManager.Instance.CreateScreenShakeFx(new Vector2(rdX2, rdY2)); break;
                case 3:
                case 4:
                case 5:
                    float rdX3 = Random.Range(-.5f, .5f);
                    float rdY3 = Random.Range(-.5f, .5f);
                    GameManager.Instance.CreateScreenShakeFx(new Vector2(rdX3, rdY3)); break;
            }

        }
    }

    public virtual void GetDamage(Transform opponentTransform, int attackWeight, bool isImpactDamage, bool isDeathDamage)
    {
        if ((isKnocked || cantBeHurtWhileHealing || isCooldownHurt) && !isDeathDamage)
            return;
        isKnocked = true;
        if (!isImpactDamage && isShielding && ((opponentTransform.position.x < transform.position.x && facingDir == -1) ||
            (opponentTransform.position.x > transform.position.x && facingDir == 1)) && opponentTransform.GetComponentInParent<Enemy>() != null)
        {
            if (attackWeight == 0 && opponentTransform.GetComponentInParent<Enemy>().isAttacking)
            {
                stateMachine.ChangeState(counterAttackState);
                opponentTransform.GetComponentInParent<Enemy>().canBeStunned = true;
                anim.GetComponent<PlayerAnimationController>().currentEnemyTarget = opponentTransform;
                if (facingDir == 1)
                    GameManager.Instance.CreateScreenShakeFx(new Vector2(-1f, 0f));
                else
                    GameManager.Instance.CreateScreenShakeFx(new Vector2(1f, 0f));
                return;
            }
            entityFx.StartCoroutine("FlashFX");
            if (attackWeight == 1)
            {
                currentKnockbackDir = knockbackDirs[3];
                currentKnockbackDuration = knockbackDurations[3];
                isStrongStrike = false;
            }
            else if (attackWeight == 2)
            {
                currentKnockbackDir = knockbackDirs[4];
                currentKnockbackDuration = knockbackDurations[4];
                isStrongStrike = true;
            }
            if (CheckGroundedWhileHurtOrParry())
                rb.velocity = new Vector2(currentKnockbackDir.x * -facingDir, currentKnockbackDir.y);
            stateMachine.ChangeState(parryState);
            return;
        }
        entityFx.StartCoroutine("FlashFX");
        HitKnockback(opponentTransform, attackWeight);
        if (opponentTransform.GetComponentInParent<Enemy>() != null)
        {
            if (!isImpactDamage)
                playerStats.GetDamageStat(opponentTransform.GetComponentInParent<Enemy>().enemyStats.damage);
            else
                playerStats.GetDamageStat(opponentTransform.GetComponentInParent<Enemy>().enemyStats.impactDamage);
        }
        else if (opponentTransform.GetComponent<PlayerSoulController>() != null)
        {
            playerStats.GetDamageStat(opponentTransform.GetComponent<PlayerSoulController>().damage);
        }
        else if (opponentTransform.GetComponent<CanDamagePlayer>() != null)
            playerStats.GetDamageStat(opponentTransform.GetComponent<CanDamagePlayer>().damage);

        if (!CheckGrounded() && attackWeight == 0)
        {
            PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.hitImpactEffect, transform.position, Quaternion.identity);
            isKnocked = false;
            isCooldownHurt = true;
            CancelInvoke("EndCooldownHurt");
            Invoke("EndCooldownHurt", 1f);
            return;
        }
        if (attackWeight == 0)
            stateMachine.ChangeState(hurtState);
        else
            stateMachine.ChangeState(strongHurtState);
    }
    private void EndCooldownHurt() => isCooldownHurt = false;
    protected virtual void HitKnockback(Transform opponentTransform, int attackWeight)
    {
        if ((opponentTransform.position.x > transform.position.x && facingDir == -1) || (opponentTransform.position.x < transform.position.x && facingDir == 1))
            Flip();
        StartCoroutine(Knockback(attackWeight));
    }
    protected virtual IEnumerator Knockback(int attackWeight)
    {
        if (attackWeight == 0)
        {
            if (!CheckGrounded())
                yield break;
            currentKnockbackDir = knockbackDirs[0];
            currentKnockbackDuration = knockbackDurations[0];
        }
        else if (attackWeight == 1)
        {
            currentKnockbackDir = knockbackDirs[1];
            currentKnockbackDuration = knockbackDurations[1];
        }
        else
        {
            currentKnockbackDir = knockbackDirs[2];
            currentKnockbackDuration = knockbackDurations[2];
        }
        if (attackWeight == 0 && !CheckGroundedWhileHurtOrParry())
            rb.velocity = new Vector2(0f, currentKnockbackDir.y);
        else
            rb.velocity = new Vector2(currentKnockbackDir.x * -facingDir, currentKnockbackDir.y);
        yield return new WaitForSeconds(currentKnockbackDuration);
        if (attackWeight == 0)
            rb.velocity = new Vector2(rb.velocity.x, 0f);
    }
    public void SpawnPlayerSoulAfterDead()
    {
        Debug.Log("Spawn Right Soul");
        Instantiate(playerSoul, spawnSoulPos, Quaternion.identity);
        //tempGameData.soulScene = tempGameData.tempCurrentScene;
        //tempGameData.soulPos = new Vector2(transform.position.x - facingDir, transform.position.y);
    }
    public void SpawnPlayerFakeSoul()
    {
        Debug.Log("Spawn Fake Soul");
        Instantiate(playerFakeSoul, spawnSoulPos, Quaternion.identity);
    }
    public void ReviveState()
    {
        stateMachine.ChangeState(reviveState);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            canLadder = true;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("CanDamagePlayer"))
        {
            if (stateMachine.currentState != dashState && stateMachine.currentState != airDashState)
            {
                GetDamage(collision.transform, 0, true, false); //Contact Damage
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            canLadder = true;
        }
    }
    public void SetKnockedFalseAfterBeHit()
    {
        CancelInvoke("SetKnockedFalse");
        Invoke("SetKnockedFalse", .3f);
    }
    private void SetKnockedFalse() => isKnocked = false;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            canLadder = false;
        }
    }
    public void RedFxOnFinishBoss()
    {
        entityFx.StartCoroutine(entityFx.RedFXForBoss());
    }
    public IEnumerator ShadowFxOnDashAttackState() 
    {
        SpriteRenderer playerSprite = transform.Find("Model").GetComponent<SpriteRenderer>();
        Color oriColor = playerSprite.color;
        playerSprite.color = new Color(210f/255f, 140f / 255f, 240f / 255f);
        yield return new WaitForSecondsRealtime(1f);
        playerSprite.color = oriColor;
    }
}