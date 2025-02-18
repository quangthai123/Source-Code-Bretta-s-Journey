using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChainSkeleton : Enemy
{
    public EnemyCSkeleton_IdleState idleState { get; private set; }
    public EnemyCSkeleton_WalkState walkState { get; private set; }
    public EnemyCSkeleton_AttackState attackState { get; private set; }
    public EnemyCSkeleton_DeathState deathState { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        idleState = new EnemyCSkeleton_IdleState(this, stateMachine, "Idle", this);
        walkState = new EnemyCSkeleton_WalkState(this, stateMachine, "Walk", this);
        attackState = new EnemyCSkeleton_AttackState(this, stateMachine, "Attack", this);
        deathState = new EnemyCSkeleton_DeathState(this, stateMachine, "Death", this);
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
    protected override void CheckDeath()
    {
        base.CheckDeath();
        if (isOver && CheckGround() && stateMachine.currentState != deathState)
            stateMachine.ChangeState(deathState);
    }
}
