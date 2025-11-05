using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public List<Skill> skillList;

    private GameDatas tempGameData;
    private Transform skills;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        DontDestroyOnLoad(gameObject);
        tempGameData = Resources.Load<GameDatas>("TempGameData");
        skills = transform.Find("Skills");
        LoadSkills();
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
        return skillList[tempGameData.magicGemEquippedItems[0]].cooldownTimer <= 0 && Player.Instance.playerStats.currentMana >= skillList[tempGameData.magicGemEquippedItems[0]].manaToUse;
    }
    public float GetSkill1Cooldown() => skillList[tempGameData.magicGemEquippedItems[0]].cooldown;
    public Color GetSkill1BorderColor() => skillList[tempGameData.magicGemEquippedItems[0]].avatarBorderColor;
    public bool CanUseSkillSlot2()
    {
        return skillList[tempGameData.magicGemEquippedItems[1]].cooldownTimer <= 0 && Player.Instance.playerStats.currentMana >= skillList[tempGameData.magicGemEquippedItems[1]].manaToUse;
    }
    public float GetSkill2Cooldown() => skillList[tempGameData.magicGemEquippedItems[1]].cooldown;
    public Color GetSkill2BorderColor() => skillList[tempGameData.magicGemEquippedItems[1]].avatarBorderColor;
    public void UseSkillSlot1() => skillList[tempGameData.magicGemEquippedItems[0]].UseSkill(false);
    public void RunCdSkill1() => skillList[tempGameData.magicGemEquippedItems[0]].ResetCd();
    public void RunCdSkill2() => skillList[tempGameData.magicGemEquippedItems[1]].ResetCd();
    public void UseSkillSlot2()
    {
        skillList[tempGameData.magicGemEquippedItems[1]].UseSkill(true);
    }
    public void SetUsingSkill(int skill, int index)
    {
        if (skill == 0)
            tempGameData.magicGemEquippedItems[0] = index;
        else
            tempGameData.magicGemEquippedItems[1] = index;
    }
    public int GetManaToUse(int skillIndex)
    {
        int result = 0;
        switch(skillIndex)
        {
            case 1:
                result = skillList[tempGameData.magicGemEquippedItems[1]].manaToUse;
                break;
            default:
                result = skillList[tempGameData.magicGemEquippedItems[0]].manaToUse;
                break;
        }
        return result;
    }
}
