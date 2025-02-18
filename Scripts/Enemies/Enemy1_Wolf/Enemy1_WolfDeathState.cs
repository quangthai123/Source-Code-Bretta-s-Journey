using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_WolfDeathState : EnemyStates
{
    private Enemy1_Wolf enemy;
    public Enemy1_WolfDeathState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy1_Wolf _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Start()
    {
        base.Start();
        //EnemiesManager.Instance.enemiesCount--;
    }
    public override void Exit()
    {
        base.Exit();
    }


    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0f, rb.velocity.y);
        if (finishAnim)
            enemy.SetActiveFalse();
    }
}
