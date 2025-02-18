using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Attack2Skill : MonoBehaviour
{
    public int facingDir;
    private BoxCollider2D boxCol;
    private Boss1 boss1; 
    public static int leftSkillCount = 0;
    public static int rightSkillCount = 0;
    private void OnEnable()
    {
        boss1 = Boss1.Instance;
        boxCol = GetComponent<BoxCollider2D>();
        //boxCol.enabled = false;
        //Invoke("ActiveBoxColAfterEnable", .3f);
        if(boss1.canResetSkillCountForAttack2)
        {
            boss1.canResetSkillCountForAttack2 = false;
            leftSkillCount = 0;
            rightSkillCount = 0;
        }
        if ((facingDir == 1 && rightSkillCount > 10) || (facingDir == -1 && leftSkillCount > 10))
            return;
        Invoke("SpawnNextSkillByCurrentAnimFrame", .3f);
    }
    private void Update()
    {
        //transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0f));
    }
    private void SpawnNextSkillByCurrentAnimFrame()
    {
        //if (spawnNextOne)
        //    return;
        if(facingDir == -1)
        {
            BossEffectSpawner.Instance.Spawn(BossEffectSpawner.Instance.Boss1_Attack2EffectLeft, new Vector3(transform.position.x - 3f, transform.position.y, 0f), Quaternion.identity);
            leftSkillCount++;
        }
        else
        {
            BossEffectSpawner.Instance.Spawn(BossEffectSpawner.Instance.Boss1_Attack2EffectRight, new Vector3(transform.position.x + 3f, transform.position.y, 0f), Quaternion.identity);
            rightSkillCount++;  
        }
    } 
    private void ActiveBoxColAfterEnable()
    {
        boxCol.enabled = true;
    }
}
