using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillSpawner : Spawner
{
    public static PlayerSkillSpawner Instance;
    public string skill0_Wood = "Skill0_Wood";
    public string skillMulti_Blood = "SkillMulti_Blood";
    protected override void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        base.Awake();
    }
    public override Transform Spawn(string name, Vector2 pos, Quaternion rot)
    {
        base.Spawn(name, pos, rot);
        if (obj.GetComponent<SkillDespawn>() != null)
        {
            if (obj.GetComponent<SkillDespawn>().canFlip && obj.GetComponent<SkillDespawn>().wrongDirWhenSpawn && Player.Instance.facingDir == 1)
                obj.GetComponent<SkillDespawn>().Flip();
            else if (obj.GetComponent<SkillDespawn>().canFlip && !obj.GetComponent<SkillDespawn>().wrongDirWhenSpawn && Player.Instance.facingDir == -1)
                obj.GetComponent<SkillDespawn>().Flip();
        }
        obj.gameObject.SetActive(true);
        return obj;
    }
}
