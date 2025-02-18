using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBGAnimationController : MonoBehaviour
{
    [SerializeField] private GameObject reviveButtonBG;
    [SerializeField] private GameObject reviveButton;
    private void Start()
    {
        reviveButtonBG.SetActive(false);
        reviveButton.SetActive(false);
    }
    private void ActiveReviveButton()
    {
        reviveButtonBG.SetActive(true);
        reviveButton.SetActive(true);
    }
}
