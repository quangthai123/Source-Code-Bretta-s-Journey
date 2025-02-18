using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAirState
{
    private float enterWallJumpTime;
    private bool wallJumped = false;
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        enterWallJumpTime = Time.time;
        player.Flip();
        rb.velocity = new Vector2(player.wallJumpForce.x * player.facingDir, player.wallJumpForce.y);
        wallJumped = false;
    }
    public override void Exit()
    {
        base.Exit();
        player.knockFlip = false;
    }


    public override void Update()
    {
        base.Update();
        if (rb.velocity.y < -0.1f)
            stateMachine.ChangeState(player.fallState);
        if (player.CheckWalled() && !player.CheckGrounded() && (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked))
            stateMachine.ChangeState(player.wallSlideState);
        if(InputManager.Instance.moveDir.x == 0) 
        {
            if (horizontalInput != 0 && horizontalInput != player.facingDir && Time.time - enterWallJumpTime >= player.allowWallJumpUpMinTime && Time.time - enterWallJumpTime <= player.allowWallJumpUpMaxTime && !wallJumped)
            {
                wallJumped = true;
                rb.velocity = new Vector2(player.wallJumpForce.x * -player.facingDir, player.wallJumpForce.y);
            }
            else if (horizontalInput != 0 && horizontalInput == player.facingDir) 
            {
                rb.velocity = new Vector2(horizontalInput * player.moveSpeed, rb.velocity.y);
            }
        } else {
            if (InputManager.Instance.moveDir.x != 0 && InputManager.Instance.moveDir.x != player.facingDir && Time.time - enterWallJumpTime >= player.allowWallJumpUpMinTime && Time.time - enterWallJumpTime <= player.allowWallJumpUpMaxTime && !wallJumped) {
                wallJumped = true;
                rb.velocity = new Vector2(player.wallJumpForce.x * -player.facingDir, player.wallJumpForce.y);
            }
            else if (InputManager.Instance.moveDir.x != 0 && InputManager.Instance.moveDir.x == player.facingDir) {
                rb.velocity = new Vector2(InputManager.Instance.moveDir.x * player.moveSpeed, rb.velocity.y);
            }
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
    }
}
