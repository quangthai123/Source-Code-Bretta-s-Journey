using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_AnimationController : EnemyAnimationController
{
    private Boss1 boss1;
    [SerializeField] private Transform spawnPos;
    protected override void Start()
    {
        base.Start();
        boss1 = GetComponentInParent<Boss1>();
        spawnPos = GetComponentInParent<Boss1>().spawnSkillAttack2Pos;
    }
    private void SpawnAttack2Skill()
    {
        Transform leftEffect = BossEffectSpawner.Instance.Spawn(BossEffectSpawner.Instance.Boss1_Attack2EffectLeft, new Vector2(spawnPos.position.x - 2.67f, spawnPos.position.y), Quaternion.identity);
        //leftEffect.position = new Vector2(spawnPos.position.x - 2.67f, spawnPos.position.y);
        Transform rightEffect = BossEffectSpawner.Instance.Spawn(BossEffectSpawner.Instance.Boss1_Attack2EffectRight, new Vector2(spawnPos.position.x + 2.67f, spawnPos.position.y), Quaternion.identity);
        //rightEffect.position = new Vector2(spawnPos.position.x + 2.67f, spawnPos.position.y);
    }
}
