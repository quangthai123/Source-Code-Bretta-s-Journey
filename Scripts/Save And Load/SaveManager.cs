using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public GameDatas tempGameData;
    [SerializeField] private string fileNameSlot1;
    [SerializeField] private string fileNameSlot2;
    [SerializeField] private string fileNameSlot3;
    private FileDataHandler dataHandlerSlot1;
    private FileDataHandler dataHandlerSlot2;
    private FileDataHandler dataHandlerSlot3;
    private EnemiesActiveInScene enemiesThisScene;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        tempGameData = Resources.Load<GameDatas>("TempGameData");
        dataHandlerSlot1 = new FileDataHandler(Application.persistentDataPath, fileNameSlot1);
        dataHandlerSlot2 = new FileDataHandler(Application.persistentDataPath, fileNameSlot2);
        dataHandlerSlot3 = new FileDataHandler(Application.persistentDataPath, fileNameSlot3);
    }
    private void Start()
    {
        enemiesThisScene = EnemiesActiveInScene.instance;
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //    gameData1.SetNewGame();
    }
    public bool CheckHadSavedData(int _slot)
    {
        if(_slot == 1 && dataHandlerSlot1.Load() != null)
        {
            tempGameData.GetDataFrom(dataHandlerSlot1.Load());
            return true;
        }       
        else if(_slot == 2 && dataHandlerSlot2.Load() != null)
        {
            tempGameData.GetDataFrom(dataHandlerSlot2.Load());
            return true;
        }
        else if(_slot==3 && dataHandlerSlot3.Load() != null)
        {
            tempGameData.GetDataFrom(dataHandlerSlot3.Load());
            return true;
        }
        return false;
    }
    public void StartGameData1()
    {
        if (dataHandlerSlot1.Load() != null)
        {
            tempGameData.GetDataFrom(dataHandlerSlot1.Load());
        }
        else
        {
            Debug.Log("Set New Game!!!!!");
            tempGameData.SetNewGame(1);
        }
        ReviveAllEnemy();
        tempGameData.currentHealth = (int)tempGameData.maxHealth.GetValue();
        tempGameData.currentMana = 0;
        tempGameData.fullHealFlaskQuantity = tempGameData.flaskQuantity;
        LoadingScene.instance.gameObject.SetActive(true);
        LoadingScene.instance.FadeIn();
        Invoke("LoadScene", 1f);
        //SceneManager.LoadScene(tempGameData.currentScene);
    }
    private void LoadScene()
    {
        SceneManager.LoadSceneAsync(tempGameData.currentScene);
    }
    public void DeleteSaveSlot1()
    {
        dataHandlerSlot1.DeleteSavedFile();
        Debug.Log("Deleted Save Slot 1");
    }
    public void StartGameData2()
    {
        if (dataHandlerSlot2.Load() != null)
            tempGameData.GetDataFrom(dataHandlerSlot2.Load());
        else
        {
            Debug.Log("Set New Game!!!!!");
            tempGameData.SetNewGame(2);
        }
        ReviveAllEnemy();
        tempGameData.currentHealth = (int)tempGameData.maxHealth.GetValue();
        tempGameData.currentMana = 0;
        tempGameData.fullHealFlaskQuantity = tempGameData.flaskQuantity;
        LoadingScene.instance.gameObject.SetActive(true);
        LoadingScene.instance.FadeIn();
        Invoke("LoadScene", 1f);
        //SceneManager.LoadScene(tempGameData.currentScene);
    }
    public void DeleteSaveSlot2()
    {
        dataHandlerSlot2.DeleteSavedFile();
        Debug.Log("Deleted Save Slot 2");
    }
    public void StartGameData3()
    {
        if (dataHandlerSlot3.Load() != null)
            tempGameData.GetDataFrom(dataHandlerSlot3.Load());
        else
        {
            Debug.Log("Set New Game!!!!!");
            tempGameData.SetNewGame(3);
        }
        ReviveAllEnemy();
        tempGameData.currentHealth = (int)tempGameData.maxHealth.GetValue();
        tempGameData.currentMana = 0;
        tempGameData.fullHealFlaskQuantity = tempGameData.flaskQuantity;
        LoadingScene.instance.gameObject.SetActive(true);
        LoadingScene.instance.FadeIn();
        Invoke("LoadScene", 1f);
        //SceneManager.LoadScene(tempGameData.currentScene);
    }
    public void DeleteSaveSlot3()
    {
        dataHandlerSlot3.DeleteSavedFile();
        Debug.Log("Deleted Save Slot 3");
    }
    public void ReviveAllEnemy()
    {
        enemiesThisScene.ReviveAllEnemy();
    }
    public void SaveGame()
    {
        ReviveAllEnemy();
        foreach(Transform enemy in EnemiesManager.Instance.enemiesList)
        {
            enemy.gameObject.SetActive(true);
        }
        switch(tempGameData.saveSlot)
        {
            case 1:
            { 
                dataHandlerSlot1.Save(tempGameData); break;
            }
            case 2:
            { 
                dataHandlerSlot2.Save(tempGameData); break;
            }
            case 3:
            {
                dataHandlerSlot3.Save(tempGameData); break;
            }
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
