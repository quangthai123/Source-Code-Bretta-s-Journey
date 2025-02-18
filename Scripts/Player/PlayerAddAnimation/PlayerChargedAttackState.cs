using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargedAttackState : PlayerStates
{
    public PlayerChargedAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        player.isKnocked = true;
    }
    public override void Exit()
    {
        base.Exit();
        player.isKnocked = false;
    }
    public override void Update()
    {
        base.Update();
        if (finishAnim)
            stateMachine.ChangeState(player.idleState);
    }
}
