using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobileInputTesting : MonoBehaviour
{
    public static MobileInputTesting Instance;
    public bool isMobileDevice;
    private GameObject magicBtn;
    private GameDatas tempGameData;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        isMobileDevice = false;
        tempGameData = Resources.Load<GameDatas>("TempGameData");
        magicBtn = transform.Find("MagicSkillButton").gameObject;
        SetStateOfMagicBtn();
    }
    public void SetStateOfMagicBtn()
    {
        if (tempGameData.magicGemEquippedItems[0] != -1)
            magicBtn.SetActive(true);
        else
            magicBtn.SetActive(false);
    }
    public void CanEnableMagicBtnToUseMagicSkill2()
    {
        if (tempGameData.magicGemEquippedItems[1] != -1)
            magicBtn.SetActive(true);
    }
}
