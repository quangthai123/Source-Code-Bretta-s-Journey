using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCSkeleton_IdleState : EnemyStates
{
    private EnemyChainSkeleton enemy;
    public EnemyCSkeleton_IdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyChainSkeleton enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }
    public override void Start()
    {
        base.Start();
        if(haveAttacked)
        {
            stateDuration = enemy.attackCooldown;
            Debug.Log("Het attack cooldown");
        }
        else
            stateDuration = Random.Range(enemy.actionMinTime, enemy.actionMaxTime);
    }
    public override void Exit()
    {
        base.Exit();
        haveAttacked = false;
    }
    public override void Update()
    {
        base.Update();
        rb.velocity = Vector2.zero;
        if (enemy.DetectedPlayer() && Vector2.Distance(enemy.transform.position, player.transform.position) < 2f)
        {
            stateMachine.ChangeState(enemy.walkState);
        }
        if (stateDuration <= 0f) 
        {
            if(enemy.CheckOpponentInAttackRange())        
                stateMachine.ChangeState(enemy.attackState);
            else 
                stateMachine.ChangeState(enemy.walkState);
        }
    }
}
