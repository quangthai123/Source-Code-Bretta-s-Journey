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
    [SerializeField] private Transform learnBtn;
    [SerializeField] private Transform learnFailedNoti;
    [Header("Skill Tree")]
    [SerializeField] private Sprite upgradedSkillImage;
    [SerializeField] private List<Transform> skillSlots;
    [SerializeField] private List<Transform> links;
    [SerializeField] private Transform selector;
    private SwordSkillSO[] swordSkillData;
    private GameDatas tempGameDatas;
    private int currentSelectedSkillIndex;
    private void Awake()
    {
        tempGameDatas = Resources.Load<GameDatas>("TempGameData");
        swordSkillData = Resources.LoadAll<SwordSkillSO>("SwordSkills");
        LoadSwordLv();
        LoadLearnedSkill();
    }
    private void Update()
    {
        if (int.Parse(swordLvText.text) != tempGameDatas.currentSwordLv)
            LoadSwordLv();
    }
    private void LoadSwordLv()
    {
        swordLvText.text = tempGameDatas.currentSwordLv + "";
        swordImage.sprite = swordSprites[tempGameDatas.currentSwordLv];
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
        skillSlots[i].GetChild(0).gameObject.SetActive(false);
        skillSlots[i].GetComponent<Image>().sprite = upgradedSkillImage;
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
            case 8:
                ActivateLink(5);
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
        selector.localPosition = skillSlots[slotIndex].localPosition;
        selector.gameObject.SetActive(true);
        SwordSkillSO swordData = swordSkillData[slotIndex];
        swordSkillNameText.text = swordData.skillName;
        swordSkillDescriptionText.text = swordData.skillDescription;
        swordSkillTutorialText.text = swordData.skillTutorial;
        swordSkillUpgradeCostText.text = swordData.upgradeCost + "";
        if (tempGameDatas.learnedSkill[slotIndex])
            learnBtn.gameObject.SetActive(false);
        else
            learnBtn.gameObject.SetActive(true);
    }
    public void OnClickLearnBtn()
    {
        if (Player.Instance.playerStats.currency < swordSkillData[currentSelectedSkillIndex].upgradeCost)
        {
            learnFailedNoti.gameObject.SetActive(true);
            learnFailedNoti.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Không đủ Huyết Quỷ Đông Tụ!";
            return;
        }
        else if(swordSkillData[currentSelectedSkillIndex].previousSkillIndex != -1)
        {
            if(!tempGameDatas.learnedSkill[swordSkillData[currentSelectedSkillIndex].previousSkillIndex])
            {
                learnFailedNoti.gameObject.SetActive(true);
                learnFailedNoti.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Yêu cầu kỹ năng trước!";
                return;
            }
        }
        Player.Instance.playerStats.currency -= swordSkillData[currentSelectedSkillIndex].upgradeCost;
        SaveManager.instance.tempGameData.currency = Player.Instance.playerStats.currency;
        learnBtn.gameObject.SetActive(false);
        tempGameDatas.learnedSkill[currentSelectedSkillIndex] = true;
        LoadLearnedSkill();
    }
}
