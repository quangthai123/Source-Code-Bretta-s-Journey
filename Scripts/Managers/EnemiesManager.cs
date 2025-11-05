using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager Instance;
    [SerializeField] private bool[] enemieBoolList = new bool[30];
    public List<Transform> enemiesList = new List<Transform>();
    private EnemiesActiveInScene enemiesThisScene;
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
            if (enemiesThisScene.enemiesInAllScene[sceneIndex, i])
            {
                enemiesList[i].gameObject.SetActive(true);
                enemieBoolList[i] = true;
            }
        }
    }
    public void SaveDeadEnemy(Transform enemy)
    {
        if (enemiesList.Contains(enemy))
        {
            int enemyIndex = enemiesList.IndexOf(enemy);
            enemiesThisScene.SaveDeadEnemy(sceneIndex, enemyIndex);
            enemieBoolList[enemyIndex] = false;
        }
    }
}
