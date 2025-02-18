using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargingState : PlayerStates
{
    private Coroutine flashFxCorou;
    public PlayerChargingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        if (!SaveManager.instance.tempGameData.learnedSkill[5])
            stateDuration = 1.5f;
        else
            stateDuration = .75f;
        flashFxCorou = null;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        rb.velocity = Vector3.zero;
    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if(stateDuration < 0 && flashFxCorou == null)
        {
            flashFxCorou = player.StartCoroutine(player.GetComponent<EntityFx>().FlashFX());
            PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.strongGroundedEffect, player.centerEffectPos.position, Quaternion.identity);
        }
        if (!InputManager.Instance.holdingAttackBtn && !Input.GetKey(KeyCode.K))
        {
            if(stateDuration > 0) 
                stateMachine.ChangeState(player.attackState);
            else
                stateMachine.ChangeState(player.chargedAttackState);
        }
    }
}
