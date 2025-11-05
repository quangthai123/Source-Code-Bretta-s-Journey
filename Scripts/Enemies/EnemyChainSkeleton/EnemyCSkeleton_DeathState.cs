using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCSkeleton_DeathState : EnemyStates
{
    private EnemyChainSkeleton enemy;
    public EnemyCSkeleton_DeathState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemyChainSkeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
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
    }
    public override void Update()
    {
        base.Update();
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        if (finishAnim)
            enemy.SetActiveFalse();
    }
}
