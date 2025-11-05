using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyLandingState : PlayerStates
{
    public PlayerHeavyLandingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        player.knockFlip = true;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.strongGroundedEffect, player.centerEffectPos.position, Quaternion.identity);
        if (SceneScenarioSelectLv.instance != null)
            GameManager.Instance.ShowPlaceUI();
    }
    public override void Exit()
    {
        base.Exit();
        player.knockFlip = false;
    }


    public override void Update()
    {
        base.Update();
        if (finishAnim)
            stateMachine.ChangeState(player.idleState);
        if (!player.CheckSlope())
            rb.linearVelocity = Vector2.zero;
        else
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }
}
