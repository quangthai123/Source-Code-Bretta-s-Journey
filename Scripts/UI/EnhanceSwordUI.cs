using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class EnhanceSwordUI : MonoBehaviour
{
    private static List<int> enhanceSwordLvPrices = new List<int>()
    {
        200, //0 -> 1
        1300, //1 -> 2
        3000, //2 -> 3
        5000, //3 -> 4
        8000, //4 -> 5
        11500, //5 -> 6
        14500 //6 -> 7
    };
    [Range(0, 6)] public int swordStatueId; 
    [SerializeField] private TextMeshProUGUI upgradeCurrencyText;
    [SerializeField] private Image nextSwordLvImage;
    [SerializeField] private TextMeshProUGUI nextSwordLvNameTxt;
    private GameDatas tempGameData;
    private void OnEnable()
    {
        tempGameData = Resources.Load<GameDatas>("TempGameData");
        LoadUpgradedOrNot();
    }
    private void LoadUpgradedOrNot()
    {
        if(tempGameData.upgradedSwordStatues[swordStatueId])
        {
            transform.parent.GetChild(1).gameObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }
        nextSwordLvImage.sprite = Inventory.Instance.allMainSwordSprites[tempGameData.currentSwordLv+1];
        int nextLv = tempGameData.currentSwordLv + 1;
        nextSwordLvNameTxt.text = "Linh Hồn Thánh Kiếm Ahamat +" + nextLv;
        upgradeCurrencyText.text = "-"+enhanceSwordLvPrices[tempGameData.currentSwordLv];
    }
    public void OnClickAbsorb()
    {
        if (tempGameData.currency < enhanceSwordLvPrices[tempGameData.currentSwordLv])
        {
            transform.parent.gameObject.SetActive(false);
            NotificationUI.Instance.SetMessageAndNotify("Không đủ Huyết Quỷ Đông Tụ!");
            return;
        }
        Player.Instance.playerStats.currency -= enhanceSwordLvPrices[tempGameData.currentSwordLv];
        Player.Instance.playerStats.UpdateSwordLv();
        tempGameData.currency = Player.Instance.playerStats.currency;
        tempGameData.upgradedSwordStatues[swordStatueId] = true;
        tempGameData.upgradedSwordLv[swordStatueId] = tempGameData.currentSwordLv;
        NotificationUI.Instance.SetMessageAndNotify("Hấp thụ thành công!", ShowSwordSkillStatue);
        transform.parent.gameObject.SetActive(false);
    }
    public void OnClickEscape() => transform.parent.GetComponent<UIEffect>().OnCloseLayer();
    private void ShowSwordSkillStatue()
    {
        transform.parent.gameObject.SetActive(true);
        transform.parent.GetChild(1).gameObject.SetActive(true);
    }
}
