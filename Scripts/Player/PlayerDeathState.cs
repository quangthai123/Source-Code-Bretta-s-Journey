using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerStates
{
    private bool setActiveDeathUI = false;
    public PlayerDeathState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        SaveManager.instance.tempGameData.deadCount++;
        AudioManager.instance.PlaySFX(11);
        player.isKnocked = true;
        setActiveDeathUI = false;
    }
    public override void Exit()
    {
        base.Exit();
    }


    public override void Update()
    {
        base.Update();
        if (player.CheckGrounded())
            rb.velocity = Vector2.zero;
        if (finishAnim && !setActiveDeathUI) 
        {
            PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.hitGroundedEffect, new Vector2(player.centerEffectPos.position.x - player.facingDir * .8f, player.centerEffectPos.position.y), Quaternion.identity);
            PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.soulDeadFx, player.transform.position, Quaternion.identity);
            if(player.playerStats.haveDied)
            {
                player.SpawnPlayerFakeSoul();
                if(PlayerSoulController.instance != null)
                    PlayerSoulController.instance.gameObject.SetActive(false);
            }
            else
                player.SpawnPlayerSoulAfterDead();
            setActiveDeathUI = true;
            PlayScreenUI.instance.ActiveDeathUI();
        }
    }
}
