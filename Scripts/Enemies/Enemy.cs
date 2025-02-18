using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine { get; private set; }
    [Header("Ground Infor")]
    [SerializeField] protected Transform frontGroundCheckPos;
    [SerializeField] protected float frontGroundCheckDistance;

    [SerializeField] protected Transform backGroundCheckPos;
    [SerializeField] protected float backGroundCheckDistance;
    [Header("Detect Player Infor")]
    [SerializeField] protected float detectPlayerDistance;
    [SerializeField] protected float detectPlayerDistanceOri;
    public bool canBeStunned = false;
    public bool canBeHitByCounterAttack = false;
    public bool isAttacking = false;
    [HideInInspector] public EntityFx entityFx;
    [Header("AI Infor")]
    public float actionMinTime;
    public float actionMaxTime;
    public float attackCooldown;
    [SerializeField] private float knockbackForceX;  
    protected Player player;
    [HideInInspector] public EnemyStats enemyStats;
    protected bool isOver = false;
    [Header("Col Infor")]
    [SerializeField] protected BoxCollider2D colNoTrigger;
    [SerializeField] protected BoxCollider2D colTrigger;
    protected EnemyHpBar enemyHpBar;
    
    protected virtual void OnEnable()
    {
        enemyStats = GetComponent<EnemyStats>();
        entityFx = GetComponent<EntityFx>();
        enemyHpBar = GetComponent<EnemyHpBar>();
        enemyStats.currentHealth = enemyStats.maxHealth;
        isDead = false;
        isOver = false;
        canBeStunned = false;
        canBeHitByCounterAttack = false;
        colNoTrigger.gameObject.layer = LayerMask.NameToLayer("Enemy");
        colTrigger.enabled = true;
        detectPlayerDistance = detectPlayerDistanceOri;
    }
    protected virtual void Start()
    {
        player = Player.Instance;
    }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        isDead = enemyStats.currentHealth == 0;
        CheckDeath();
    }
    public override void Flip()
    {
        base.Flip();
        if (knockFlip) return;
        if (enemyHpBar != null)
            enemyHpBar.hpBar.transform.Rotate(0f, 180f, 0f);
    }
    protected virtual void CheckDeath()
    {
        if (isDead && !isOver)
        {
            isOver = true;
            colNoTrigger.gameObject.layer = LayerMask.NameToLayer("Default");
            colTrigger.enabled = false;
            player.playerStats.AddCurrency(enemyStats.currency);
        }
    }

    public bool CheckGround() => Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool CheckNotFrontGround() => !Physics2D.Raycast(frontGroundCheckPos.position, Vector2.down, frontGroundCheckDistance, whatIsGround);
    public bool CheckNotBackGround() => !Physics2D.Raycast(backGroundCheckPos.position, Vector2.down, backGroundCheckDistance, whatIsGround);
    public bool DetectedPlayer1() => Physics2D.Raycast(transform.position, Vector2.right * facingDir, detectPlayerDistance, opponentLayer);
    public bool DetectedPlayer2() => Physics2D.Raycast(transform.position, Vector2.right * -facingDir, detectPlayerDistance, opponentLayer);
    public bool DetectedPlayer() => (DetectedPlayer1() || DetectedPlayer2()) && !player.isDead;
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(frontGroundCheckPos.position, new Vector2(frontGroundCheckPos.position.x, frontGroundCheckPos.position.y - frontGroundCheckDistance));
        Gizmos.DrawLine(backGroundCheckPos.position, new Vector2(backGroundCheckPos.position.x, backGroundCheckPos.position.y - backGroundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + facingDir * detectPlayerDistance, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - facingDir * detectPlayerDistance, transform.position.y));
    }
    public void SetFinishAnim() => stateMachine.currentState.SetFinishAnim();
    public virtual void DoDamagePlayer(int attackWeight)
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPointPos.position, attackRangeRadius, opponentLayer);
        if (hit != null)
        {
            if (hit.GetComponentInParent<Player>() != null)
            {
                hit.GetComponentInParent<Player>().GetDamage(transform, attackWeight, false, false);
            }
        }
    }
    public void GetDamage(int attackWeight, float _damage)
    {
        if(enemyHpBar != null)
            enemyHpBar.ShowHpBarOnBeDamaged();
        enemyStats.GetDamageStat(_damage);
        if(enemyStats.currentHealth < 1)
            return;
        entityFx.StartCoroutine(entityFx.FlashFX());
    }
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void DecreaseDetectPlayerDistanceTemp()
    {
        detectPlayerDistance = 4.5f;
        CancelInvoke("SetDetectDistanceValueBack");
        Invoke("SetDetectDistanceValueBack", 10f);
    }
    private void SetDetectDistanceValueBack()
    {
        detectPlayerDistance = detectPlayerDistanceOri;
    }
}