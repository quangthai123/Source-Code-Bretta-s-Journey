using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    private Vector2 fallPos;
    public PlayerFallState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        fallPos = player.transform.position;
    }
    public override void Exit()
    {
        base.Exit();
    }


    public override void Update()
    {
        base.Update();
        if (player.gotNextCombo)
        {
            stateMachine.ChangeState(player.attackState);
            return;
        }
        rb.linearVelocity = new Vector2(horizontalInput * player.moveSpeed, rb.linearVelocity.y);
        if(InputManager.Instance.moveDir.x != 0)
            rb.linearVelocity = new Vector2(InputManager.Instance.moveDir.x * player.moveSpeed, rb.linearVelocity.y);
        if (player.CheckGrounded())
        {
            if(!player.CheckSlope() || (player.CheckSlope() && player.CheckJumpOnSlope()))
            {
                if (Mathf.Abs(player.transform.position.y - fallPos.y) >= player.landingCheckDistance) 
                {
                    stateMachine.ChangeState(player.landingState);
                } else
                {
                    PlayerEffectSpawner.instance.Spawn("lightGroundedFx", player.centerEffectPos.position, Quaternion.identity);
                    AudioManager.instance.PlaySFX(3);
                    if (horizontalInput == 0 && InputManager.Instance.moveDir.x == 0)
                        stateMachine.ChangeState(player.lightGroundedState);
                    else
                        stateMachine.ChangeState(player.enterRunState);
                }
            }
        }
        if (player.CheckLedge() && !player.canLadder)
        {
            stateMachine.ChangeState(player.toLedgeGrabState);
        }
        if(SceneScenarioSelectLv.instance != null)
        {
            if (!SceneScenarioSelectLv.instance.isScenario)
                stateMachine.ChangeState(player.fallingState);
        }
        if (player.isDead)
            stateMachine.ChangeState(player.deathState);
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //if(stateMachine.currentState != player.landingState && stateMachine.currentState != player.heavyLandingState)
    }
    public override void LateUpdate() 
    {
        base.LateUpdate();
    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if ((Input.GetKeyDown(KeyCode.LeftShift) || InputManager.Instance.dashed))
        {
            if (!player.CheckAirDashGrounded() && !player.airDashState.airDashed) 
            {
                if (player.TempGameData.GainedAbilities[1])
                    stateMachine.ChangeState(player.airDashState);          
            }
            else if (player.CheckAirDashGrounded())
            {
                Debug.Log("Dash from fall");
                player.canChangeToDashState = true;
            }

        }
        if(verticalInput != 0 && player.canLadder && player.transform.position.y > player.LadderBottomPosY)
        {
            if (Mathf.Abs(player.LadderPosX - player.transform.position.x) >= .5f)
                stateMachine.ChangeState(player.enterLadderState);
            else
                stateMachine.ChangeState(player.enterLadder1State);
        }
    }
}
