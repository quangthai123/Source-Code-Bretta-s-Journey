using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivatedSwordPairNoti : InventoryNotificationUI
{
    [SerializeField] private Transform pairImageHolder;
    [SerializeField] private Image sword1Image;
    [SerializeField] private Image sword2Image;
    [SerializeField] private TextMeshProUGUI sword1NameTxt;
    [SerializeField] private TextMeshProUGUI sword2NameTxt;
    [SerializeField] private TextMeshProUGUI sword1Buff;
    [SerializeField] private TextMeshProUGUI sword2Buff;
    protected override IEnumerator CloseNoti()
    {
        while (canvasGroup.alpha > 0f)
        {
            yield return new WaitForSecondsRealtime(showNotiDuration / 20f);
            canvasGroup.alpha -= .1f;
        }
        SwordPieceUI.Instance.SetSwordPieceUIInteractable();
        gameObject.SetActive(false);
    }
    public void SetSword1Infor(PerfectSwordSO sword1)
    {
        sword1Image.sprite = sword1.image;
        sword1NameTxt.text = sword1.swordName;
        sword1Buff.text = sword1.effectPairFunctionForNoti;
    }
    public void SetSword2Infor(PerfectSwordSO sword2)
    {
        sword2Image.sprite = sword2.image;
        sword2NameTxt.text = sword2.swordName;
        sword2Buff.text = sword2.effectPairFunctionForNoti;
    }
    public void SetPairIndex(int pairIndex)
    {
        foreach(Transform pairImage in pairImageHolder)
        {
            pairImage.gameObject.SetActive(false);
        }
        pairImageHolder.GetChild(pairIndex).gameObject.SetActive(true);
    }
}
