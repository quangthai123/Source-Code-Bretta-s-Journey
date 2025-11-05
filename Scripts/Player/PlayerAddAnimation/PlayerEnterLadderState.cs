using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterLadderState : PlayerStates
{
    public PlayerEnterLadderState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        player.FlipWithPosX(player.LadderPosX);
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        player.knockFlip = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(stateMachine.currentState == player.enterLadderState)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
        }
        if(finishAnim)
            stateMachine.ChangeState(player.ladderState);
        //if (!player.CheckGrounded() && !player.canLadder)
        //    stateMachine.ChangeState(player.fallState);
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
    }
}
