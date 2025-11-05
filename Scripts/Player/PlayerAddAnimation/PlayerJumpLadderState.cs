using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpLadderState : PlayerStates
{
    public PlayerJumpLadderState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (stateMachine.currentState == player.jumpLadderState)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
        }
        if (finishAnim)
            stateMachine.ChangeState(player.jumpState);
    }
}
