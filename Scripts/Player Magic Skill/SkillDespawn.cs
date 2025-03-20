using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDespawn : MonoBehaviour
{
    public bool canFlip;
    public bool canDespawbyTime;
    public bool wrongDirWhenSpawn = false;
    public float despawnTime;
    private float despawnTimeCounter;
    private void OnEnable()
    {
        despawnTimeCounter = despawnTime;
    }
    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }
    public void Update()
    {
        if(despawnTimeCounter > 0)
            despawnTimeCounter -= Time.deltaTime;
        if(despawnTimeCounter <= 0 && canDespawbyTime)
            PlayerSkillSpawner.Instance.Despawn(transform);
    }
    private void DespawnAfterFinishAnim()
    {
        PlayerSkillSpawner.Instance.Despawn(transform);
    }
}
