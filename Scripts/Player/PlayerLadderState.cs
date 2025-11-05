using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderState : PlayerStates
{
    public bool IsFromLadderInState = false;
    private float posY;
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
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        if(IsFromLadderInState)
        {
            posY = player.transform.position.y - player.LadderInOffsetY;
        }
        else
            posY = player.transform.position.y;
        player.transform.position = new Vector2(player.LadderPosX, posY);
        player.knockFlip = true;
    }
    public override void Exit()
    {
        base.Exit();
        player.anim.speed = 1f;
        IsFromLadderInState = false;
    }
    public override void Update()
    {
        base.Update();
        if(player.transform.position.x != player.LadderPosX && stateMachine.currentState == player.ladderState)
            player.transform.position = new Vector2(player.LadderPosX, player.transform.position.y);
        if(stateMachine.currentState == player.ladderState)
            rb.gravityScale = 0f;
        if(verticalInput == 0 && !InputManager.Instance.jumped && stateMachine.currentState == player.ladderState)
        {
            player.anim.speed = 0f;
            rb.linearVelocity = Vector2.zero;
        }
        if((verticalInput != 0 || InputManager.Instance.moveDir.y==-1 || InputManager.Instance.jumped) && stateMachine.currentState != player.jumpState)
        {
            player.anim.speed = 1f;
            if (InputManager.Instance.moveDir.y == 0 && !InputManager.Instance.jumped)
                rb.linearVelocity = new Vector2(0f, player.ladderMoveSpeed * verticalInput);
            else if(InputManager.Instance.jumped)
            {
                rb.linearVelocity = new Vector2(0f, player.ladderMoveSpeed);
            } else
                rb.linearVelocity = new Vector2(0f, player.ladderMoveSpeed * -1);
        }
    }
    public override void LateUpdate()
    {
        if (!player.CheckGrounded() && !player.canLadder)
            stateMachine.ChangeState(player.fallState);
        else if (player.CheckGrounded() && rb.linearVelocity.y < 0f)
        {
            stateMachine.ChangeState(player.lightGroundedState);
        }
    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if ((Input.GetKeyDown(KeyCode.LeftShift) || InputManager.Instance.dashed) && verticalInput == 0)
            stateMachine.ChangeState(player.fallState);
        if (InputManager.Instance.jumped || Input.GetKeyDown(KeyCode.Space) && verticalInput >= 0f) // must add mobile input
        {
            player.knockFlip = false;
            if (Input.GetAxisRaw("Horizontal") < 0 && player.facingDir == 1)
                player.Flip();
            if (Input.GetAxisRaw("Horizontal") > 0 && player.facingDir == -1)
                player.Flip();
            stateMachine.ChangeState(player.jumpLadderState);
        }
        if(verticalInput < 0f && Input.GetKey(KeyCode.Space)) // must add mobile input
        {
            stateMachine.ChangeState(player.slideDownState);
        } 
    }
}
