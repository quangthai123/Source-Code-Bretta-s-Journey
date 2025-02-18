using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicState : PlayerStates {
    public PlayerMagicState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) 
    {
    }

    public override void Start() 
    {
        base.Start();
        if (player.playerStatsWithItems.CheckEquippedPerfectSword(12))
            player.anim.speed = 1.75f;
        if(player.playerStatsWithItems.CheckActivatedSwordPair(1))
            player.anim.speed = 2f;
        player.knockFlip = true;
        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.useSkillFx1, player.transform.position, Quaternion.identity);
        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.useSkillFx3, player.transform.position, Quaternion.identity);
        SkillManager.instance.UseSkill();
    }
    public override void Exit() 
    {
        base.Exit();
        player.knockFlip = false;
        player.anim.speed = 1f;
    }


    public override void Update() {
        base.Update();
        rb.velocity = Vector3.zero;
        if(finishAnim)
            stateMachine.ChangeState(player.idleState);
    }
}
