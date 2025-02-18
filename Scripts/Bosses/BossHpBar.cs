using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private EnemyStats bossStats;
    [SerializeField] private List<GameObject> boundingBossPhase;
    private void Start()
    {
        hpBar.maxValue = bossStats.maxHealth;
    }
    void Update()
    {
        hpBar.value = bossStats.currentHealth;
        //if(bossStats.currentHealth <= 0)
        //{
        //    foreach(GameObject go in boundingBossPhase)
        //    {
        //        go.SetActive(false);
        //    }
        //    gameObject.SetActive(false);
        //}
    }

}
