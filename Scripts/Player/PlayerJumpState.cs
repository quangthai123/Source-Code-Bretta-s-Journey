using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        AudioManager.instance.PlaySFX(1);
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
        stateDuration = player.jumpDuration;
        if(!player.canLadder && player.CheckGrounded())
            PlayerEffectSpawner.instance.Spawn("startJumpFx", player.centerEffectPos.position, Quaternion.identity);
    }
    public override void Exit()
    {
        base.Exit();
        //rb.velocity = Vector3.zero; 
    }
    public override void Update()
    {
        base.Update();
        if ((InputManager.Instance.moveDir.x == 0))
        {
            rb.velocity = new Vector2(horizontalInput * player.moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(InputManager.Instance.moveDir.x* player.moveSpeed, rb.velocity.y);
        }
        if (stateDuration < 0 && rb.velocity.y < -.1f && !Input.GetKeyDown(KeyCode.Space) && !InputManager.Instance.jumped && !InputManager.Instance.dashed && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            stateMachine.ChangeState(player.fallState);
        }
        rb.gravityScale = 6f;
        if (player.CheckGrounded() && !player.CheckSlope() && rb.velocity.y < .1f)
            stateMachine.ChangeState(player.lightGroundedState);
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
