using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndDashState : PlayerStates
{
    public PlayerEndDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        rb.velocity = Vector3.zero;
        if (player.playerStatsWithItems.CheckEquippedArmorial(5)) // Check have equipped speed up armorial 
        {
            player.anim.speed = 1.25f;
        }
        if (!player.CheckSlope() && player.CheckGrounded())
            PlayerEffectSpawner.instance.Spawn("startDashFx", player.rightEffectPos.position + player.facingDir * new Vector3(.75f, 0f, 0f), Quaternion.identity);
    }
    public override void Exit()
    {
        base.Exit();
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
        if (player.CheckCeilling()) 
            stateMachine.ChangeState(player.crouchState);
        if (!player.CheckGrounded())
            stateMachine.ChangeState(player.fallState);
        if (finishAnim)
        {
            if(horizontalInput == 0 && InputManager.Instance.moveDir.x == 0f)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.runState);
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //rb.velocity = Vector3.zero;
    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (Input.GetKeyDown(KeyCode.Space) || InputManager.Instance.jumped)
            stateMachine.ChangeState(player.jumpState);
        if (Input.GetKeyDown(KeyCode.J) || InputManager.Instance.parried)
            stateMachine.ChangeState(player.shieldState);
        if (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked)
            stateMachine.ChangeState(player.attackState);
    }
}
