using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill0 : Skill
{
    [SerializeField] private float offsetXWithPlayerPos;
    public override void UseSkill(bool canIncreaseDamageBySlot2)
    {
        base.UseSkill(canIncreaseDamageBySlot2);
        skillSpawned = PlayerSkillSpawner.Instance.Spawn(PlayerSkillSpawner.Instance.skill0_Wood, new Vector2(Player.Instance.transform.position.x + Player.Instance.facingDir * offsetXWithPlayerPos, Player.Instance.transform.position.y), Quaternion.identity);
        if (canIncreaseDamageBySlot2)
            skillSpawned.GetComponent<CanDamageEnemy>().damage *= 1.2f;
        //cooldownTimer = cooldown;
    }
}
