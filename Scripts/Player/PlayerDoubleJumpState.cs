using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerStates
{
    public PlayerDoubleJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        player.canGrabLedge = true;
        stateDuration = player.jumpDuration;
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
        if (!player.canLadder)
            PlayerEffectSpawner.instance.Spawn("doubleJumpFx", player.centerEffectPos.position, Quaternion.identity);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if ((InputManager.Instance.moveDir.x == 0))
        {
            //if (stateDuration > 0)
            //    rb.velocity = new Vector2(horizontalInput * player.moveSpeed, player.jumpForce);
            //else
                rb.velocity = new Vector2(horizontalInput * player.moveSpeed * .8f, rb.velocity.y);
        }
        else
        {
            //if (stateDuration > 0)
            //    rb.velocity = new Vector2(InputManager.Instance.moveDir.x * player.moveSpeed, player.jumpForce);
            //else
                rb.velocity = new Vector2(InputManager.Instance.moveDir.x * player.moveSpeed * .8f, rb.velocity.y);
        }
        rb.gravityScale = 6f;
        if (stateDuration < 0 && rb.velocity.y < -.1f && !InputManager.Instance.dashed && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            stateMachine.ChangeState(player.fallState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked)
            stateMachine.ChangeState(player.attackState);
        if ((Input.GetKeyDown(KeyCode.LeftShift) || InputManager.Instance.dashed) && !player.airDashState.airDashed)
            stateMachine.ChangeState(player.airDashState);
        if (player.CheckWalled() && !player.CheckGrounded() && (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked))
            stateMachine.ChangeState(player.wallSlideState);
    }
}
