using UnityEngine;

public class ItemShadowEffectSpawner : Spawner
{
    public static ItemShadowEffectSpawner Instance { get; private set; }
    public float spawnShadowCd = .02f;
    public string ShadowName = "ItemShadow";
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
