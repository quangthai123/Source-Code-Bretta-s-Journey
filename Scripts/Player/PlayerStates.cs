using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates
{
    protected string animBoolName;
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected Rigidbody2D rb;
    protected bool finishAnim;
    protected float horizontalInput;
    protected float verticalInput;
    protected float stateDuration;
    public PlayerStates(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    public virtual void Start()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        if(SceneScenarioSelectLv.instance != null)
        {
            if(!SceneScenarioSelectLv.instance.isScenario)
                rb.gravityScale = 6f;
        }
        player.knockFlip = false;
    }
    public virtual void Update()
    {
        if (stateMachine.currentState != this)
            return;
        //if (LoadingScene.instance.gameObject.activeInHierarchy)
        //    return;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        stateDuration -= Time.deltaTime;
        if (player.CheckSlope())
        {
            if(player.stateMachine.currentState != player.runState)
            {
                rb.sharedMaterial = player.onSlopePhysicMat;
            }
        }
        else
        {
            rb.sharedMaterial = player.normalPhysicMat;
        }
        if ((!player.CheckGrounded() || player.CheckStandOnLedge()) && !player.CheckGetOutSlope())
            MakeEnableNormalCol(false);
        else
            MakeEnableNormalCol(true);
        ChangeStateByInput();
    }
    public virtual void FixedUpdate()
    {
        if (stateMachine.currentState != this)
            return;
    }
    public virtual void LateUpdate()
    {
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
        finishAnim = false;
        if (SceneScenarioSelectLv.instance == null)
            rb.gravityScale = 6f;
        if (SceneScenarioSelectLv.instance != null)
        {
            if (SceneScenarioSelectLv.instance.isScenario)
                rb.gravityScale = 6f;
        }
        player.knockFlip = false;
    }
    public void SetFinishAnimation()
    {
        finishAnim = true;
    }
    protected virtual void ChangeStateByInput()
    {

    }
    protected void MakeEnableNormalCol(bool _enabled)
    {
        player.normalCol.GetComponent<CapsuleCollider2D>().enabled = _enabled; // => slope col
        player.normalCol.GetComponent<BoxCollider2D>().enabled = !_enabled;
    }
}
