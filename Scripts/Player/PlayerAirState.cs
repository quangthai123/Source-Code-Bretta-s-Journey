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
    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if ((Input.GetKeyDown(KeyCode.Space) || InputManager.Instance.jumped) && !player.doubleJumped)
        {
            if (player.TempGameData.GainedAbilities[0])
            {
                player.doubleJumped = true;
                stateMachine.ChangeState(player.doubleJumpState);
            }
        }
        if (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked)
            stateMachine.ChangeState(player.attackState);
        if(player.CheckWalled() && !player.CheckGrounded() && (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked))
            stateMachine.ChangeState(player.wallSlideState);
        if ((Input.GetKeyDown(KeyCode.Q) || InputManager.Instance.usedSkill) && SaveManager.instance.tempGameData.magicGemEquippedItems != null)
        {
            if (player.TempGameData.magicGemEquippedItems[0] == -1)
                return;
            if (SkillManager.instance.CanUseSkillSlot1())
                stateMachine.ChangeState(player.magicSkill1State);
            else if(player.playerStats.currentMana < SkillManager.instance.GetManaToUse(0))
                PlayScreenUI.instance.IndicateWhenOutOfManaToUseSkill(SkillManager.instance.GetManaToUse(0));
        }
    }
}
