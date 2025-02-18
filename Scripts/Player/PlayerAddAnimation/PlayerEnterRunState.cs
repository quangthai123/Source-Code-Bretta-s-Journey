using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterRunState : PlayerOnGroundState
{
    public PlayerEnterRunState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        if (player.playerStatsWithItems.CheckEquippedArmorial(5)) // Check have equipped speed up armorial 
        {
            player.anim.speed = 1.25f;
        }
    }
    public override void Exit()
    {
        base.Exit();
        //if (!player.isKnocked)
        //    rb.velocity = Vector3.zero;
        player.anim.speed = 1f;
    }


    public override void Update()
    {
        base.Update();
        rb.sharedMaterial = player.normalPhysicMat;
        if (InputManager.Instance.moveDir.x == 0)
        {
            if ((!player.CheckSlope() || player.CheckGetOutSlope()) && (player.stateMachine.currentState == player.jumpState || player.facingDir == -1))
                rb.velocity = new Vector2(player.moveSpeed * horizontalInput, rb.velocity.y);
            else if (!player.CheckSlope() || player.CheckGetOutSlope() && (player.stateMachine.currentState != player.jumpState || player.facingDir == 1))
                rb.velocity = new Vector2(player.moveSpeed * horizontalInput, 0f);
            else
            {
                if (player.CheckJumpOnSlope() && player.stateMachine.currentState != player.jumpState)
                    rb.velocity = new Vector2(player.moveSpeed * horizontalInput * -player.slopeMoveDir.x, player.moveSpeed * horizontalInput * -player.slopeMoveDir.y);
            }
        }
        else
        {
            if ((!player.CheckSlope() || player.CheckGetOutSlope()) && (player.stateMachine.currentState == player.jumpState || player.facingDir == -1))
                rb.velocity = new Vector2(player.moveSpeed * InputManager.Instance.moveDir.x, rb.velocity.y);
            else if ((!player.CheckSlope() || player.CheckGetOutSlope()) && (player.stateMachine.currentState != player.jumpState || player.facingDir == 1))
                rb.velocity = new Vector2(player.moveSpeed * InputManager.Instance.moveDir.x, 0f);
            else
            {
                if (player.CheckJumpOnSlope() && player.stateMachine.currentState != player.jumpState)
                    rb.velocity = new Vector2(player.moveSpeed * InputManager.Instance.moveDir.x * -player.slopeMoveDir.x, player.moveSpeed * InputManager.Instance.moveDir.x * -player.slopeMoveDir.y);
            }
        }
        if (finishAnim)
        {
            if (horizontalInput != 0 || InputManager.Instance.moveDir.x != 0)
                stateMachine.ChangeState(player.runState);
            else
                stateMachine.ChangeState(player.idleState);
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();

    }
}
