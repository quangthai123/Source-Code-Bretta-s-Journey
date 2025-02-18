using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    public static CheckPoint instance;
    private Animator anim;
    [SerializeField] private bool canSaveGame;
    private bool isSaving;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        canSaveGame = false;
        isSaving = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Can Save Game!");
            canSaveGame = true;
        }
        //else if (collision.gameObject.tag == "Player" && SceneScenarioSelectLv.instance != null &&
        //    !SceneScenarioSelectLv.instance.isScenario)
        //{
        //    Debug.Log("Auto Save on start new game");
        //    SaveGameByCheckPoint();
        //}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canSaveGame = false;
        }
    }
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    SceneManager.LoadScene(0);
        //}
        if(canSaveGame && !isSaving)
        {
            if (Input.GetKeyDown(KeyCode.E) || InputManager.Instance.attacked)
            {
                SaveGameByCheckPoint();
            }
        }
    }

    public void SaveGameByCheckPoint()
    {
        Debug.Log("Saved Game!");
        isSaving = true;
        anim.SetBool("Saving", true);
        Player.Instance.playerStats.Resting();
        SaveManager.instance.tempGameData.tempCurrentScene = SceneManager.GetActiveScene().name;
        SaveManager.instance.tempGameData.currentScene = SaveManager.instance.tempGameData.tempCurrentScene;
        SaveManager.instance.tempGameData.initializePos = Player.Instance.transform.position;
        SaveManager.instance.tempGameData.revivalCheckPointPos = transform.position;
        SaveManager.instance.SaveGame();
    }
    private void SetSavingFalse()
    {
        anim.SetBool("Saving", false);
        isSaving = false;
    }
}
