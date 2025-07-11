using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerStates
{
    //private float jumpHigherDuration = .08f;
    //private float jumpHigherTimer = .08f;
    //private bool endJump = false;
    public PlayerDoubleJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        player.canGrabLedge = true;
        stateDuration = player.jumpDuration;
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
        //endJump = false;
        //jumpHigherTimer = jumpHigherDuration;
        AudioManager.instance.PlaySFX(1);
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
                rb.velocity = new Vector2(horizontalInput * player.moveSpeed, rb.velocity.y);
        }
        else
        {
            //if (stateDuration > 0)
            //    rb.velocity = new Vector2(InputManager.Instance.moveDir.x * player.moveSpeed, player.jumpForce);
            //else
                rb.velocity = new Vector2(InputManager.Instance.moveDir.x * player.moveSpeed, rb.velocity.y);
        }
        if (stateDuration < 0 && rb.velocity.y < -.1f && !InputManager.Instance.dashed && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            stateMachine.ChangeState(player.fallState);
        }
        if (player.CheckGrounded() && !player.CheckSlope() && rb.velocity.y < .1f)
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
            stateMachine.ChangeState(player.airDashState);
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
