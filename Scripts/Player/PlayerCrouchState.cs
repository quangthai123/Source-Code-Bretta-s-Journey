using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerStates
{
    public PlayerCrouchState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        player.normalCol.SetActive(false);
        player.dashCol.SetActive(true);
     }
    public override void Exit()
    {
        base.Exit();
        player.normalCol.SetActive(true);
        player.dashCol.SetActive(false);
        MobileInputTesting.Instance.SetStateOfMagicBtn();
    }


    public override void Update()
    {
        base.Update();
        if (player.CheckGrounded())
            rb.linearVelocity = Vector3.zero;
        else
            stateMachine.ChangeState(player.fallState);
        if (horizontalInput != player.facingDir && InputManager.Instance.moveDir.x != player.facingDir && horizontalInput != 0 && InputManager.Instance.moveDir.x != 0)
            player.Flip();
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (!MobileInputTesting.Instance.isMobileDevice)
        {
            if (!player.CheckCeilling() && Input.GetKeyUp(KeyCode.S))
                stateMachine.ChangeState(player.exitCrouchState);
        } else
        {
            if (!player.CheckCeilling() && InputManager.Instance.moveDir.y != -1)
                stateMachine.ChangeState(player.exitCrouchState);
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift) || InputManager.Instance.dashed) && Time.time - player.dashTimer > player.dashCooldown)
        {
            player.dashTimer = Time.time;
            stateMachine.ChangeState(player.dashState);
        }
        if ((Input.GetKeyDown(KeyCode.Q) || InputManager.Instance.usedSkill) && SaveManager.instance.tempGameData.magicGemEquippedItems != null)
        {
            if (SaveManager.instance.tempGameData.magicGemEquippedItems[1] == -1)
                return;
            if (SkillManager.instance.CanUseSkillSlot2())
                stateMachine.ChangeState(player.magicState);
            else if (player.playerStats.currentMana < SkillManager.instance.GetManaToUse(1))
                PlayScreenUI.instance.IndicateWhenOutOfManaToUseSkill(SkillManager.instance.GetManaToUse(1));
        }
    }
}
