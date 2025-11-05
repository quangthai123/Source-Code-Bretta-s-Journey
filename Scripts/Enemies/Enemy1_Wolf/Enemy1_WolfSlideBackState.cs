using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_WolfSlideBackState : EnemyStates
{
    private Enemy1_Wolf enemy;
    private float spawnGroundedFxTimer;
    public Enemy1_WolfSlideBackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy1_Wolf enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }
    public override void Start()
    {
        base.Start();
        enemy.Flip();
        enemy.knockFlip = true;
        rb.linearVelocity = new Vector2(-enemy.slideForceX * enemy.facingDir, rb.linearVelocity.y);
        stateDuration = enemy.slideBackDuration;
        spawnGroundedFxTimer = 0f;
    }
    public override void Exit()
    {
        base.Exit();
        enemy.knockFlip = false;
    }
    public override void Update()
    {
        base.Update();
        if (spawnGroundedFxTimer <= 0f)
        {
            EnemiesEffectSpawner.Instance.Spawn(EnemiesEffectSpawner.Instance.enemy1_groundedFx, enemy.spawnGroundedFxPos.position, enemy.facingDir == 1 ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity);
            spawnGroundedFxTimer = enemy.spawnGroundedFxCooldown;
        } else 
            spawnGroundedFxTimer -= Time.deltaTime;      
        if (stateDuration < 0f)
            stateMachine.ChangeState(enemy.idleState);
        if (enemy.CheckNotBackGround())
            rb.linearVelocity = Vector2.zero;
    }
}
