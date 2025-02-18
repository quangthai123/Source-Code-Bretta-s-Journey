using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_JumpState : EnemyStates
{
    private Boss1 enemy;
    public Boss1_JumpState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Boss1 enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Start()
    {
        base.Start();
        //if (player.rb.velocity.x != 0)
        //{
        //    rb.velocity = new Vector2(player.transform.position.x - enemyBase.transform.position.x + 7f * player.facingDir, enemy.jumpForce);
        //}
        //else
        if(player.transform.position.x < enemyBase.transform.position.x)
        {
            if(Vector2.Distance(player.transform.position, enemy.transform.position) > 7.5f)
                rb.velocity = new Vector2(-(-player.transform.position.x + enemyBase.transform.position.x + 7.5f), enemy.jumpForce);
            else
                rb.velocity = new Vector2(-(-player.transform.position.x + enemyBase.transform.position.x), enemy.jumpForce);
        }
        else
        {
            if (Vector2.Distance(player.transform.position, enemy.transform.position) > 7.5f)
                rb.velocity = new Vector2(player.transform.position.x - enemyBase.transform.position.x + 7.5f, enemy.jumpForce);
            else
                rb.velocity = new Vector2(player.transform.position.x - enemyBase.transform.position.x, enemy.jumpForce);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        FlipToFacePlayer();
        if (rb.velocity.y < -.1f)
        {
            stateMachine.ChangeState(enemy.fallState);
        }
    }
}
