using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_WolfAnimationController : EnemyAnimationController
{
    private Enemy1_Wolf enemy;
    protected override void Start()
    {
        base.Start();
        enemy = GetComponentInParent<Enemy1_Wolf>();
    }
    private void JumpUpAttackTrigger()
    {
        enemy.transform.Find("Col Trigger").gameObject.layer = LayerMask.NameToLayer("Can Collide Player");
        if (!enemy.CheckNotFrontGround()) 
            enemy.rb.linearVelocity = new Vector2(enemy.attackForce.x * enemy.facingDir, enemy.attackForce.y);
        else 
            enemy.rb.linearVelocity = new Vector2(0f, enemy.attackForce.y);
    }
}
