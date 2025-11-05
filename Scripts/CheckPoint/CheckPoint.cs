using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : NPC
{
    public static CheckPoint instance;
    private Animator anim;
    private GameDatas tempGameData;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        anim = GetComponent<Animator>();
        tempGameData = Resources.Load<GameDatas>("TempGameData");
    }
    protected override void Start()
    {
        base.Start();
        targetPosX = transform.position.x;
    }
    protected override void OnInteract()
    {
        base.OnInteract();
        showInteractImage.SetActive(false);
        Invoke("SetSaveGame", 1f);
    }
    private void SetSaveGame()
    {
        anim.SetBool("Saving", true);
        Invoke("StartFadeIn", 1f);
    }
    private void StartFadeIn()
    {
        LoadingScene.instance.StartFadeIn(1 / 2f, false);
        Invoke("SaveGame", .75f);
    }
    private void SaveGame()
    {
        Debug.Log("Saved Game!");
        player.playerStats.Resting();
        tempGameData.tempCurrentScene = SceneManager.GetActiveScene().name;
        tempGameData.currentScene = tempGameData.tempCurrentScene;
        tempGameData.initializePos = player.transform.position;
        tempGameData.revivalCheckPointPos = transform.position;
        SaveManager.instance.SaveGame();
        Invoke("SetOutRest", 1.5f);
    }
    private void SetOutRest()
    {
        LoadingScene.instance.StartFadeOut(1 / 2f);
        // them tuy chon dich chuyen ve lang va dich chuyen den cac check point khac
        Invoke("SetPlayerOutRest", 1.5f);
    }
    private void SetPlayerOutRest()
    {
        player.restState.SetOutRest();
        //showInteractImage.SetActive(true);
    }
    private void SetSavingFalse()
    {
        anim.SetBool("Saving", false);
    }
    public void ShowInteractImage()
    {
        if (showInteractImage.activeInHierarchy)
            return;
        CanvasGroup canvasGroup = showInteractImage.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        showInteractImage.SetActive(true);
        StartCoroutine(ShowInteractImageFadeFx(canvasGroup));
    }
    private IEnumerator ShowInteractImageFadeFx(CanvasGroup cg)
    {
        while (cg.alpha < 1f)
        {
            yield return new WaitForSecondsRealtime(.1f);
            cg.alpha += .2f;
        }
        cg.alpha = 1f;
    }
}
