using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    private EnemyStats enemyStats;
    public GameObject hpBar;
    [SerializeField] private Slider enemyHpSlider;
    private Coroutine deactiveHpBarCorou;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyHpSlider.maxValue = enemyStats.maxHealth;
        hpBar.gameObject.SetActive(false);
    }
    void Update()
    {
        enemyHpSlider.value = enemyStats.currentHealth;
        if (enemyStats.currentHealth <= 0)
            hpBar.gameObject.SetActive(false);
    }
    public void ShowHpBarOnBeDamaged()
    {
        hpBar.gameObject.SetActive(true);
        if(deactiveHpBarCorou != null)
        {
            StopCoroutine(deactiveHpBarCorou);
            deactiveHpBarCorou = null;
        }
        deactiveHpBarCorou = StartCoroutine(DeactiveHpBarAfter(2.5f));
    }
    private IEnumerator DeactiveHpBarAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        hpBar.gameObject.SetActive(false);
        deactiveHpBarCorou = null;
    } 
}
