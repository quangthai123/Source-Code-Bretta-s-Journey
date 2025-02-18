using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectDespawnByAnim : MonoBehaviour
{
    public bool canFlip;
    public bool wrongDirWhenSpawn = false;
    public void Flip() 
    {
        if (gameObject.tag == "CounterAttackFx")
        {
            transform.localRotation = Quaternion.Euler(0f, 180f, 40f);
            return;
        }
        transform.Rotate(0f, 180f, 0f);
    }
    private void DespawnAfterFinishAnim()
    {
        PlayerEffectSpawner.instance.Despawn(transform);
    }
}
