using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerOnGroundState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        player.isShielding = false;
        player.hitEnemy = false;
    }
    public override void Exit()
    {
        base.Exit();
        changedToIdleState = false;
    }
    public override void Update()
    {
        base.Update();
        if(player.gotNextCombo)
        {
            stateMachine.ChangeState(player.attackState);
            return;
        } else if(InputManager.Instance.CheckCanChargeAtk() && SaveManager.instance.tempGameData.learnedSkill[4])
        {
            //Debug.Log("Do may 2!");
            stateMachine.ChangeState(player.chargingState);
            return;
        }
        if(InputManager.Instance.moveDir.x == 0) 
        {
            if (!player.CheckSlope() && player.stateMachine.currentState != player.jumpState)
                rb.velocity = Vector2.zero;
        } else 
        {
            if (!player.CheckSlope() && player.stateMachine.currentState != player.jumpState)
                rb.velocity = Vector2.zero;
        }
    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (!MobileInputTesting.Instance.isMobileDevice)
        {
            if (horizontalInput != 0 && !Input.GetKeyDown(KeyCode.J) && !Input.GetKeyDown(KeyCode.K) && !Input.GetKeyDown(KeyCode.F)
                && !(Input.GetKeyDown(KeyCode.LeftShift) && Time.time - player.dashTimer > player.dashCooldown) && !(player.CheckGrounded() && Input.GetKey(KeyCode.Space) && !player.canLadder) && !(Input.GetKey(KeyCode.S) && !player.canLadder)
                && !(Input.GetKeyDown(KeyCode.Q) && SkillManager.instance.CanUseSkill()))
            {
                stateMachine.ChangeState(player.enterRunState);
            }
        }
        else
        {
            if (InputManager.Instance.moveDir.x != 0 && !InputManager.Instance.parried && !InputManager.Instance.attacked && !InputManager.Instance.healed
                && !(InputManager.Instance.dashed && Time.time - player.dashTimer > player.dashCooldown) && !(player.CheckGrounded() && InputManager.Instance.jumped && !player.canLadder) && !(InputManager.Instance.moveDir.y == -1 && !player.canLadder)
                && !(InputManager.Instance.usedSkill && SkillManager.instance.CanUseSkill()))
            {
                stateMachine.ChangeState(player.enterRunState);
            }
        }
    }
}
