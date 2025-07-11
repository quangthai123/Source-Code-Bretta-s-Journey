using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashAttackState : PlayerStates
{
    public PlayerDashAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        //stateDuration = player.dashDuration;
        //player.StartCoroutine(player.ShadowFxOnDashAttackState());
        player.StartCoroutine(player.GetComponent<EntityFx>().QuickFlashFX()); 
        player.endDashAttack = false;
        rb.velocity = Vector2.zero;
        player.normalCol.SetActive(false);
        player.dashCol.SetActive(true);
        player.isKnocked = true;
        player.StartSpawnDashShadowFx(player.spawnDashShadowCooldown);
    }
    public override void Exit()
    {
        base.Exit();
        player.isKnocked = false;
        player.normalCol.SetActive(true);
        player.dashCol.SetActive(false);
        player.dashTimer = Time.time;
        player.StopSpawnDashShadowFx();
    }
    public override void Update()
    {
        base.Update();
        rb.sharedMaterial = player.normalPhysicMat;
        if (finishAnim)
            stateMachine.ChangeState(player.idleState);
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //if(player.CheckOpponentInAttackRange() || player.hitEnemy)
        //    rb.velocity = Vector2.zero;
        if (!player.endDashAttack && !player.CheckOpponentInAttackRange() && !player.hitEnemy)
        {
            if (player.CheckGetOutSlope() && player.facingDir == 1)
                //rb.AddForce(new Vector2(player.dashAttackSpeed * player.facingDir, 0f), ForceMode2D.Impulse);
                rb.velocity = new Vector2(player.dashSpeed * 1.1f * player.facingDir, 0f);
            else if (player.CheckSlope() && stateMachine.currentState != player.jumpState)
                //rb.AddForce(new Vector2(player.dashAttackSpeed * player.facingDir * -player.slopeMoveDir.x, player.dashAttackSpeed * player.facingDir * -player.slopeMoveDir.y), ForceMode2D.Impulse);
                rb.velocity = new Vector2(player.dashSpeed * 1.1f * player.facingDir * -player.slopeMoveDir.x, player.dashSpeed * 1.1f * player.facingDir * -player.slopeMoveDir.y);
            else
                //rb.AddForce(new Vector2(player.dashAttackSpeed * player.facingDir, rb.velocity.y), ForceMode2D.Impulse);
                rb.velocity = new Vector2(player.dashSpeed * 1.1f * player.facingDir, rb.velocity.y);
        } else
            rb.velocity = new Vector2(0f, rb.velocity.y);
    }
}
