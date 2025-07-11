using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerStates
{
    public int comboCounter = 0;
    private float attackExitTime;
    public int airAttackCounter = 1;
    private bool airAttacking = false;
    public PlayerAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        //Debug.Log("Enter Attack State");
        player.canHaveNextCombo = false;
        player.gotNextCombo = false;
        if (player.CheckGrounded())
        {
            if (Time.time - attackExitTime > player.allowComboTime || !player.CheckOpponentInAttackRange())
                comboCounter = 0;
            if (comboCounter == 3 && SaveManager.instance.tempGameData.learnedSkill[1])
                player.anim.speed = 1.5f;
            if (player.playerStatsWithItems.CheckEquippedPerfectSword(4))
            {
                if(comboCounter != 3 || (comboCounter == 3 && !SaveManager.instance.tempGameData.learnedSkill[1]))
                    player.anim.speed = 1.1f;
                else if(comboCounter == 3 && SaveManager.instance.tempGameData.learnedSkill[1])
                    player.anim.speed = 1.65f;
            }
            if(player.playerStatsWithItems.CheckActivatedSwordPair(0))
            {
                if (comboCounter != 3 || (comboCounter == 3 && !SaveManager.instance.tempGameData.learnedSkill[1]))
                    player.anim.speed = 1.15f;
                else if (comboCounter == 3 && SaveManager.instance.tempGameData.learnedSkill[1])
                    player.anim.speed = 1.725f;
            }
        }
        else if (airAttackCounter == 1)
        {
            player.anim.SetBool("AirAttack1", true);
            airAttacking = true;
            return;
        }
        else if (airAttackCounter == 2)
        {
            player.anim.SetBool("AirAttack2", true);       
            airAttacking = true;
            return;
        }
        else
        {
            stateMachine.ChangeState(player.fallState);
            return;
        }
        if(comboCounter != 0)
            player.knockFlip = true;
        player.anim.SetInteger("AttackCount", comboCounter);
        
    }
    public override void Exit()
    {
        base.Exit();
        player.anim.SetBool("AirAttack1", false);
        player.anim.SetBool("AirAttack2", false);
        if (player.CheckGrounded())
        {
            attackExitTime = Time.time;
            if (comboCounter < 2 && player.CheckOpponentInAttackRange())
                comboCounter++;
            else if(comboCounter == 2 && player.CheckOpponentInAttackRange() 
                && SaveManager.instance.tempGameData.learnedSkill[0])
                comboCounter++;
            else
                comboCounter = 0;

            if((Input.GetKey(KeyCode.S) || InputManager.Instance.holdingMoveDownBtn) && comboCounter == 3 &&
                SaveManager.instance.tempGameData.learnedSkill[3])
                comboCounter++;
        }
        else
            airAttackCounter++;
        player.knockFlip = false;
        player.anim.speed = 1f;
        airAttacking = false;
        if (((horizontalInput < 0 && player.facingDir == 1) || (horizontalInput > 0 && player.facingDir == -1)) ||
            ((InputManager.Instance.moveDir.x < 0 && player.facingDir == 1) || (InputManager.Instance.moveDir.x > 0 && player.facingDir == -1)))
        {
            player.Flip();
        }
    }
    public override void Update()
    {
        base.Update();
        if (player.CheckGrounded())
        {
            rb.velocity = Vector2.zero;
            if(airAttacking)
                stateMachine.ChangeState(player.lightGroundedState);
        }
        else {
            if(InputManager.Instance.moveDir.x == 0)
                rb.velocity = new Vector2(horizontalInput * 0.8f * player.moveSpeed, rb.velocity.y);
            else
                rb.velocity = new Vector2(InputManager.Instance.moveDir.x * 0.8f * player.moveSpeed, rb.velocity.y);
        }
        if (finishAnim && player.CheckGrounded())
            stateMachine.ChangeState(player.idleState);
        else if (finishAnim && !player.CheckGrounded())
            stateMachine.ChangeState(player.fallState);
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if ((Input.GetKeyDown(KeyCode.LeftShift) || InputManager.Instance.dashed) && Time.time - player.dashTimer > player.dashCooldown)
        {
            if(player.CheckGrounded())
                stateMachine.ChangeState(player.dashState);
            else
                stateMachine.ChangeState(player.airDashState);
        }
        if (player.CheckGrounded() && (Input.GetKeyDown(KeyCode.J) || InputManager.Instance.parried))
            stateMachine.ChangeState(player.shieldState);
        if (player.canHaveNextCombo && (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked))
            player.gotNextCombo = true;
    }
}
