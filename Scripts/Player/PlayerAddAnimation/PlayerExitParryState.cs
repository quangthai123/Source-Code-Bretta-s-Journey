using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExitParryState : PlayerOnGroundState
{
    public PlayerExitParryState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        player.knockFlip = true;
    }
    public override void Exit()
    {
        base.Exit();
        player.knockFlip = false;
    }


    public override void Update()
    {
        base.Update();
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        if (finishAnim)
            stateMachine.ChangeState(player.idleState);
    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if(player.facingDir != horizontalInput && player.facingDir != InputManager.Instance.moveDir.x && stateMachine.currentState == player.dashState
            && (horizontalInput != 0 || InputManager.Instance.moveDir.x != 0)) 
        {
            player.Flip();
        }
    }
}
