using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingState : PlayerStates
{
    public PlayerLandingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        player.knockFlip = true;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.strongGroundedEffect, player.centerEffectPos.position, Quaternion.identity);       
        AudioManager.instance.PlaySFX(14);
    }
    public override void Exit()
    {
        base.Exit();
        player.knockFlip = false;
        if (SceneScenarioSelectLv.instance != null)
            SceneScenarioSelectLv.instance.FinishScenarioSelectLv();
    }
    public override void Update()
    {
        base.Update();
        if (finishAnim)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (!player.CheckSlope())
            rb.linearVelocity = Vector2.zero;
        else
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }
}
