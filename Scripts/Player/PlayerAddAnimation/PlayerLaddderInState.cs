using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaddderInState : PlayerStates
{
    public PlayerLaddderInState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        rb.velocity = Vector3.zero;
        if (finishAnim)
            stateMachine.ChangeState(player.ladderState);
    }
}
