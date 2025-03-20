using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBloodController : CanDamageEnemy
{
    [SerializeField] private Transform target;
    protected override void OnEnable()
    {
        base.OnEnable();
        target = null;
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Enemy"));
        if(hit && !hit.GetComponentInParent<Enemy>().isDead)
        {
            target = hit.transform.parent;
        }
        //if (target == null)
        //    transform.position = Player.Instance.transform.position + Vector3.right * Player.Instance.facingDir * 2f;
    }
    private void AttackTrigger()
    {
        if(target != null)
        {
            target.GetComponent<Enemy>().GetDamage(attackWeight, damage, target.transform.position);
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attackImpactFx2, target.transform.position, randomRotation);
        }
    }
    private void Update()
    {
        if(target == null)
            return;
        if (target.gameObject.activeInHierarchy)
        {
            transform.position = target.position;
        }
    }
}
