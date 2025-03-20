using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillBlood : Skill
{
    [SerializeField] private float attackRadius;
    public override void UseSkill(bool canIncreaseDamageBySlot2)
    {
        base.UseSkill(canIncreaseDamageBySlot2);
        UseSkillBlood(canIncreaseDamageBySlot2);
        //cooldownTimer = cooldown;
    }
    private void UseSkillBlood(bool canIncreaseDamageBySlot2)
    {
        Debug.Log("Use Blood Skill");
        Collider2D[] hits = Physics2D.OverlapCircleAll(Player.Instance.transform.position, attackRadius, LayerMask.GetMask("Enemy"));
        if(hits.Length < 1)
        {
            SpawnBloodOnPos(Player.Instance.transform.position + new Vector3(Player.Instance.facingDir * 4f, .15f), canIncreaseDamageBySlot2);
            return;
        }
        Debug.Log("Detect Enemy: " + hits.Length + " numbers");
        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponentInParent<Enemy>() != null)
            {
                Debug.Log("Attack on: " + hit.transform.parent.name);
                SpawnBloodOnPos(hit.transform.parent.position, canIncreaseDamageBySlot2);
            }
        }
    }
    private void SpawnBloodOnPos(Vector2 pos, bool canIncreaseDamageBySlot2)
    {
        skillSpawned = PlayerSkillSpawner.Instance.Spawn(PlayerSkillSpawner.Instance.skillMulti_Blood, pos, Quaternion.identity);
        if (canIncreaseDamageBySlot2)
            skillSpawned.GetComponent<CanDamageEnemy>().damage *= 1.2f;
    }
}
