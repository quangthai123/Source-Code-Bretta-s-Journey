using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicSkill1State : PlayerStates
{
    public PlayerMagicSkill1State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Start()
    {
        base.Start();
        if (player.playerStatsWithItems.CheckEquippedPerfectSword(12))
            player.anim.speed = 1.75f;
        if (player.playerStatsWithItems.CheckActivatedSwordPair(1))
            player.anim.speed = 2f;
        rb.gravityScale = 0f;
        player.knockFlip = true;
        SkillManager.instance.UseSkillSlot1();
    }
    public override void Exit()
    {
        base.Exit();
        player.knockFlip = false;
        player.anim.speed = 1f;
        MagicSkillAvatars.Instance.RunCooldownAvatar(0, SkillManager.instance.GetSkill1Cooldown());
        SkillManager.instance.RunCdSkill1();
    }
    public override void Update()
    {
        base.Update();
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        if (finishAnim)
        {
            if(player.CheckGrounded() || player.CheckJumpOnSlope())
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }
}
