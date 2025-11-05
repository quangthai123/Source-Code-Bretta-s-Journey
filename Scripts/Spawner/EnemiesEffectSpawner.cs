using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesEffectSpawner : Spawner
{
    public static EnemiesEffectSpawner Instance { get; private set; }
    public string enemy1_groundedFx = "enemy1_groundedFx";
    protected override void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    public override Transform Spawn(string name, Vector2 pos, Quaternion rot)
    {
        base.Spawn(name, pos, rot);
        obj.gameObject.SetActive(true);
        return obj;
    }
}
