using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveButtonAnimationController : MonoBehaviour
{
    [SerializeField] private GameObject xButtonBG;
    [SerializeField] private GameObject xButton;
    [SerializeField] private GameObject normalGiveUpText;
    [SerializeField] private GameObject warningGiveUpText;
    private void Awake()
    {
        xButtonBG.SetActive(false);
        xButton.SetActive(false);
    }
    private void ActiveXButton()
    {
        xButtonBG.SetActive(true);
        if (!Player.Instance.playerStats.haveDied)
        {
            normalGiveUpText.SetActive(true);
            warningGiveUpText.SetActive(false);
        } else
        {
            normalGiveUpText.SetActive(false);
            warningGiveUpText.SetActive(true);
        }
        xButton.SetActive(true);
    }
}
