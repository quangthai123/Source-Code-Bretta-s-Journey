using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReviveState : PlayerStates
{
    public PlayerReviveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        player.transform.position = player.spawnSoulPos;
        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.reviveFx, new Vector2(player.transform.position.x, player.transform.position.y + 3.5f), Quaternion.identity);
    }
    public override void Exit()
    {
        base.Exit();
        player.isKnocked = false;
    }


    public override void Update()
    {
        base.Update();
        if(finishAnim)
            stateMachine.ChangeState(player.idleState);
    }
}
