using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerStates
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        player.attackState.comboCounter = 0;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        //if (player.CheckGrounded() && !player.CheckSlope())
        //    stateMachine.ChangeState(player.lightGroundedState);
        //if (horizontalInput == 0 && InputManager.Instance.moveDir.x == 0)
        //    stateMachine.ChangeState(player.lightGroundedState);
        //else if (!player.CheckSlope() || (player.CheckSlope() && player.CheckJumpOnSlope()))
        //    stateMachine.ChangeState(player.enterRunState);
    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (Input.GetKeyDown(KeyCode.Space) && !player.doubleJumped)
        {
            Debug.Log("Jumped");
            player.doubleJumped = true;
            stateMachine.ChangeState(player.doubleJumpState);
        }
        if (InputManager.Instance.jumped && !player.doubleJumped)
        {
            Debug.Log("Jumped");
            player.doubleJumped = true;
            stateMachine.ChangeState(player.doubleJumpState);
        }
        if (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked)
            stateMachine.ChangeState(player.attackState);
        //if ((Input.GetKeyDown(KeyCode.LeftShift) || InputManager.Instance.dashed))
        //{
        //    if(!player.CheckAirDashGrounded() && !player.airDashState.airDashed)
        //        stateMachine.ChangeState(player.airDashState);
        //}
        if(player.CheckWalled() && !player.CheckGrounded() && (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked))
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
