using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderState : PlayerStates
{
    public PlayerLadderState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        player.doubleJumped = false;
        player.attackState.airAttackCounter = 1;
        player.airDashState.airDashed = false;
        player.canGrabLedge = true;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if(stateMachine.currentState != player.fallState && stateMachine.currentState != player.jumpState)
            rb.gravityScale = 0f;
        if(verticalInput == 0 && !InputManager.Instance.jumped && stateMachine.currentState != player.jumpState)
        {
            player.anim.SetBool("LadderMove", false);
            rb.velocity = Vector2.zero;
        }
        if((verticalInput != 0 || InputManager.Instance.moveDir.y==-1 || InputManager.Instance.jumped) && stateMachine.currentState != player.jumpState)
        {
            player.anim.SetBool("LadderMove", true);
            if(InputManager.Instance.moveDir.y == 0 && !InputManager.Instance.jumped)
                rb.velocity = new Vector2(0f, player.ladderMoveSpeed * verticalInput);
            else if(InputManager.Instance.jumped)
            {
                rb.velocity = new Vector2(0f, player.ladderMoveSpeed);
            } else
                rb.velocity = new Vector2(0f, player.ladderMoveSpeed * -1);
        }
        if (!player.CheckGrounded() && !player.canLadder)
            stateMachine.ChangeState(player.fallState);
        else if (player.CheckGrounded())
            stateMachine.ChangeState(player.idleState);
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (Input.GetKeyDown(KeyCode.LeftShift) || InputManager.Instance.dashed)
            stateMachine.ChangeState(player.fallState);
        if ((InputManager.Instance.moveDir.x != 0 && InputManager.Instance.jumped) || (horizontalInput != 0 && 
            Input.GetKeyDown(KeyCode.Space)))
            stateMachine.ChangeState(player.jumpState);
    }
}
