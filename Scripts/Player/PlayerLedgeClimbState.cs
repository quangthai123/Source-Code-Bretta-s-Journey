using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerStates
{
    public PlayerLedgeClimbState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        rb.gravityScale = 0f;
        //player.normalCol.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
        player.normalCol.SetActive(true);
        player.transform.position = new Vector2(player.transform.position.x + player.facingDir * player.offSetClimbedUp.x, player.transform.position.y + player.offSetClimbedUp.y);
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = Vector3.zero;
        if (finishAnim)
            stateMachine.ChangeState(player.idleState);
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
    }
}
