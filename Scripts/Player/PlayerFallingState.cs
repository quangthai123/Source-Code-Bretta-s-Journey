using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerStates
{
    public PlayerFallingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        player.isKnocked = true;
    }
    public override void Exit()
    {
        base.Exit();
        player.isKnocked = false;
    }
    public override void Update()
    {
        base.Update();
        if(player.CheckGrounded())
        {
            stateMachine.ChangeState(player.heavyLandingState);
        }
    }
}
