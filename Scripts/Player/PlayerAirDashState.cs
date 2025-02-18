using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirDashState : PlayerStates
{
    public bool airDashed = false;
    public PlayerAirDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        rb.gravityScale = 0f;
        stateDuration = player.dashDuration;
        if (!player.CheckGrounded())
        {
            PlayerEffectSpawner.instance.Spawn("airDashFx", player.leftEffectPos.position + new Vector3(0f, 1.2f, 0f), Quaternion.identity);
            Transform effect2 = PlayerEffectSpawner.instance.Spawn("airDashFx", player.leftEffectPos.position + new Vector3(-1 / 3f * player.facingDir, 1.2f, 0f), Quaternion.identity);
            effect2.localScale = new Vector2(0.2666667f, 1f);
        }
        player.isKnocked = true;
        Debug.Log("Enter airDash State!!");
        player.StartSpawnDashShadowFx();
    }
    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = 6f;
        airDashed = true;
        player.isKnocked = false;
        player.StopSpawnDashShadowFx();
    }
    public override void Update()
    {
        base.Update();
        if (stateDuration < 0f)
        {
            Debug.Log("airDash => fall");
            stateMachine.ChangeState(player.fallState);
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        rb.velocity = new Vector2(player.dashSpeed * player.facingDir, 0f);
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (player.CheckWalled() && !player.CheckGrounded() && (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked))
            stateMachine.ChangeState(player.wallSlideState);
    }
}
