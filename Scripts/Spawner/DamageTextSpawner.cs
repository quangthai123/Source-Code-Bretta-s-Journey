using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextSpawner : Spawner
{
    public static DamageTextSpawner Instance { get; private set; }
    public string DamageTextName = "DamageTextCanvas";
    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public override Transform Spawn(string name, Vector2 pos, Quaternion rot)
    {
        base.Spawn(name, pos, rot);
        obj.gameObject.SetActive(true);
        return obj;
    }
}
