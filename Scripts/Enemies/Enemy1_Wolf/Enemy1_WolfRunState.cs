using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_WolfRunState : EnemyStates
{
    private Enemy1_Wolf enemy;
    public Enemy1_WolfRunState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy1_Wolf enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Start()
    {
        base.Start();
        if(!enemy.DetectedPlayer())
        {
            float rdTime = Random.Range(enemy.actionMinTime, enemy.actionMaxTime);
            stateDuration = rdTime;
            int rdDir = Random.Range(0, 2);
            if (rdDir == 1)
                enemy.Flip();
        } else 
            enemy.anim.speed = 1.5f;

    }
    public override void Exit()
    {
        base.Exit();
        rb.linearVelocity = Vector3.zero;
        enemy.anim.speed = 1f;
    }


    public override void Update()
    {
        base.Update();
        if(enemy.DetectedPlayer()) 
        {
            FlipToFacePlayer();
            if (enemy.CanAttackPlayer())
            {
                rb.linearVelocity = Vector2.zero;
                stateMachine.ChangeState(enemy.attackState);
            }
            else
            {
                enemy.anim.speed = 1.5f;
                rb.linearVelocity = new Vector2(enemy.moveSpeed * enemy.facingDir * 1.5f, 0f);
            }
        } else
        {
            rb.linearVelocity = new Vector2(enemy.moveSpeed * enemy.facingDir, 0f);
            if (enemy.CheckNotFrontGround() || enemy.CheckWalled())
            {
                rb.linearVelocity = Vector2.zero;
                enemy.Flip();
            }
            if (stateDuration < 0f)
                stateMachine.ChangeState(enemy.idleState);
        }
        if (CheckPlayerPosThrougWallOrOtherGround())
            enemy.DecreaseDetectPlayerDistanceTemp();
    }
}
