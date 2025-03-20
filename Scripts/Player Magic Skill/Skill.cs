using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public float cooldown;
    public float cooldownTimer;
    public int manaToUse;
    public Color avatarBorderColor;
    protected Transform skillSpawned;
    protected virtual void Update()
    {
        if(cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;
    }
    public virtual void UseSkill(bool canIncreaseDamageBySlot2)
    {
        Player.Instance.playerStats.DeductManaByMagicSkill(manaToUse);
    }
    public void ResetCd() => cooldownTimer = cooldown;
}
