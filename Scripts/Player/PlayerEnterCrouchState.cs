using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterCrouchState : PlayerStates
{
    public PlayerEnterCrouchState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        rb.linearVelocity = Vector2.zero;
        MobileInputTesting.Instance.CanEnableMagicBtnToUseMagicSkill2();
    }
    public override void Exit()
    {
        base.Exit();
    }


    public override void Update()
    {
        base.Update();
        if (player.CheckGrounded())
            rb.linearVelocity = Vector3.zero;
        else
            stateMachine.ChangeState(player.fallState);
        if (finishAnim)
            stateMachine.ChangeState(player.crouchState);
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if(InputManager.Instance.moveDir.y == 0)
        {
            if (Input.GetKeyUp(KeyCode.S))
                stateMachine.ChangeState(player.exitCrouchState);
        } else
        {
            if(InputManager.Instance.moveDir.y != -1)
                stateMachine.ChangeState(player.exitCrouchState);
        }
        if ((Input.GetKeyDown(KeyCode.Q) || InputManager.Instance.usedSkill) && SaveManager.instance.tempGameData.magicGemEquippedItems != null)
        {
            if (SaveManager.instance.tempGameData.magicGemEquippedItems[1] == -1)
                return;
            if (SkillManager.instance.CanUseSkillSlot2())
                stateMachine.ChangeState(player.magicState);
            else if(player.playerStats.currentMana < SkillManager.instance.GetManaToUse(1))
                PlayScreenUI.instance.IndicateWhenOutOfManaToUseSkill(SkillManager.instance.GetManaToUse(1));
        }
    }
}
