using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupState : PlayerStates
{
    public PlayerPickupState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        rb.linearVelocity = Vector3.zero;
        if ((player.transform.position.x < player.currentItemPosX && player.facingDir == -1) ||
            (player.transform.position.x > player.currentItemPosX && player.facingDir == 1))
        {
            player.Flip();
        }
        player.isKnocked = true;
        if(player.transform.position.x < player.currentItemPosX)
            player.transform.position = new Vector2(player.currentItemPosX - .9f, player.transform.position.y);
        else
            player.transform.position = new Vector2(player.currentItemPosX + .9f, player.transform.position.y);
    }
    public override void Exit()
    {
        base.Exit();
        player.isKnocked = false;
        player.canPickup = false;
        player.pickupedItem = true;
    }


    public override void Update()
    {
        base.Update();
        rb.linearVelocity = Vector3.zero;
        if (finishAnim)
            stateMachine.ChangeState(player.exitCrouchState);
    }
}
