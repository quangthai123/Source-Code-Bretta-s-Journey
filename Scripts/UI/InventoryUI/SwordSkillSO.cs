using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordSkillData", menuName = "ScriptableObjects/SwordSkillData")]
public class SwordSkillSO : ScriptableObject
{
    public string skillName;
    [TextArea(5, 7)] public string skillDescription;
    public string skillTutorial;
    public int upgradeCost;
    public int previousSkillIndex;
}
