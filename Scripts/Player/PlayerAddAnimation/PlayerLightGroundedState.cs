using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightGroundedState : PlayerOnGroundState
{
    public PlayerLightGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (finishAnim)
            stateMachine.ChangeState(player.idleState);

    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (!MobileInputTesting.Instance.isMobileDevice)
        {
            if (horizontalInput != 0 && !Input.GetKeyDown(KeyCode.J) && !Input.GetKeyDown(KeyCode.K) && !Input.GetKeyDown(KeyCode.F)
                && !(Input.GetKeyDown(KeyCode.LeftShift) && Time.time - player.dashTimer > player.dashCooldown) && !(player.CheckGrounded() && Input.GetKey(KeyCode.Space) && !player.canLadder) && !(Input.GetKey(KeyCode.S) && !player.canLadder)
                && !(Input.GetKeyDown(KeyCode.Q) && SkillManager.instance.CanUseSkillSlot1()))
            {
                stateMachine.ChangeState(player.enterRunState);
            }
        }
        else
        {
            if (InputManager.Instance.moveDir.x != 0 && !InputManager.Instance.parried && !InputManager.Instance.attacked && !InputManager.Instance.healed
                && !(InputManager.Instance.dashed && Time.time - player.dashTimer > player.dashCooldown) && !(player.CheckGrounded() && InputManager.Instance.jumped && !player.canLadder) && !(InputManager.Instance.moveDir.y == -1 && !player.canLadder)
                && !(InputManager.Instance.usedSkill && SkillManager.instance.CanUseSkillSlot1()))
            {
                stateMachine.ChangeState(player.enterRunState);
            }
        }
    }
}
