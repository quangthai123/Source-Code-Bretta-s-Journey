using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCSkeleton_WalkState : EnemyStates
{
    private EnemyChainSkeleton enemy;
    public EnemyCSkeleton_WalkState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyChainSkeleton enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }
    public override void Start()
    {
        base.Start();
        float rdTime = Random.Range(enemy.actionMinTime, enemy.actionMaxTime);
        stateDuration = rdTime;
        int rdDir = Random.Range(0, 2);
        if (rdDir == 1)
            enemy.Flip();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        rb.linearVelocity = new Vector2(enemy.moveSpeed * enemy.facingDir, 0f);
        if (enemy.DetectedPlayer())
        {
            if (enemy.DetectedPlayer() && Vector2.Distance(enemy.transform.position, player.transform.position) >= 2f)
            {
                FlipToFacePlayer();
            }
            if (enemy.CheckOpponentInAttackRange())
            {
                rb.linearVelocity = Vector2.zero;
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
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
