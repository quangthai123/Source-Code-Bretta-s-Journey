using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    private EnemiesActiveInScene enemiesThisScene;
    public static EnemiesManager Instance;
    public bool[] enemieBoolList = new bool[30];
    public List<Transform> enemiesList = new List<Transform>();
    private int sceneIndex;
    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

    }
    private void Start()
    {
        enemiesThisScene = EnemiesActiveInScene.instance;
        Debug.Log(enemiesThisScene.name);
        sceneIndex = SceneIndexManager.Instance.sceneIndex;
        int cnt = 0;
        foreach (Transform enemy in transform)
        {
            enemy.gameObject.SetActive(false);
            enemiesList.Add(enemy);
            enemieBoolList[cnt] = false;
            cnt++;
        }
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesThisScene.enabledEnemyList[sceneIndex, i])
            {
                enemiesList[i].gameObject.SetActive(true);
                enemieBoolList[i] = true;
            }
        }
    }
    private void Update()
    {
        //if (enemiesCount != fixedEnemyQuantity)
        //{
            //CheckEnemyDied();
        //    fixedEnemyQuantity = enemiesCount;
        //}
    }
    public void CheckEnemyDied()
    {
        for(int i=0; i<enemiesList.Count; i++)
        {
            if (!enemiesList[i].gameObject.activeInHierarchy)
            {
                Debug.Log("Deactive");
                enemieBoolList[i] = false;
            } else
            {
                enemieBoolList[i] = true;
            }
        }
        enemiesThisScene.SaveEnemiesQuantityBeforeChangeScene(sceneIndex, enemieBoolList);
    }
}
