using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPortal : MonoBehaviour
{
    private GameDatas tempGameData;
    public string nextSceneName;
    [SerializeField] private Vector2 nextSceneInitializePos;
    [SerializeField] private int nextFacingDir;
    private GameObject tutorialImage;
    private bool canEnterLv = false;
    private void Awake()
    {
        tempGameData = Resources.Load<GameDatas>("TempGameData");
    }
    void Start()
    {
        tutorialImage = transform.Find("TutorialImage").gameObject;
        tutorialImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(canEnterLv && InputManager.Instance.moveDir.y == -1)
        {
            canEnterLv = false;
            LoadingScene.instance.StartFadeIn(1 / 6f, false);
            Debug.LogWarning("Load!!!");
            tempGameData.currentSwordLv = Player.Instance.playerStats.swordLv;
            tempGameData.initializePos = nextSceneInitializePos;
            tempGameData.facingDir = nextFacingDir;
            tempGameData.tempCurrentScene = nextSceneName;
            SceneManager.LoadSceneAsync(nextSceneName);
            //Invoke("LoadScene", .5f);
        }
    }
    private void LoadScene()
    {
        SceneManager.LoadSceneAsync(nextSceneName);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            canEnterLv = true;
            tutorialImage.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canEnterLv = false;
            tutorialImage.SetActive(false);
        }
    }
}
