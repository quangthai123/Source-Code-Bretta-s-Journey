using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesEffectDespawner : MonoBehaviour
{
    private void DespawnAfterFinishAnim()
    {
        EnemiesEffectSpawner.Instance.Despawn(transform);
    }
}
