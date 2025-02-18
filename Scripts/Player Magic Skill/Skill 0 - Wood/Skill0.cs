using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill0 : Skill
{
    [SerializeField] private float offsetXWithPlayerPos;
    public override void UseSkill()
    {
        base.UseSkill();
        PlayerSkillSpawner.Instance.Spawn(PlayerSkillSpawner.Instance.skill0_Wood, new Vector2(Player.Instance.transform.position.x + Player.Instance.facingDir * offsetXWithPlayerPos, Player.Instance.transform.position.y), Quaternion.identity);
        cooldownTimer = cooldown;
    }
}
