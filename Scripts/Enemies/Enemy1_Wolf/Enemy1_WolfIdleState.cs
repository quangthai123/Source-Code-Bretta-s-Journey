using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_WolfIdleState : EnemyStates
{
    private Enemy1_Wolf enemy;
    public Enemy1_WolfIdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy1_Wolf enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Start()
    {
        base.Start();
        if (!enemy.DetectedPlayer())
        {
            float rdTime = Random.Range(enemy.actionMinTime, enemy.actionMaxTime);
            stateDuration = rdTime;
        } else
        {
            stateDuration = enemy.attackCooldown;
        }
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0f, rb.velocity.y);
        if (stateDuration < 0f)
        {
            if(!enemy.CanAttackPlayer())
                stateMachine.ChangeState(enemy.runState);
            else
                stateMachine.ChangeState(enemy.attackState);
        }
    }
}
