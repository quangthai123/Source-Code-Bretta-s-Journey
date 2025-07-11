using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirDashState : PlayerStates
{
    public bool airDashed = false;
    private float hoverBeforeTimer = 0f;
    //private float hoverDuration = .15f;
    //private float hoverTimer;
    private bool startDash = false;
    public PlayerAirDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        //Time.timeScale = .1f;
        stateDuration = 0f;
        rb.gravityScale = 0f;
        hoverBeforeTimer = .09f;
        //hoverTimer = hoverDuration;
        player.isKnocked = true;
        AudioManager.instance.PlaySFX(4);
    }
    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = 4f;
        airDashed = true;
        startDash = false;
        player.isKnocked = false;
        rb.velocity = new Vector2(0f, rb.velocity.y);
        player.StopSpawnDashShadowFx();
        //player.anim.ResetTrigger("StopAirDash");
        //Time.timeScale = 1f;
    }
    public override void Update()
    {
        base.Update();
        if(hoverBeforeTimer > 0)
        {
            rb.velocity = Vector2.zero;
            hoverBeforeTimer -= Time.deltaTime;
        }
        else if(!startDash)
        {
            startDash = true;
            stateDuration = player.dashDuration * .5f;
            player.StartSpawnDashShadowFx(player.spawnAirDashShadowCooldown);
            //if ()
            //{
                PlayerEffectSpawner.instance.Spawn("airDashFx", player.leftEffectPos.position + new Vector3(0f, 1.2f, 0f), Quaternion.identity);
                Transform effect2 = PlayerEffectSpawner.instance.Spawn("airDashFx", player.leftEffectPos.position + new Vector3(-1 / 3f * player.facingDir, 1.2f, 0f), Quaternion.identity);
                effect2.localScale = new Vector2(0.2666667f, 1f);
            //}
        }
        if (stateDuration < 0f && startDash)
        {
            //if (hoverTimer > 0f)
            //{
            //    if (hoverTimer == hoverDuration)
            //    {
            //        player.anim.SetTrigger("StopAirDash");
            //        player.StopSpawnDashShadowFx();
            //    }
            //    hoverTimer -= Time.deltaTime;
            //    rb.velocity = new Vector2(horizontalInput * player.moveSpeed * .5f, 0f);
            //    if (InputManager.Instance.moveDir.x != 0)
            //        rb.velocity = new Vector2(InputManager.Instance.moveDir.x * player.moveSpeed * .5f, 0f);
            //    rb.gravityScale = 0.5f;
            //}
            //else
                stateMachine.ChangeState(player.fallState);
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(stateDuration > 0)
            rb.velocity = new Vector2(player.dashSpeed * 1.75f * player.facingDir, 0f);
    }

    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (player.CheckWalled() && !player.CheckGrounded() && (Input.GetKeyDown(KeyCode.K) || InputManager.Instance.attacked))
            stateMachine.ChangeState(player.wallSlideState);
        if (Input.GetKeyDown(KeyCode.Space) && !player.doubleJumped && stateDuration < 0f && startDash)
        {
            player.doubleJumped = true;
            stateMachine.ChangeState(player.doubleJumpState);
        }
    }
}
