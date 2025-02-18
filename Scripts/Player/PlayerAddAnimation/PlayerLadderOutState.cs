using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderOutState : PlayerStates
{
    public PlayerLadderOutState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        rb.velocity = Vector2.zero;
        if (finishAnim)
            stateMachine.ChangeState(player.idleState);
    }
}
