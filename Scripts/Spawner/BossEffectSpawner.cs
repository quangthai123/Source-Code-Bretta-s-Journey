using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffectSpawner : Spawner
{
    public static BossEffectSpawner Instance { get; private set; }
    [Header("Boss1_Beleth")]
    public string boss1_heavyGroundedFx = "Boss1_HeavyGroundedFx";
    //public string boss1_skillFx = "boss1_skillFx";
    public string Boss1_Attack2EffectLeft = "Boss1_Attack2EffectLeft";
    public string Boss1_Attack2EffectRight = "Boss1_Attack2EffectRight";
    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public override Transform Spawn(string name, Vector2 pos, Quaternion rot)
    {
        base.Spawn(name, pos, rot);
        obj.gameObject.SetActive(true); 
        return obj;
    }
}
