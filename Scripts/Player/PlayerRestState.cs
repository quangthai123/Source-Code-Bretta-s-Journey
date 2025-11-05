using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRestState : PlayerStates
{
    public bool IsEndRest { get; set; }
    public PlayerRestState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        IsEndRest = false;
        player.isKnocked = true;
        player.anim.ResetTrigger("OutRest");
    }

    public override void Exit()
    {
        base.Exit();
        player.isKnocked = false;
        PlayScreenUI.instance.ShowControlUI();
        if (CheckPoint.instance != null)
            CheckPoint.instance.ShowInteractImage();
    }

    public override void Update()
    {
        base.Update();
        if (IsEndRest)
            player.anim.SetTrigger("OutRest");
        if (finishAnim)
            stateMachine.ChangeState(player.idleState);
    }
    public void SetOutRest() => IsEndRest = true;
}
