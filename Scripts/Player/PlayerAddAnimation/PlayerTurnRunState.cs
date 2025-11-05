using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnRunState : PlayerOnGroundState
{
    public PlayerTurnRunState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        if (player.playerStatsWithItems.CheckEquippedArmorial(5)) // Check have equipped speed up armorial 
        {
            player.anim.speed = 1.25f;
        }
        stateDuration = .1f;
        Transform fx = PlayerEffectSpawner.instance.Spawn("startDashFx", player.leftEffectPos.position - player.facingDir * new Vector3(.3f, 0f, 0f), Quaternion.identity);
        fx.GetComponent<PlayerEffectDespawnByAnim>().Flip();
    }
    public override void Exit()
    {
        base.Exit();
        player.anim.speed = 1f;
        //Time.timeScale = 1f;
    }


    public override void Update()
    {
        base.Update();
        rb.sharedMaterial = player.normalPhysicMat;
        if (stateMachine.currentState == player.turnRunState && stateDuration > 0f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (InputManager.Instance.moveDir.x == 0)
        {
            if ((!player.CheckSlope() || player.CheckGetOutSlope()) && (player.stateMachine.currentState == player.jumpState || player.facingDir == -1))
                rb.linearVelocity = new Vector2(player.moveSpeed * horizontalInput, rb.linearVelocity.y);
            else if (!player.CheckSlope() || player.CheckGetOutSlope() && (player.stateMachine.currentState != player.jumpState || player.facingDir == 1))
                rb.linearVelocity = new Vector2(player.moveSpeed * horizontalInput, 0f);
            else
            {
                if (player.CheckJumpOnSlope() && player.stateMachine.currentState != player.jumpState)
                    rb.linearVelocity = new Vector2(player.moveSpeed * horizontalInput * -player.slopeMoveDir.x, player.moveSpeed * horizontalInput * -player.slopeMoveDir.y);
            }
        }
        else
        {
            if ((!player.CheckSlope() || player.CheckGetOutSlope()) && (player.stateMachine.currentState == player.jumpState || player.facingDir == -1))
                rb.linearVelocity = new Vector2(player.moveSpeed * InputManager.Instance.moveDir.x, rb.linearVelocity.y);
            else if ((!player.CheckSlope() || player.CheckGetOutSlope()) && (player.stateMachine.currentState != player.jumpState || player.facingDir == 1))
                rb.linearVelocity = new Vector2(player.moveSpeed * InputManager.Instance.moveDir.x, 0f);
            else
            {
                if (player.CheckJumpOnSlope() && player.stateMachine.currentState != player.jumpState)
                    rb.linearVelocity = new Vector2(player.moveSpeed * InputManager.Instance.moveDir.x * -player.slopeMoveDir.x, player.moveSpeed * InputManager.Instance.moveDir.x * -player.slopeMoveDir.y);
            }
        }
        if (finishAnim)
            stateMachine.ChangeState(player.runState);
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (!MobileInputTesting.Instance.isMobileDevice)
        {
            if (horizontalInput == 0 && !Input.GetKeyDown(KeyCode.J) && !Input.GetKeyDown(KeyCode.K) && !Input.GetKeyDown(KeyCode.F)
                && !(Input.GetKeyDown(KeyCode.LeftShift) && Time.time - player.dashTimer > player.dashCooldown) && !(player.CheckGrounded() && Input.GetKey(KeyCode.Space) && !player.canLadder) && !(Input.GetKey(KeyCode.S) && !player.canLadder)
                && !(Input.GetKeyDown(KeyCode.Q) && SkillManager.instance.CanUseSkillSlot1()))
            {
                stateMachine.ChangeState(player.breakRunState);
            }
        }
        else
        {
            if (InputManager.Instance.moveDir.x == 0 && !InputManager.Instance.parried && !InputManager.Instance.attacked && !InputManager.Instance.healed
                && !(InputManager.Instance.dashed && Time.time - player.dashTimer > player.dashCooldown) && !(player.CheckGrounded() && InputManager.Instance.jumped && !player.canLadder) && !(InputManager.Instance.moveDir.y == -1 && !player.canLadder)
                && !(InputManager.Instance.usedSkill && SkillManager.instance.CanUseSkillSlot1()))
            {
                stateMachine.ChangeState(player.breakRunState);
            }

        }
    }
}
