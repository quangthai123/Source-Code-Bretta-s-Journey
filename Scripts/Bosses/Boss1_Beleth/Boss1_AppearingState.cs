using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_AppearingState : EnemyStates
{
    private Boss1 enemy;
    public Boss1_AppearingState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Boss1 enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
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
        if (finishAnim)
            stateMachine.ChangeState(enemy.idleState);
    }
}
