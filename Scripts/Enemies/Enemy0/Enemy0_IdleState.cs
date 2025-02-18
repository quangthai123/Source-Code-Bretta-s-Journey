using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0_IdleState : EnemyStates
{
    private Enemy0 enemy;
    public Enemy0_IdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy0 enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }
    public override void Start()
    {
        base.Start();
        stateDuration = Random.Range(enemy.actionMinTime, enemy.actionMaxTime);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (stateDuration <= 0f || enemy.DetectedPlayer())
            stateMachine.ChangeState(enemy.walkState);
    }
}
