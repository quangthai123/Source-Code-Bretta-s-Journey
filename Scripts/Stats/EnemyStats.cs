using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats: MonoBehaviour
{
    public int currency;
    public float currentHealth;
    public float maxHealth;
    public int damage;
    public int impactDamage;
    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void GetDamageStat(float _damage)
    {
        currentHealth -= _damage;
        if(currentHealth <= 0 )
        {
            currentHealth = 0;
            Died();
        }
    }
    public void Died()
    {
        Debug.Log(gameObject.name + " died!");
        //EnemiesManager.Instance.enemiesCount--;
    }
}
