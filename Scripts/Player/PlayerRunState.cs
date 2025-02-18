using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerOnGroundState
{
    public PlayerRunState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        if(player.playerStatsWithItems.CheckEquippedArmorial(5)) // Check have equipped speed up armorial 
        {
            player.anim.speed = 1.25f;
        }
        Debug.Log("Enter run State!!!");
    }
    public override void Exit()
    {
        base.Exit();
        //if(!player.isKnocked)
        //    rb.velocity = Vector3.zero;
        player.anim.speed = 1f;
    }

    public override void Update()
    {
        base.Update();
        rb.sharedMaterial = player.normalPhysicMat;
        if (InputManager.Instance.moveDir.x == 0)
        {
            if ((!player.CheckSlope() || player.CheckGetOutSlope()) && (player.stateMachine.currentState == player.jumpState || player.facingDir == -1))
                rb.velocity = new Vector2(player.moveSpeed * horizontalInput, rb.velocity.y);
            else if (!player.CheckSlope() || player.CheckGetOutSlope() && (player.stateMachine.currentState != player.jumpState || player.facingDir == 1))
                rb.velocity = new Vector2(player.moveSpeed * horizontalInput, 0f);
            else
            {
                if (player.CheckJumpOnSlope() && player.stateMachine.currentState != player.jumpState)
                    rb.velocity = new Vector2(player.moveSpeed * horizontalInput * -player.slopeMoveDir.x, player.moveSpeed * horizontalInput * -player.slopeMoveDir.y);
            }
        }
        else
        {
            if ((!player.CheckSlope() || player.CheckGetOutSlope()) && (player.stateMachine.currentState == player.jumpState || player.facingDir == -1))
                rb.velocity = new Vector2(player.moveSpeed * InputManager.Instance.moveDir.x, rb.velocity.y);
            else if ((!player.CheckSlope() || player.CheckGetOutSlope()) && (player.stateMachine.currentState != player.jumpState || player.facingDir == 1))
                rb.velocity = new Vector2(player.moveSpeed * InputManager.Instance.moveDir.x, 0f);
            else
            {
                if (player.CheckJumpOnSlope() && player.stateMachine.currentState != player.jumpState)
                    rb.velocity = new Vector2(player.moveSpeed * InputManager.Instance.moveDir.x * -player.slopeMoveDir.x, player.moveSpeed * InputManager.Instance.moveDir.x * -player.slopeMoveDir.y);
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
        if (!MobileInputTesting.Instance.isMobileDevice)
        {
            if (horizontalInput == 0 && !Input.GetKeyDown(KeyCode.J) && !Input.GetKeyDown(KeyCode.K) && !Input.GetKeyDown(KeyCode.F)
                && !(Input.GetKeyDown(KeyCode.LeftShift) && Time.time - player.dashTimer > player.dashCooldown) && !(player.CheckGrounded() && Input.GetKey(KeyCode.Space) && !player.canLadder) && !(Input.GetKey(KeyCode.S) && !player.canLadder)
                && !(Input.GetKeyDown(KeyCode.Q) && SkillManager.instance.CanUseSkill()))
            {
                stateMachine.ChangeState(player.breakRunState);
            }
        }
        else
        {
            if (InputManager.Instance.moveDir.x == 0 && !InputManager.Instance.parried && !InputManager.Instance.attacked && !InputManager.Instance.healed
                && !(InputManager.Instance.dashed && Time.time - player.dashTimer > player.dashCooldown) && !(player.CheckGrounded() && InputManager.Instance.jumped && !player.canLadder) && !(InputManager.Instance.moveDir.y == -1 && !player.canLadder)
                && !(InputManager.Instance.usedSkill && SkillManager.instance.CanUseSkill()))
            {
                stateMachine.ChangeState(player.breakRunState);
            }

        }
    }
}
