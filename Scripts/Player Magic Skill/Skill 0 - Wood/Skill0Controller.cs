using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill0Controller : CanDamageEnemy
{
    private bool canLaunch;
    private BoxCollider2D boxCol;
    [SerializeField] private float launchSpeed;
    protected override void OnEnable()
    {
        base.OnEnable();
        boxCol = GetComponent<BoxCollider2D>();
        boxCol.enabled = false;
        anim.ResetTrigger("CanBreak");
        anim.ResetTrigger("CanLaunch");
        canLaunch = false;
    }
    void Update()
    {
        if (canLaunch)
        {
            boxCol.enabled = true;
            anim.SetTrigger("CanLaunch");
            anim.speed = 1f;
            rb.linearVelocity = transform.right * launchSpeed;
        }
        else
            rb.linearVelocity = Vector2.zero;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            anim.SetTrigger("CanBreak");
            canLaunch = false;
            boxCol.enabled = false;
        }
    }
    private void SetCanLaunchByAnim() => canLaunch = true;
}
