using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0_WalkState : EnemyStates
{
    private Enemy0 enemy;
    public Enemy0_WalkState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy0 enemy0) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy0;
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
        rb.linearVelocity = Vector3.zero;
    }
    public override void Update()
    {
        base.Update();
        if(enemy.DetectedPlayer() && Vector2.Distance(enemy.transform.position, player.transform.position) > 2f)
        {
            FlipToFacePlayer();
        } else if(!enemy.DetectedPlayer())
            FlipOnDetectWallOrEndGround();

        if (CanConstraintMoveOnDetectWallOrEndGround() && enemy.DetectedPlayer())
        {
            rb.linearVelocity = Vector2.zero;
            if(!enemy.canDamageByAnim && !CheckPlayerPosThrougWallOrOtherGround())
                enemy.Flip();
        }
        if (CheckPlayerPosThrougWallOrOtherGround())
            enemy.DecreaseDetectPlayerDistanceTemp();

        if (stateDuration <= 0 && !enemy.DetectedPlayer())
        stateMachine.ChangeState(enemy.idleState);
    }
}
