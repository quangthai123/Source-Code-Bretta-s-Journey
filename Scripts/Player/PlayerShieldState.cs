using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldState : PlayerStates
{
    public PlayerShieldState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        stateDuration = player.shieldDuration;
        player.knockFlip = true;
        player.isShielding = true;
        AudioManager.instance.PlaySFX(9);
    }
    public override void Exit()
    {
        base.Exit();
        player.anim.ResetTrigger("FinishShield");
        player.isShielding = false;
        player.knockFlip = false;
    }
    public override void Update()
    {
        base.Update();
        rb.velocity = Vector2.zero;
        if(stateDuration < 0)
        {
            player.anim.SetTrigger("FinishShield");
            player.isShielding = false;
        }
        if (finishAnim)
            stateMachine.ChangeState(player.idleState);
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
    }
}
