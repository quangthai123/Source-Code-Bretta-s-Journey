using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerInteractNpcState : PlayerStates
{
    private Vector2 targetPos;
    private int targetFacingDir;
    private PlayerStates targetState;
    private Action interactCallBack;
    private Vector2 moveDir;
    public PlayerInteractNpcState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        Vector2 curPos = (Vector2)player.transform.position;
        moveDir = (targetPos - curPos).normalized;
        if ((targetPos.x > curPos.x && player.facingDir == -1) ||
            (targetPos.x < curPos.x && player.facingDir == 1))
            player.Flip();
    }
    public override void Exit()
    {
        base.Exit();
        rb.linearVelocity = Vector2.zero;
        interactCallBack?.Invoke();
    }
    public override void Update()
    {
        base.Update();
        //if (Vector2.Distance(player.transform.position, targetPos) <= .1f)
        //    stateMachine.ChangeState(targetState);
    }
    public override void FixedUpdate()
    {
        Vector2 currentPos = player.transform.position;

        float distance = Vector3.Distance(currentPos, targetPos);
        float moveStep = player.moveSpeed * Time.fixedDeltaTime;

        // Nếu đã gần tới nơi, đi thẳng đến đích luôn để không vượt quá
        if (moveStep >= distance)
        {
            rb.MovePosition(targetPos);
            if (player.facingDir != targetFacingDir)
                player.Flip();
            stateMachine.ChangeState(targetState);
        }
        else
        {
            rb.MovePosition(currentPos + moveDir * moveStep);
        }
    }
    public void SetInteractInfo(Vector2 targetPos, int targeFacingDir, PlayerStates targetState, Action callBack)
    {
        this.targetPos = targetPos;
        this.targetFacingDir = targeFacingDir;
        this.targetState = targetState;
        this.interactCallBack = callBack;
    }
}
