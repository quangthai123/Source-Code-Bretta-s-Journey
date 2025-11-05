using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeGrabState : PlayerStates
{
    public PlayerLedgeGrabState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector3.zero;
        player.normalCol.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        rb.linearVelocity = Vector3.zero;
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (Input.GetKeyDown(KeyCode.W) || InputManager.Instance.jumped)
            stateMachine.ChangeState(player.ledgeClimbState);
        if ((Input.GetKeyDown(KeyCode.S) || InputManager.Instance.moveDir.y == -1))
        {
            player.canGrabLedge = false;
            player.normalCol.SetActive(true);
            stateMachine.ChangeState(player.fallState);
        }
    }
}
