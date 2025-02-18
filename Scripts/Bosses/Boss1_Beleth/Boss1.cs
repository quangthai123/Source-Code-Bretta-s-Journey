using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Boss1 : Enemy
{
    public static Boss1 Instance { get; private set; }
    public Boss1_IdleState idleState { get; private set; }
    public Boss1_WalkState walkState { get; private set; }
    public Boss1_JumpState jumpState { get; private set; }
    public Boss1_FallState fallState { get; private set; }
    public Boss1_AppearingState appearingState { get; private set; }
    public Boss1_GroundedState groundedState { get; private set; }
    public Boss1_DyingState dyingState { get; private set; }
    public Boss1_Attack1State attack1State { get; private set; }
    public Boss1_Attack2State attack2State { get; private set; }
    public float jumpForce;
    public Transform spawnGroundedFxPos;
    public Transform spawnSkillAttack2Pos;
    [Header("Combat phase")]
    public List<GameObject> boundingBossPhase;
    public bool startBoss1Phase = false;
    [Header("Attack logic")] // ko cho danh chieu 1 qua 3 lan
    public int attack1Count = 0;
    public bool canResetSkillCountForAttack2 = false;
    public bool firstAttacked = false;
    [SerializeField] private GameObject finishBossBlackUI;
    [Header("Add another attack point")]
    [SerializeField] protected Transform attackPointPosForAttack1;
    [SerializeField] protected float attackRangeRadiusForAttack1;
    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        idleState = new Boss1_IdleState(this, stateMachine, "Idle", this);
        walkState = new Boss1_WalkState(this, stateMachine, "Walk", this);
        jumpState = new Boss1_JumpState(this, stateMachine, "Jump", this);
        fallState = new Boss1_FallState(this, stateMachine, "Fall", this);
        groundedState = new Boss1_GroundedState(this, stateMachine, "Grounded", this);
        appearingState = new Boss1_AppearingState(this, stateMachine, "Appearing", this);
        dyingState = new Boss1_DyingState(this, stateMachine, "Dying", this);
        attack1State = new Boss1_Attack1State(this, stateMachine, "Attack1", this);
        attack2State = new Boss1_Attack2State(this, stateMachine, "Attack2", this);
        finishBossBlackUI.SetActive(false);
    }
    public override void DoDamagePlayer(int attackWeight)
    {
        base.DoDamagePlayer(attackWeight);
        Collider2D hit2 = Physics2D.OverlapCircle(attackPointPosForAttack1.position, attackRangeRadiusForAttack1, opponentLayer);
        if (hit2 != null)
        {
            if (hit2.GetComponentInParent<Player>() != null)
            {
                hit2.GetComponentInParent<Player>().GetDamage(transform, attackWeight, false, false);
            }
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(attackPointPosForAttack1.position, attackRangeRadiusForAttack1);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(appearingState);
        Debug.Log("Arise!");
        facingDir = -1;
    }
    protected override void CheckDeath()
    {
        base.CheckDeath();
        if (isOver && CheckGround() && stateMachine.currentState != dyingState)
            stateMachine.ChangeState(dyingState);
    }
    public void FinishBossSlowMotionFx()
    {
        Time.timeScale = .2f;
        GameManager.Instance.HideAllInGameUI();
        finishBossBlackUI.SetActive(true);
        StartCoroutine("EndSlowMotion");
    }
    private IEnumerator EndSlowMotion()
    {
        yield return new WaitForSeconds(.75f);
        Time.timeScale = 1f;
        GameManager.Instance.ShowAllInGameUI();
        finishBossBlackUI.SetActive(false);
    }
    public void RedFxOnFinishBoss()
    {
        entityFx.StartCoroutine(entityFx.RedFXForBoss());
    }
}
