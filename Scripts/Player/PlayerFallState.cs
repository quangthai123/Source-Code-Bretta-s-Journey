using System.Collections;
using System.Collections.Generic;
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
        rb.velocity = new Vector2(horizontalInput * player.moveSpeed, rb.velocity.y);
        if(InputManager.Instance.moveDir.x != 0)
            rb.velocity = new Vector2(InputManager.Instance.moveDir.x * player.moveSpeed, rb.velocity.y);
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
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
    }
}
