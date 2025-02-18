using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerStates
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        //player.wallSlideCol.SetActive(true);
        rb.gravityScale = 0f;
        player.doubleJumped = false;
        player.attackState.airAttackCounter = 1;
        player.airDashState.airDashed = false;
        player.canGrabLedge = true;
        rb.velocity = Vector2.zero;
    }
    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = 6f;
        //player.wallSlideCol.SetActive(false);
    }


    public override void Update()
    {
        base.Update();
        if (player.CheckWalled() && player.stateMachine.currentState != player.fallState)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
        }
        else if(player.stateMachine.currentState != player.wallJumpState)
            stateMachine.ChangeState(player.fallState);
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (Input.GetKeyDown(KeyCode.Space) || InputManager.Instance.jumped)
        {
            stateMachine.ChangeState(player.wallJumpState);
        }
        if (Input.GetKeyDown(KeyCode.S) || InputManager.Instance.moveDir.y == -1)
        {
            player.Flip();
            stateMachine.ChangeState(player.fallState);
        }
    }
}
