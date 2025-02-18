using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerStates
{
    private Transform startDashFx;
    private int oldFacingDir;
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Start()
    {
        base.Start();
        oldFacingDir = player.facingDir;
        stateDuration = player.dashDuration;
        player.anim.ResetTrigger("DashStop");
        player.normalCol.SetActive(false);
        player.dashCol.SetActive(true);
        AudioManager.instance.PlaySFX(0);
        if (!player.CheckSlope())
            startDashFx = PlayerEffectSpawner.instance.Spawn("endDashFx", player.leftEffectPos.position, Quaternion.identity);
        player.isKnocked = true;
        player.StartSpawnDashShadowFx();
    }
    public override void Exit()
    {
        base.Exit();
        player.normalCol.SetActive(true);
        player.dashCol.SetActive(false);
        player.isKnocked = false;
        player.dashTimer = Time.time;
        player.StopSpawnDashShadowFx();
    }
    public override void Update()
    {
        base.Update();
        if(player.facingDir != oldFacingDir)
        {
            oldFacingDir = player.facingDir;
            startDashFx.Rotate(0f, 180f, 0f);
        }    
        rb.sharedMaterial = player.normalPhysicMat;
        if (!player.CheckGrounded() && !player.CheckGetOutSlope())
        {
            rb.velocity = Vector2.zero;
            stateMachine.ChangeState(player.fallState);
        }
        if(stateDuration <= 0f)
        {
            if (!player.CheckCeilling() && !Input.GetKey(KeyCode.S) && InputManager.Instance.moveDir.y != -1)
                stateMachine.ChangeState(player.endDashState);
            else
                stateMachine.ChangeState(player.crouchState);
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(stateDuration > 0f)
        {
            if (player.CheckGetOutSlope() && player.facingDir == 1)
                rb.velocity = new Vector2(player.dashSpeed * player.facingDir, 0f);
            else if(player.CheckSlope() && stateMachine.currentState != player.jumpState)
                rb.velocity = new Vector2(player.dashSpeed * player.facingDir * -player.slopeMoveDir.x, player.dashSpeed * player.facingDir * -player.slopeMoveDir.y);
            else
                rb.velocity = new Vector2(player.dashSpeed * player.facingDir, rb.velocity.y);
        }
    }
    protected override void ChangeStateByInput()
    {
        base.ChangeStateByInput();
        if (Input.GetKeyDown(KeyCode.Space) || InputManager.Instance.jumped)
            stateMachine.ChangeState(player.jumpState);
        if(Input.GetKeyDown(KeyCode.J) || InputManager.Instance.parried)
            stateMachine.ChangeState(player.shieldState);
        if (InputManager.Instance.CheckCanDashAtk() && SaveManager.instance.tempGameData.learnedSkill[7])
            stateMachine.ChangeState(player.dashAttackState);
        else if(InputManager.Instance.getAttackedBtnUp)
            stateMachine.ChangeState(player.attackState);
        //stateMachine.ChangeState(player.attackState);
    }
}
