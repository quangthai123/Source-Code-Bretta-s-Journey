using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LearnSwordSkillUI : MonoBehaviour
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
    [SerializeField] private Transform swordHolder;
    [SerializeField] private GameObject swordSkillInfor;
    //[SerializeField] private Transform swordSelector;
    [Header("Skill Tree")]
    [SerializeField] private SwordSkillSO[] swordSkillData;
    [SerializeField] private List<Transform> skillSlots;
    [SerializeField] private List<Transform> links;
    [SerializeField] private Transform selector;
    [SerializeField] private SwordSkillPreview swordSkillPreview;
    [SerializeField] private TextMeshProUGUI currencyTxt;
    [SerializeField] private TextMeshProUGUI learnPriceTxt;
    private GameDatas tempGameData;
    private PlayerStats playerStats;
    private int currentSelectedSkillIndex;
    private Image openingSkillImg;
    private bool isLearning;
    private Coroutine learningCorou;
    private float currencyValueUI;
    private float tempCurrencyValueUI;
    private void Awake()
    {
        learnBtn.gameObject.SetActive(false);
        swordSkillInfor.SetActive(false);
    }
    private void OnEnable()
    {
        tempGameData = Resources.Load<GameDatas>("TempGameData");
        currencyValueUI = tempGameData.currency;
        currencyTxt.text = tempGameData.currency.ToString();
        playerStats = Player.Instance.playerStats;
        LoadCanLearnSkill();
        LoadLearnedSkill();
        LoadSword();
    }
    private void LoadCanLearnSkill()
    {
        int swordLv = tempGameData.currentSwordLv;
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
        for (int i = 0; i < tempGameData.learnedSkill.Count; i++)
        {
            if (tempGameData.learnedSkill[i])
            {
                ActivateSkillSlot(i);
            }
        }
    }
    private void LoadSword()
    {
        int statueId = transform.parent.GetChild(0).GetComponent<EnhanceSwordUI>().swordStatueId;
        int swordLv = tempGameData.upgradedSwordLv[statueId];
        swordImage.sprite = swordSprites[swordLv];
        swordLvText.text = swordLv.ToString();
    }
    private void ActivateSkillSlot(int i)
    {
        Transform skillSlot = skillSlots[i];
        skillSlot.Find("LockingSkill").gameObject.SetActive(false);
        skillSlot.Find("Opened").GetComponent<Image>().fillAmount = 1;
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
                if (tempGameData.learnedSkill[4])
                    ActivateLink(12);
                break;
            case 4:
                if (tempGameData.learnedSkill[0])
                    ActivateLink(12);
                if (tempGameData.learnedSkill[8])
                    ActivateLink(13);
                break;
            case 8:
                if (tempGameData.learnedSkill[4])
                    ActivateLink(13);
                if (tempGameData.learnedSkill[12])
                    ActivateLink(14);
                break;
            case 12:
                if (tempGameData.learnedSkill[8])
                    ActivateLink(14);
                if (tempGameData.learnedSkill[15])
                    ActivateLink(15);
                break;
            case 15:
                if (tempGameData.learnedSkill[12])
                    ActivateLink(15);
                if (tempGameData.learnedSkill[17])
                    ActivateLink(16);
                break;
            case 17:
                if (tempGameData.learnedSkill[15])
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
        if (isLearning)
            return;
        currentSelectedSkillIndex = slotIndex;
        selector.SetParent(skillSlots[slotIndex], true);
        selector.SetSiblingIndex(selector.parent.childCount-2);
        selector.localPosition = Vector2.zero;
        selector.localScale = Vector2.one;
        selector.gameObject.SetActive(true);
        swordHolder.GetChild(0).gameObject.SetActive(false);
        SwordSkillSO swordData = swordSkillData[slotIndex];
        swordSkillNameText.text = swordData.skillName;
        swordSkillDescriptionText.text = swordData.skillDescription;
        swordSkillTutorialText.text = swordData.skillTutorial;
        swordSkillInfor.SetActive(true);
        if (swordData.needingSwordLevel <= tempGameData.currentSwordLv)
        {
            swordSkillNameText.transform.parent.gameObject.SetActive(true);
            learnBtn.gameObject.SetActive(true);
        }
        else
        {
            swordSkillNameText.transform.parent.gameObject.SetActive(false);
            learnBtn.gameObject.SetActive(false);
        }
        if (tempGameData.learnedSkill[slotIndex])
        {
            learnBtn.gameObject.SetActive(false);
            openingSkillImg = null;
        }
        else
        {
            if (swordData.needingSwordLevel <= tempGameData.currentSwordLv)
                learnBtn.gameObject.SetActive(true);
            learnPriceTxt.text = "-"+swordData.upgradeCost;
            openingSkillImg = skillSlots[slotIndex].Find("Opened").GetComponent<Image>();
        }
    }
    public void OnPressingLearnBtn()
    {
        if (Player.Instance.playerStats.currency < swordSkillData[currentSelectedSkillIndex].upgradeCost)
        {
            learnFailedNoti.gameObject.SetActive(true);
            learnFailedNoti.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Không đủ Huyết Quỷ Đông Tụ!";
            return;
        }
        else if (swordSkillData[currentSelectedSkillIndex].previousSkillIndex != -1)
        {
            if (!tempGameData.learnedSkill[swordSkillData[currentSelectedSkillIndex].previousSkillIndex])
            {
                learnFailedNoti.gameObject.SetActive(true);
                learnFailedNoti.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Yêu cầu kỹ năng trước!";
                return;
            }
        }
        if(openingSkillImg != null && learningCorou == null)
        {
            isLearning = true;
            StopAllCoroutines();
            learningCorou = StartCoroutine(OpeningSkill());
            selector.gameObject.SetActive(false);
        }
    }
    private IEnumerator OpeningSkill()
    {
        while(openingSkillImg.fillAmount < 1)
        {
            yield return new WaitForSecondsRealtime(.05f);
            openingSkillImg.fillAmount += .05f;
        }
        openingSkillImg.fillAmount = 1;
        selector.gameObject.SetActive(true);
        isLearning = false;
        learningCorou = null;
        LearnSkill();
    }
    public void OnReleaseLearnBtn()
    {
        Debug.Log("Cancel Learn skill");
        StopAllCoroutines();
        learningCorou = null;
        StartCoroutine(CloseSkill());
    }
    private IEnumerator CloseSkill()
    {
        while (openingSkillImg.fillAmount > 0)
        {
            yield return new WaitForSecondsRealtime(.05f);
            openingSkillImg.fillAmount -= .1f;
        }
        openingSkillImg.fillAmount = 0;
        selector.gameObject.SetActive(true);
        isLearning = false;
    }
    public void LearnSkill()
    {
        Player.Instance.playerStats.currency -= swordSkillData[currentSelectedSkillIndex].upgradeCost;
        tempGameData.currency = Player.Instance.playerStats.currency;
        StartCoroutine(StartChangeCurrencyValueUI());
        learnBtn.gameObject.SetActive(false);
        tempGameData.learnedSkill[currentSelectedSkillIndex] = true;
        ActivateSkillSlot(currentSelectedSkillIndex);
        if (currentSelectedSkillIndex == 17)
            MagicSkillUI.Instance.LockGemSlot2Image.SetActive(false);
    }
    public void OnClickEscape()
    {
        if(!isLearning)
            transform.parent.GetComponent<UIEffect>().OnCloseLayer();
    }
    private IEnumerator StartChangeCurrencyValueUI()
    {
        tempCurrencyValueUI = currencyValueUI;
        while ((currencyValueUI < playerStats.currency && playerStats.currency > tempCurrencyValueUI)
            || (currencyValueUI > playerStats.currency && playerStats.currency < tempCurrencyValueUI))
        {
            currencyValueUI += (float)(playerStats.currency - tempCurrencyValueUI) / 20f;
            yield return new WaitForSeconds(.1f);
            currencyTxt.text = (int)currencyValueUI + "";
        }
        AddCurrencyUI();
    }
    private void AddCurrencyUI()
    {
        Debug.Log(currencyValueUI);
        currencyValueUI = playerStats.currency;
        currencyTxt.text = currencyValueUI + "";
    }
    public void OnClickSword()
    {
        selector.SetParent(swordHolder, true);
        selector.localPosition = new Vector2(0f, -5.08f);
        selector.localScale = new Vector2(1.1f, 1.1f);
        selector.gameObject.SetActive(true);
        swordHolder.GetChild(0).gameObject.SetActive(true);
        learnBtn.gameObject.SetActive(false);

        swordSkillInfor.SetActive(true);
        //swordSelector.localPosition = new Vector2(swordSelector.localPosition.x, swordHolder.GetChild(index).localPosition.y);
        //swordSelector.gameObject.SetActive(true);
    }
}
