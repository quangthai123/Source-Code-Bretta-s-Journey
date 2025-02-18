using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCSkeleton_AttackState : EnemyStates
{
    private EnemyChainSkeleton enemy;

    public EnemyCSkeleton_AttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyChainSkeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Start()
    {
        base.Start();
    }
    public override void Exit()
    {
        base.Exit();
        haveAttacked = true;
    }
    public override void Update()
    {
        base.Update();
        if(finishAnim)
            stateMachine.ChangeState(enemy.idleState);
    }
}
