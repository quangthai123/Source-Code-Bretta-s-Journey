using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_WolfAttackState : EnemyStates
{
    private Enemy1_Wolf enemy;
    private bool attacked = false;
    public Enemy1_WolfAttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy1_Wolf _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
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
        rb.linearVelocity = Vector3.zero;
        enemy.isAttacking = false;
        enemy.transform.Find("Col Trigger").gameObject.layer = LayerMask.NameToLayer("CanDamagePlayer");
        attacked = false;
    }
    public override void Update()
    {
        base.Update();
        if (rb.linearVelocity.y < -.1f)
            attacked = true;
        if(enemy.CheckNotFrontGround() && !enemy.CheckGround())
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            if(stateMachine.currentState != enemy.slideState)
            {
                enemy.Flip();
                enemy.rb.linearVelocity = new Vector2(enemy.jumpBackForce.x * enemy.facingDir, enemy.jumpBackForce.y);
                Debug.Log("bounce!!!!");
            }
        } else if (enemy.CheckGround() && attacked)
        {
            Debug.Log("Stop Attack");
            stateMachine.ChangeState(enemy.slideState);
        }   
        if (enemy.canBeStunned)
            stateMachine.ChangeState(enemy.stunnedState);
    }
}
