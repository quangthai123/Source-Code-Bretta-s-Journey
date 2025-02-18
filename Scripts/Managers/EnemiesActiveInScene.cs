using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesActiveInScene : MonoBehaviour
{
    public static EnemiesActiveInScene instance;
    public bool[,] enabledEnemyList = new bool[300, 30];
    void Awake()
    {
        if(instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
        for(int i=0; i<300; i++)
        {
            for (int j=0; j<30; j++)
            {
                enabledEnemyList[i, j] = true;
            }
        }
        DontDestroyOnLoad(gameObject);
    }
    public void ReviveAllEnemy()
    {
        for (int i = 0; i < 300; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                enabledEnemyList[i, j] = true;
            }
        }
    }
    public void SaveEnemiesQuantityBeforeChangeScene(int _sceneIndex, bool[] _arr)
    {
        for(int i = 0; i < 30; i++)
        {
            enabledEnemyList[_sceneIndex, i] = _arr[i];
        }
    }
}
