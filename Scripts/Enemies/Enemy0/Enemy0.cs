using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy0 : Enemy
{
    public Enemy0_IdleState idleState { get; private set; }
    public Enemy0_DeathState deathState { get; private set; }
    public Enemy0_WalkState walkState { get; private set; }
    //public bool canMove = true;
    public bool canDamageByAnim = false;
    protected override void Awake()
    {
        base.Awake();
        idleState = new Enemy0_IdleState(this, stateMachine, "Idle", this);
        deathState = new Enemy0_DeathState(this, stateMachine, "Dead", this);
        walkState = new Enemy0_WalkState(this, stateMachine, "Walk", this);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        stateMachine.Initialize(idleState);
        facingDir = 1;
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        facingDir = 1;
    }
    protected override void CheckDeath()
    {
        base.CheckDeath();
        if (isOver && stateMachine.currentState != deathState)
            stateMachine.ChangeState(deathState);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;
        if (!(CheckWalled() || CheckNotFrontGround()))
            return;
        if (player.stateMachine.currentState != player.dashState && player.stateMachine.currentState != player.airDashState)
        {
            canDamageByAnim = true;
            Debug.Log("Damage By Anim");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            canDamageByAnim = false;
    }
}
