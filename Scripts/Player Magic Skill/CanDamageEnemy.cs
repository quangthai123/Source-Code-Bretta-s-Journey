using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CanDamageEnemy : MonoBehaviour
{
    [SerializeField] protected int attackWeight = 0;
    public float damage;
    [SerializeField] protected float damageFactor;
    [SerializeField] protected bool canSpeedUpWithPlayerStats;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected virtual void OnEnable()
    {      
        rb = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();
        damage = Player.Instance.playerStats.damage.baseValue * damageFactor;
        if (Player.Instance.playerStatsWithItems.CheckEquippedPerfectSword(8) 
            && !Player.Instance.playerStatsWithItems.CheckActivatedSwordPair(1))
            damage *= 1.15f;
        if (Player.Instance.playerStatsWithItems.CheckActivatedSwordPair(1))
        {
            damage *= 1.25f;
        }
        if (!canSpeedUpWithPlayerStats)
            return;
        if (Player.Instance.playerStatsWithItems.CheckEquippedPerfectSword(12))
            anim.speed = 1.75f;
        else
            anim.speed = 1f;
        if(Player.Instance.playerStatsWithItems.CheckActivatedSwordPair(1))
        {
            anim.speed = 2f;
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Damage Enemy By " + gameObject.name + ": " + damage);
            Enemy target = collision.GetComponentInParent<Enemy>();
            Vector2 spawnPoint = collision.ClosestPoint(transform.position);
            target.GetDamage(attackWeight, damage, spawnPoint);
            //Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            //PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attackImpactFx2, spawnPoint, randomRotation);
        }
    }
}
