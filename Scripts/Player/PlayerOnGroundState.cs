using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundState : PlayerStates
{
    protected bool changedToRunState;
    protected bool changedToIdleState;
    public PlayerOnGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        player.doubleJumped = false;
        player.attackState.airAttackCounter = 1;
        player.airDashState.airDashed = false;
        player.canGrabLedge = true;
        player.spawnSoulPos = player.transform.position;
    }
    public override void Exit()
    {
        base.Exit();
        MakeEnableNormalCol(true);
        player.spawnSoulPos = new Vector2(player.transform.position.x - player.facingDir, player.transform.position.y);
    }
    public override void Update()
    {
        base.Update();
        if (!player.CheckGrounded() && !player.CheckSlope() && !player.canLadder && !player.CheckGetOutSlope())
            stateMachine.ChangeState(player.fallState);
        if(player.isDead)
            stateMachine.ChangeState(player.deathState);

    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if(player.CheckGrounded() && (Input.GetKeyDown(KeyCode.Space) || InputManager.Instance.jumped) && !player.canLadder)
            stateMachine.ChangeState(player.jumpState);
        if (Input.GetKeyDown(KeyCode.J) || InputManager.Instance.parried)
            stateMachine.ChangeState(player.shieldState);
        if ((Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked))
        {
            stateMachine.ChangeState(player.attackState);
        }
        if((Input.GetKeyDown(KeyCode.LeftShift) || InputManager.Instance.dashed) && Time.time - player.dashTimer > player.dashCooldown)
        {
            
            stateMachine.ChangeState(player.dashState);
        }
        if ((Input.GetKeyDown(KeyCode.F) || InputManager.Instance.healed) && player.playerStats.fullHealFlaskQuantity > 0)
            stateMachine.ChangeState(player.healState);
        if ((Input.GetKey(KeyCode.S) || InputManager.Instance.moveDir.y == -1) && !player.canLadder && !player.gotNextCombo)
        {
            if (!player.canPickup)
                stateMachine.ChangeState(player.enterCrouchState);
            else
                stateMachine.ChangeState(player.pickupState);
        }
        if ((Input.GetKeyDown(KeyCode.Q) || InputManager.Instance.usedSkill) && SaveManager.instance.tempGameData.magicGemEquippedItems != null) 
        {
            if (SaveManager.instance.tempGameData.magicGemEquippedItems[0] == -1)
                return;
            if(SkillManager.instance.CanUseSkillSlot1())
                stateMachine.ChangeState(player.magicSkill1State);
            else if (player.playerStats.currentMana < SkillManager.instance.GetManaToUse(0))
                PlayScreenUI.instance.IndicateWhenOutOfManaToUseSkill(SkillManager.instance.GetManaToUse(0));
        }
    }
}
