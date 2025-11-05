using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_WalkState : EnemyStates
{
    private Boss1 enemy;
    public Boss1_WalkState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Boss1 enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Start()
    {
        base.Start();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        FlipToFacePlayer();
        if (!enemy.CheckOpponentInAttackRange())
            rb.linearVelocity = new Vector2(enemy.moveSpeed * enemy.facingDir, 0f);
        else
            stateMachine.ChangeState(enemy.attack1State);
    }
}
