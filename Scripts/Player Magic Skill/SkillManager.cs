using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public int equippedSkillNum;
    public List<Skill> skillList;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
    public bool CanUseSkill()
    {
        return skillList[equippedSkillNum].cooldownTimer <= 0 && Player.Instance.playerStats.currentMana >= skillList[equippedSkillNum].manaToUse;
    }
    public void UseSkill()
    {
        skillList[equippedSkillNum].UseSkill();
    }
}
