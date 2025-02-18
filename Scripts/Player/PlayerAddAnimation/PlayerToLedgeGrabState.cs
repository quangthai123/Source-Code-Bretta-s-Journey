using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToLedgeGrabState : PlayerStates
{
    public PlayerToLedgeGrabState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        rb.gravityScale = 0f;
        rb.velocity = Vector3.zero;
        player.doubleJumped = false;
        player.attackState.airAttackCounter = 1;
        player.airDashState.airDashed = false;
        player.canGrabLedge = true;
    }

    public override void Exit()
    {
        base.Exit();
    }


    public override void Update()
    {
        base.Update();
        rb.velocity = Vector3.zero;
        if (finishAnim)
            stateMachine.ChangeState(player.ledgeGrabState);
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (Input.GetKeyDown(KeyCode.W) || InputManager.Instance.jumped)
            stateMachine.ChangeState(player.ledgeClimbState);
    }
}
