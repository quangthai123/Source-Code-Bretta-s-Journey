using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{     
    private bool isJumping = false;
    private float jumpTimeCounter;
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        AudioManager.instance.PlaySFX(1);

        isJumping = true;
        jumpTimeCounter = player.maxHoldJumpTime;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpForce);

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
        // Giữ để tiếp tục nhảy
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.linearVelocity = new Vector2(horizontalInput * player.moveSpeed, rb.linearVelocity.y + player.holdJumpForce * Time.deltaTime);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        } else
            rb.linearVelocity = new Vector2(horizontalInput * player.moveSpeed, rb.linearVelocity.y);
        // Nhả phím thì dừng nhảy sớm
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
        if (rb.linearVelocity.y < -.1f && !Input.GetKeyDown(KeyCode.Space) && !InputManager.Instance.jumped && !InputManager.Instance.dashed && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            stateMachine.ChangeState(player.fallState);
        }
        if (player.CheckGrounded() && !player.CheckSlope() && rb.linearVelocity.y < 0.1f)
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
            if (player.TempGameData.GainedAbilities[1])
                stateMachine.ChangeState(player.airDashState);
        }
    }
}
