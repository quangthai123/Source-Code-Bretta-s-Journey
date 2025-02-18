using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_GroundedState : EnemyStates
{
    private Boss1 enemy;
    private Transform groundedFx;
    public Boss1_GroundedState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Boss1 enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Start()
    {
        base.Start();
        groundedFx = BossEffectSpawner.Instance.Spawn(BossEffectSpawner.Instance.boss1_heavyGroundedFx, enemy.spawnGroundedFxPos.position, Quaternion.identity);
        enemy.canResetSkillCountForAttack2 = true;
        GameManager.Instance.CreateScreenShakeFx(GameManager.Instance.strongEarthQuake);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        groundedFx.position = enemy.spawnGroundedFxPos.position;
        FlipToFacePlayer();
        rb.velocity = new Vector2(0f, rb.velocity.y);
        if (finishAnim)
            stateMachine.ChangeState(enemy.attack2State);
    }
}
