using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwordSkillUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI swordLvText;
    [SerializeField] private Image swordImage;
    [SerializeField] private List<Sprite> swordSprites;
    [SerializeField] private TextMeshProUGUI swordSkillNameText;
    [SerializeField] private TextMeshProUGUI swordSkillDescriptionText;
    [SerializeField] private TextMeshProUGUI swordSkillTutorialText;
    [SerializeField] private TextMeshProUGUI swordSkillUpgradeCostText;
    [SerializeField] private Image demonBloodImage;
    [SerializeField] private Transform learnBtn;
    [SerializeField] private Transform learnFailedNoti;
    [Header("Skill Tree")]
    [SerializeField] private SwordSkillSO[] swordSkillData;
    [SerializeField] private Sprite upgradedSkillImage;
    [SerializeField] private List<Transform> skillSlots;
    [SerializeField] private List<Transform> links;
    [SerializeField] private Transform selector;
    [SerializeField] private SwordSkillPreview swordSkillPreview;
    private GameDatas tempGameDatas;
    private int currentSelectedSkillIndex;
    private void Awake()
    {
        selector.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        tempGameDatas = Resources.Load<GameDatas>("TempGameData");
        LoadSwordLv();
        LoadCanLearnSkill();
        LoadLearnedSkill();
    }
    //private void Update()
    //{
    //    if (int.Parse(swordLvText.text) != tempGameDatas.currentSwordLv)
    //        LoadSwordLv();
    //}
    private void LoadSwordLv()
    {
        swordLvText.text = tempGameDatas.currentSwordLv + "";
        swordImage.sprite = swordSprites[tempGameDatas.currentSwordLv];
    }
    private void LoadCanLearnSkill()
    {
        int swordLv = tempGameDatas.currentSwordLv;
        for (int i = 0; i < skillSlots.Count; i++)
        {
            if (swordSkillData[i].needingSwordLevel <= swordLv) 
            {
                skillSlots[i].Find("LockingSkill").gameObject.SetActive(false);
            }
        }
    }
    private void LoadLearnedSkill()
    {
        for (int i = 0; i < tempGameDatas.learnedSkill.Count; i++)
        {
            if (tempGameDatas.learnedSkill[i])
            {
                ActivateSkillSlot(i);
            }
        }
    }
    private void ActivateSkillSlot(int i)
    {
        Transform skillSlot = skillSlots[i];
        skillSlot.Find("BG").gameObject.SetActive(false);
        skillSlot.GetComponent<Image>().sprite = upgradedSkillImage;
        switch (i)
        {
            case 1:
                ActivateLink(0);
                break;
            case 2:
                ActivateLink(1);
                break;
            case 3:
                ActivateLink(2);
                break;
            case 5:
                ActivateLink(3);
                break;
            case 6:
                ActivateLink(4);
                break;
            case 7:
                ActivateLink(5);
                break;
            case 9:
                ActivateLink(6);
                break;
            case 10:
                ActivateLink(7);
                break;
            case 11:
                ActivateLink(8);
                break;
            case 13:
                ActivateLink(9);
                break;
            case 14:
                ActivateLink(10);
                break;
            case 16:
                ActivateLink(11);
                break;
            case 0:
                if (tempGameDatas.learnedSkill[4])
                    ActivateLink(12);
                break;
            case 4:
                if (tempGameDatas.learnedSkill[0])
                    ActivateLink(12);
                if (tempGameDatas.learnedSkill[8])
                    ActivateLink(13);
                break;
            case 8:
                if (tempGameDatas.learnedSkill[4])
                    ActivateLink(13);
                if (tempGameDatas.learnedSkill[12])
                    ActivateLink(14);
                break;
            case 12:
                if (tempGameDatas.learnedSkill[8])
                    ActivateLink(14);
                if (tempGameDatas.learnedSkill[15])
                    ActivateLink(15);
                break;
            case 15:
                if (tempGameDatas.learnedSkill[12])
                    ActivateLink(15);
                if (tempGameDatas.learnedSkill[17])
                    ActivateLink(16);
                break;
            case 17:
                if (tempGameDatas.learnedSkill[15])
                    ActivateLink(16);
                break;
        }
    }
    private void ActivateLink(int i) 
    {
        Image linkColor = links[i].GetComponent<Image>();
        linkColor.color = new Color(linkColor.color.r, linkColor.color.g, linkColor.color.b, 1f);
    }
    public void OnClickSkillTreeSlot(int slotIndex)
    {
        currentSelectedSkillIndex = slotIndex;
        selector.SetParent(skillSlots[slotIndex], true);
        selector.SetAsFirstSibling();
        selector.localPosition = Vector2.zero;
        selector.gameObject.SetActive(true);
        SwordSkillSO swordData = swordSkillData[slotIndex];
        swordSkillNameText.text = swordData.skillName;
        swordSkillDescriptionText.text = swordData.skillDescription;
        swordSkillTutorialText.text = swordData.skillTutorial;
        if(swordData.needingSwordLevel <= tempGameDatas.currentSwordLv)
        {
            swordSkillUpgradeCostText.gameObject.SetActive(true);
            demonBloodImage.gameObject.SetActive(true);
            swordSkillUpgradeCostText.text = swordData.upgradeCost + "";
            //if(!swordSkillPreview.IsPlaying("Testing1"))
            //    swordSkillPreview.PlaySkillPreview("Testing1");
        } else
        {
            demonBloodImage.gameObject.SetActive(false);
            swordSkillUpgradeCostText.gameObject.SetActive(false);
        }
    }
}
