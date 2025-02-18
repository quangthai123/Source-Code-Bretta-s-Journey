using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CanDamageEnemy : MonoBehaviour
{
    [SerializeField] protected int attackWeight = 0;
    [SerializeField] protected float damage;
    [SerializeField] protected float damageFactor;
    [SerializeField] protected float launchSpeed;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected virtual void OnEnable()
    {      
        rb = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();
        if (Player.Instance.playerStatsWithItems.CheckEquippedPerfectSword(12))
            anim.speed = 1.75f;
        else
            anim.speed = 1f;
        damage = Player.Instance.playerStats.damage.baseValue * damageFactor;
        if (Player.Instance.playerStatsWithItems.CheckEquippedPerfectSword(8) 
            && !Player.Instance.playerStatsWithItems.CheckActivatedSwordPair(1))
            damage *= 1.15f;
        if(Player.Instance.playerStatsWithItems.CheckActivatedSwordPair(1))
        {
            anim.speed = 2f;
            damage *= 1.25f;
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Damage Enemy By " + gameObject.name + ": " + damage);
            collision.GetComponentInParent<Enemy>().GetDamage(attackWeight, damage);
        }
    }
}
