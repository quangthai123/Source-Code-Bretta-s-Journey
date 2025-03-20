using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public int equippedSkillSlot1Index;
    public int equippedSkillSlot2Index;
    public List<Skill> skillList;

    private GameDatas tempGameData;
    private Transform skills;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        tempGameData = Resources.Load<GameDatas>("TempGameData");
        skills = transform.Find("Skills");
        LoadSkills();
        equippedSkillSlot1Index = tempGameData.magicGemEquippedItems[0];
        equippedSkillSlot2Index = tempGameData.magicGemEquippedItems[1];
    }
    private void LoadSkills()
    {
        skillList.Clear();
        foreach(Transform skill in skills)
        {
            skillList.Add(skill.GetComponent<Skill>());
        }
    }
    public bool CanUseSkillSlot1()
    {
        return skillList[equippedSkillSlot1Index].cooldownTimer <= 0 && Player.Instance.playerStats.currentMana >= skillList[equippedSkillSlot1Index].manaToUse;
    }
    public float GetSkill1Cooldown() => skillList[equippedSkillSlot1Index].cooldown;
    public Color GetSkill1BorderColor() => skillList[equippedSkillSlot1Index].avatarBorderColor;
    public bool CanUseSkillSlot2()
    {
        return skillList[equippedSkillSlot2Index].cooldownTimer <= 0 && Player.Instance.playerStats.currentMana >= skillList[equippedSkillSlot2Index].manaToUse;
    }
    public float GetSkill2Cooldown() => skillList[equippedSkillSlot2Index].cooldown;
    public Color GetSkill2BorderColor() => skillList[equippedSkillSlot2Index].avatarBorderColor;
    public void UseSkillSlot1() => skillList[equippedSkillSlot1Index].UseSkill(false);
    public void RunCdSkill1() => skillList[equippedSkillSlot1Index].ResetCd();
    public void RunCdSkill2() => skillList[equippedSkillSlot2Index].ResetCd();
    public void UseSkillSlot2()
    {
        skillList[equippedSkillSlot2Index].UseSkill(true);
    }
    public void SetUsingSkill(int skill, int index)
    {
        if(skill == 0)
            equippedSkillSlot1Index = index;
        else
            equippedSkillSlot2Index = index;
    }
}
