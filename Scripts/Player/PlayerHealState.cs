using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealState : PlayerStates
{
    public PlayerHealState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        player.knockFlip = true;
        stateDuration = player.healingDuration;
        if(player.playerStatsWithItems.CheckEquippedArmorial(6))
            player.anim.speed = 1.8f;
        if(player.playerStatsWithItems.CheckEquippedArmorial(7))
            player.isKnocked = true;
        AudioManager.instance.PlaySFX(8);
        //player.transform.GetComponentInc<SpriteRenderer>().sortingLayerID = 4;
    }
    public override void Exit()
    {
        base.Exit();
        player.knockFlip = false;
        player.anim.speed = 1f;
        player.isKnocked = false;
        //player.transform.GetComponent<SpriteRenderer>().sortingLayerID = 3;
    }


    public override void Update()
    {
        base.Update();
        rb.velocity = Vector2.zero;
        if (stateDuration < 0f)
            stateMachine.ChangeState(player.idleState);
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
    }
}
