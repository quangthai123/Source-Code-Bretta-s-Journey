using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0_DeathState : EnemyStates
{
    private Enemy0 enemy;
    public Enemy0_DeathState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy0 enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
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
        rb.linearVelocity = Vector2.zero;
        if (finishAnim)
            enemy.SetActiveFalse();
    }
}
