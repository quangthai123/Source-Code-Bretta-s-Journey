using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider easeBar;
    private PlayerStats playerStats;
    void Start()
    {
        playerStats = Player.Instance.playerStats;
        healthBar.maxValue = playerStats.maxHealth.GetValue();
        easeBar.maxValue = playerStats.maxHealth.GetValue();
        healthBar.value = playerStats.currentHealth;
        easeBar.value = healthBar.value;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.maxValue = playerStats.maxHealth.GetValue();
        healthBar.value = playerStats.currentHealth;
        easeBar.maxValue = playerStats.maxHealth.GetValue();
        if (healthBar.value != easeBar.value)
            easeBar.value = Mathf.MoveTowards(easeBar.value, healthBar.value, 20f * Time.deltaTime);
    }
}
