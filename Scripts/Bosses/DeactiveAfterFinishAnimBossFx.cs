using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveAfterFinishAnimBossFx : MonoBehaviour
{
    private void DeactiveOnFinishAnim()
    {
        BossEffectSpawner.Instance.Despawn(transform);
    }
}
