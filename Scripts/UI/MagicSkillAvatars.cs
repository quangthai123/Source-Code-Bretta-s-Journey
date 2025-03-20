using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSkillAvatars : MonoBehaviour
{
    public static MagicSkillAvatars Instance { get; private set; }
    private List<Transform> skillAvatars = new List<Transform>();
    private GameDatas tempGameData;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        tempGameData = Resources.Load<GameDatas>("TempGameData");
        skillAvatars.Add(transform.GetChild(0));
        skillAvatars.Add(transform.GetChild(1));
    }
    private void Start()
    {     
        LoadSkillAvatars();
    }
    public void LoadSkillAvatars()
    {
        skillAvatars[0].gameObject.SetActive(false);
        skillAvatars[1].gameObject.SetActive(false);
        if (tempGameData.magicGemEquippedItems[0] != -1)
        {
            skillAvatars[0].GetChild(2).GetComponent<Image>().sprite = Inventory.Instance.GetSpriteByItemIndex(ItemType.MagicGem, tempGameData.magicGemEquippedItems[0]);
            skillAvatars[0].GetChild(0).GetComponent<Image>().color = SkillManager.instance.GetSkill1BorderColor();
            skillAvatars[0].gameObject.SetActive(true);
        }
        if (tempGameData.magicGemEquippedItems[1] != -1)
        {
            skillAvatars[1].GetChild(2).GetComponent<Image>().sprite = Inventory.Instance.GetSpriteByItemIndex(ItemType.MagicGem, tempGameData.magicGemEquippedItems[1]);
            skillAvatars[1].GetChild(0).GetComponent<Image>().color = SkillManager.instance.GetSkill2BorderColor();
            skillAvatars[1].gameObject.SetActive(true);
        }
    }
    public void RunCooldownAvatar(int skillNum, float cd)
    {
        Transform skillAvatar = skillAvatars[skillNum];
        skillAvatar.GetChild(0).GetComponent<Image>().fillAmount = 0f;
        skillAvatar.GetChild(1).GetComponent<Image>().fillAmount = 0f;
        skillAvatar.GetChild(2).GetComponent<Image>().fillAmount = 0f;
        StartCoroutine(RunCd(skillAvatar, cd));
    }
    private IEnumerator RunCd(Transform skillAvatar, float cd)
    {
        Image borderImage = skillAvatar.GetChild(0).GetComponent<Image>();
        Image bgImage = skillAvatar.GetChild(1).GetComponent<Image>();
        Image skillImage = skillAvatar.GetChild(2).GetComponent<Image>();
        while (borderImage.fillAmount < 1)
        {
            borderImage.fillAmount += 1f/cd * Time.deltaTime;
            bgImage.fillAmount += 1f / cd * Time.deltaTime;
            skillImage.fillAmount += 1f / cd * Time.deltaTime;
            yield return null;
        }
        DoSkillAvatarFx(skillAvatars.IndexOf(skillAvatar));
    }
    public void DoSkillAvatarFx(int skillNum)
    {
        skillAvatars[skillNum].GetComponent<Animator>().ResetTrigger("Indicate");
        skillAvatars[skillNum].GetComponent<Animator>().SetTrigger("Indicate");
    }
}
