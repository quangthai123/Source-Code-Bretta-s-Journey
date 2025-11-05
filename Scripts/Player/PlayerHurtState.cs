using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : PlayerStates
{
    private bool felt;
    public PlayerHurtState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        felt = false;
        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.hitImpactEffect, player.hitEffectPos.position, Quaternion.identity);
    }

    public override void Exit()
    {
        base.Exit();
        if (stateMachine.currentState != player.dashState)
            rb.linearVelocity = Vector2.zero;
        //if(stateMachine.currentState != player.deathState)
        //    player.SetKnockedFalseAfterBeHit();
        //else
            player.isKnocked = false;
    }

    public override void Update()
    {
        base.Update();
        if (rb.linearVelocity.y < -.1f)
            felt = true;
        if(!player.CheckGroundedWhileHurtOrParry())
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        if (felt && player.CheckGrounded())
        {
            rb.linearVelocity = Vector2.zero;
            if (player.isDead)
                stateMachine.ChangeState(player.deathState);
        }
        if (finishAnim && !player.isDead)
            stateMachine.ChangeState(player.idleState);
        
    }
}
