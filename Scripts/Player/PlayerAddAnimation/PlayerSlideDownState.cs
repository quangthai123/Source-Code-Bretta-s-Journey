using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideDownState : PlayerStates
{
    public PlayerSlideDownState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void FixedUpdate()
    {
        base.Update();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.ladderMoveSpeed * -2f);
    }
    public override void LateUpdate()
    {
        if (!player.CheckGrounded() && !player.canLadder && rb.linearVelocity.y < 0.1f)
            stateMachine.ChangeState(player.fallState);
        else if (player.CheckGrounded() && rb.linearVelocity.y < 0.1f)
        {
            stateMachine.ChangeState(player.lightGroundedState);
        }
    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (verticalInput == 0f || Input.GetKeyUp(KeyCode.Space))
            stateMachine.ChangeState(player.ladderState);
    }
}
