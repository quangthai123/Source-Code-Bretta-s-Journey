using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerStates
{
    private bool isJumping = false;
    private float jumpTimeCounter;
    public PlayerDoubleJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        player.canGrabLedge = true;

        isJumping = true;
        jumpTimeCounter = player.maxHoldJumpTime;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpForce);

        AudioManager.instance.PlaySFX(1);
        PlayerEffectSpawner.instance.Spawn("doubleJumpFx", player.centerEffectPos.position, Quaternion.identity);
    }
    public override void Exit()
    {
        base.Exit();
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
        }
        else
            rb.linearVelocity = new Vector2(horizontalInput * player.moveSpeed, rb.linearVelocity.y);

        // Nhả phím thì dừng nhảy sớm
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
        if (stateDuration < 0 && rb.linearVelocity.y < -.1f && !InputManager.Instance.dashed && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            stateMachine.ChangeState(player.fallState);
        }
        if (player.CheckGrounded() && !player.CheckSlope() && rb.linearVelocity.y < .1f)
            stateMachine.ChangeState(player.lightGroundedState);
    }

    public override void FixedUpdate()
    {
        //base.FixedUpdate();
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
        if (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked)
            stateMachine.ChangeState(player.attackState);
        if ((Input.GetKeyDown(KeyCode.LeftShift) || InputManager.Instance.dashed) && !player.airDashState.airDashed)
        {
            if (player.TempGameData.GainedAbilities[1])
                stateMachine.ChangeState(player.airDashState);
        }
        if (player.CheckWalled() && !player.CheckGrounded() && (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked))
            stateMachine.ChangeState(player.wallSlideState);
        if ((Input.GetKeyDown(KeyCode.Q) || InputManager.Instance.usedSkill) && SaveManager.instance.tempGameData.magicGemEquippedItems != null)
        {
            if (SaveManager.instance.tempGameData.magicGemEquippedItems[0] == -1)
                return;
            if (SkillManager.instance.CanUseSkillSlot1())
                stateMachine.ChangeState(player.magicSkill1State);
            else if(player.playerStats.currentMana < SkillManager.instance.GetManaToUse(0))
                PlayScreenUI.instance.IndicateWhenOutOfManaToUseSkill(SkillManager.instance.GetManaToUse(0));
        }
    }
}
