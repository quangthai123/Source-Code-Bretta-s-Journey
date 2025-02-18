using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobileInputTesting : MonoBehaviour
{
    public static MobileInputTesting Instance;
    public bool isMobileDevice;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        isMobileDevice = true;
        transform.Find("MagicSkillButton").gameObject.SetActive(false);
    }
    private void Start()
    {
        if (SaveManager.instance.tempGameData.learnedSkill[9])
            transform.Find("MagicSkillButton").gameObject.SetActive(true);
    }
    private void Update()
    {
        if (SaveManager.instance.tempGameData.learnedSkill[9] && !transform.Find("MagicSkillButton").gameObject.activeSelf)
            transform.Find("MagicSkillButton").gameObject.SetActive(true);
    }
}
