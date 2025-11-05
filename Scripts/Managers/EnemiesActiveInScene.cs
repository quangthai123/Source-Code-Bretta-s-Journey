using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesActiveInScene : MonoBehaviour
{
    public static EnemiesActiveInScene instance;
    public bool[,] enemiesInAllScene = new bool[300, 30];
    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
        DontDestroyOnLoad(gameObject);
        ReviveAllEnemy();
    }
    public void ReviveAllEnemy()
    {
        for (int i = 0; i < 300; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                enemiesInAllScene[i, j] = true;
            }
        }
    }
    public void SaveDeadEnemy(int _sceneIndex, int enemyIndex)
    {
        enemiesInAllScene[_sceneIndex, enemyIndex] = false;
    }
}

