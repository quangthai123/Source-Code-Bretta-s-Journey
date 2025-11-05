using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private Button backToMenuBtn;
    private void Start()
    {
        backToMenuBtn.onClick.AddListener(GameManager.Instance.BackToMainMenu);
    }
}
