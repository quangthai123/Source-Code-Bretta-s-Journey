using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates
{
    private string animBoolName;
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    protected Rigidbody2D rb;
    protected float stateDuration;
    protected bool finishAnim = false;
    protected static bool haveAttacked = false;
    protected Player player;
    public EnemyStates(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _enemyStateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Start()
    {
        enemyBase.anim.SetBool(animBoolName, true);
        rb = enemyBase.rb;
        player = Player.Instance;
    }

    public virtual void Update()
    {
        if(stateDuration > 0)
            stateDuration -= Time.deltaTime;
    }
    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        finishAnim = false;
    }
    public void SetFinishAnim()
    {
        finishAnim = true;
    }
    protected void FlipToFacePlayer()
    {
        if ((enemyBase.transform.position.x < player.transform.position.x && enemyBase.facingDir == -1)
            || (enemyBase.transform.position.x > player.transform.position.x && enemyBase.facingDir == 1))
        {
            rb.linearVelocity = Vector2.zero;
            enemyBase.Flip();
        }
    }
    protected void FlipOnDetectWallOrEndGround()
    {
        if (enemyBase.CheckNotFrontGround() || enemyBase.CheckWalled())
        {
            rb.linearVelocity = Vector2.zero;
            enemyBase.Flip();
        }
    }
    protected bool CanConstraintMoveOnDetectWallOrEndGround()
    {
        if (enemyBase.CheckNotFrontGround() || enemyBase.CheckWalled())
        {
            return true;
        }
        return false;
    }
    protected bool CheckPlayerPosThrougWallOrOtherGround()
    {
        if ((enemyBase.transform.position.x < player.transform.position.x && enemyBase.facingDir == 1)
            || (enemyBase.transform.position.x > player.transform.position.x && enemyBase.facingDir == -1))
        {
            if(enemyBase.CheckWalled() || enemyBase.CheckNotFrontGround())
            {
                return true;
            }
        }
        return false;
    }
}
