using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManaBar : MonoBehaviour
{
    [SerializeField] private Slider manaBar;
    private PlayerStats playerStats;
    void Start()
    {
        playerStats = Player.Instance.playerStats;
    }

    // Update is called once per frame
    void Update()
    {
        manaBar.maxValue = playerStats.maxMana.GetValue();
        manaBar.value = playerStats.currentMana;
    }
}
