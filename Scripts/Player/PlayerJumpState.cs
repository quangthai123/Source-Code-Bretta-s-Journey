using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    //private float jumpHigherDuration = .08f;
    //private float jumpHigherTimer = .08f;
    //private bool endJump = false;
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        AudioManager.instance.PlaySFX(1);
        //endJump = false;
        //jumpHigherTimer = jumpHigherDuration;
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
        stateDuration = player.jumpDuration;
        if (!player.canLadder && player.CheckGrounded())
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
        if (player.CheckGrounded() && !player.CheckSlope() && rb.velocity.y < .1f)
            stateMachine.ChangeState(player.lightGroundedState);
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //if (endJump)
        //    return;
        //if (Input.GetKey(KeyCode.Space) && holdJumpBtnTimer < .15f && !releaseJumpBtn)
        //{
        //    holdJumpBtnTimer += Time.fixedDeltaTime;
        //} else
        //{
        //    releaseJumpBtn = true;
        //    //if (stateDuration > 0 || player.jumpForce * holdJumpBtnTimer * 10f < 1f)
        //    //    return;
        //    rb.velocity = new Vector2(rb.velocity.x, 0f);
        //    rb.velocity = new Vector2(rb.velocity.x, player.jumpForce * (1 + holdJumpBtnTimer));
        //    endJump = true;
        //}
        //if (jumpHigherTimer < 0 || endJump)
        //    return;
        //rb.gravityScale = 6f;
        //jumpHigherTimer -= Time.fixedDeltaTime;
        //if (Input.GetKey(KeyCode.Space) || InputManager.Instance.holdingJumpBtn)
        //    rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
        //else
        //{
        //    //rb.velocity = new Vector2(rb.velocity.x, 0f);
        //    endJump = true;
        //}
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if ((Input.GetKeyDown(KeyCode.LeftShift) || InputManager.Instance.dashed) && !player.airDashState.airDashed)
        {
            stateMachine.ChangeState(player.airDashState);
        }
    }
}
