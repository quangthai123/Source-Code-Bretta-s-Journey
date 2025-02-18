using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_IdleState : EnemyStates
{
    private Boss1 enemy;
    public Boss1_IdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Boss1 enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
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
        FlipToFacePlayer();
        rb.velocity = new Vector2(0f, rb.velocity.y);
        if (!enemy.startBoss1Phase)
            return;
        if (!enemy.firstAttacked)
        {
            enemy.firstAttacked = true;
            stateMachine.ChangeState(enemy.walkState);
        }
        else if (stateDuration <= 0 && !Player.Instance.isDead)
        {
            int rdAttack = Random.Range(0, 2);
            if (rdAttack == 1 && enemy.attack1Count >= 3)
            {
                enemy.attack1Count = 0;
                rdAttack = 0;
            }
            if (rdAttack == 0)
                stateMachine.ChangeState(enemy.jumpState);
            else
            {
                enemy.attack1Count++;
                stateMachine.ChangeState(enemy.walkState);
            }
        }

    }
}
