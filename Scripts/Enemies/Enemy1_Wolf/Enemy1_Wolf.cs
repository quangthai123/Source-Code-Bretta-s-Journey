using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_Wolf : Enemy
{
    public Enemy1_WolfIdleState idleState {  get; private set; }
    public Enemy1_WolfRunState runState { get; private set; }
    public Enemy1_WolfHurtState hurtState { get; private set; }
    public Enemy1_WolfStunnedState stunnedState { get; private set; }
    public Enemy1_WolfAttackState attackState { get; private set; }
    public Enemy1_WolfDeathState deathState { get; private set; }
    public Enemy1_WolfSlideBackState slideState { get; private set; }
    [Header("Attack Force")]
    public Vector2 attackForce;
    public Vector2 jumpBackForce;
    [Header("Slide Back")]
    public float slideForceX;
    public float slideBackDuration;
    [Header("Slide back Fx")]
    public Transform spawnGroundedFxPos;
    public float spawnGroundedFxCooldown;
    protected override void Awake()
    {
        base.Awake();
        idleState = new Enemy1_WolfIdleState(this, stateMachine, "Idle", this);
        runState = new Enemy1_WolfRunState(this, stateMachine, "Run", this);
        hurtState = new Enemy1_WolfHurtState(this, stateMachine, "Hit", this);
        stunnedState = new Enemy1_WolfStunnedState(this, stateMachine, "Stunned", this);
        attackState = new Enemy1_WolfAttackState(this, stateMachine, "Attack", this);
        deathState = new Enemy1_WolfDeathState(this, stateMachine, "Death", this);
        slideState = new Enemy1_WolfSlideBackState(this, stateMachine, "Slide", this);
    }
    
    public bool CanAttackPlayer() => Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + .1f), Vector2.right * facingDir, 4.5f, opponentLayer);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y + .1f), new Vector2(transform.position.x + 6f * facingDir, transform.position.y + .1f));
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        stateMachine.Initialize(idleState);
        facingDir = -1;
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        facingDir = -1;
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void CheckDeath()
    {
        base.CheckDeath();
        if (isOver && CheckGround() && stateMachine.currentState != deathState) 
            stateMachine.ChangeState(deathState);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && transform.Find("Col Trigger").gameObject.layer == LayerMask.NameToLayer("Can Collide Player"))
        {
            if (player.stateMachine.currentState != player.dashState && player.stateMachine.currentState != player.airDashState)
            {
                isAttacking = true;
                DoDamagePlayer(0);           
            }
        }
    }
}
