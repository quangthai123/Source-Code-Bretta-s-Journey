using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterLadder1State : PlayerStates
{
    public PlayerEnterLadder1State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        player.knockFlip = true;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (player.transform.position.x != player.LadderPosX && stateMachine.currentState == player.enterLadder1State)
            player.transform.position = new Vector2(player.LadderPosX, player.transform.position.y);
        if (stateMachine.currentState == player.enterLadder1State)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
        }

        if (finishAnim)
            stateMachine.ChangeState(player.ladderState);
        //if (!player.CheckGrounded() && !player.canLadder)
        //    stateMachine.ChangeState(player.fallState);
    }
}
