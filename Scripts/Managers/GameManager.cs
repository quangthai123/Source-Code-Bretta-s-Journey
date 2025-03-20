using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Player player;
    private GameDatas tempGameData;
    private EnemiesActiveInScene enemiesAllScene;

    private bool LoadUIComponentBeforeDeactive;
    public bool pressedResetGame { get; private set; }
    private float playedTimeFloat;
    [Header("UI")]
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GameObject placeUI;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject controlUI;
    [SerializeField] private GameObject currencyUI;

    private CinemachineImpulseSource screenShake;
    [Header("Screen Shake")]
    public Vector3 earthQuake;
    public Vector3 strongEarthQuake;
    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        SetActiveTrueInventoryUIToLoad();
    }
    private void Start()
    {
        player = Player.Instance;
        tempGameData = Resources.Load<GameDatas>("TempGameData");
        enemiesAllScene = EnemiesActiveInScene.instance;
        screenShake = GetComponent<CinemachineImpulseSource>();
        if (tempGameData.tempCurrentScene == tempGameData.soulScene)
        {
            Instantiate(player.playerSoul, tempGameData.soulPos, Quaternion.identity);
        }
        pressedResetGame = false;
        LoadUIComponentBeforeDeactive = false;
        playedTimeFloat = tempGameData.playedTime;
        if (placeUI != null)
        {
            placeUI.SetActive(false);
        }
    }
    private void Update()
    {
        playedTimeFloat += Time.deltaTime;
        tempGameData.playedTime = (int)playedTimeFloat;
    }
    private void LateUpdate()
    {
        if (!LoadUIComponentBeforeDeactive)
        {
            LoadUIComponentBeforeDeactive = true;
            inventoryUI.gameObject.SetActive(false);
        }
    }
    public void SetActiveTrueInventoryUIToLoad() => inventoryUI.gameObject.SetActive(true);
    public void ShowAdToRevive()
    {
        Debug.Log("Watch ad to revive!");
        Observer.Notify(GameEvent.OnWatchingAd, RewardType.Revive);
    }
    public void OnClickWatchRewardAdToEarnDemonBlood()
    {
        Debug.Log("Watch ad to earn demon blood!");
        Observer.Notify(GameEvent.OnWatchingAd, RewardType.EarnDemonBlood);
    }
    public void ReviveByRewardedAd()
    {
        if (PlayerFakeSoul.instance != null)
            Destroy(PlayerFakeSoul.instance.gameObject);
        if (player.playerStats.haveDied && PlayerSoulController.instance != null && tempGameData.tempCurrentScene == tempGameData.soulScene)
        {
            PlayerSoulController.instance.gameObject.SetActive(true);
        }
        else if (!player.playerStats.haveDied && PlayerSoulController.instance != null)
        {
            Destroy(PlayerSoulController.instance.gameObject);
            tempGameData.soulPos = Vector2.zero;
            tempGameData.soulScene = string.Empty;
            tempGameData.currencySoul = 0;
        }
        player.playerStats.currentHealth = player.playerStats.maxHealth.GetValue() / 2;
        tempGameData.currentHealth = (int)player.playerStats.currentHealth;
        player.playerStats.Refill2FlaskByRevive();
        PlayScreenUI.instance.DeactiveDeathUI();
        player.ReviveState();

    }
    public void ResetGame()
    {
        pressedResetGame = true;
        Observer.Notify(GameEvent.OnResetGame, null); // PlayScreenUI

        tempGameData.currentMana = 0;
        tempGameData.initializePos = tempGameData.revivalCheckPointPos;
        tempGameData.haveDied = true;
        tempGameData.soulScene = tempGameData.tempCurrentScene;
        tempGameData.soulPos = new Vector2(player.transform.position.x - player.facingDir, player.transform.position.y);
        tempGameData.tempCurrentScene = tempGameData.currentScene;
        tempGameData.currencySoul = player.playerStats.currency;
        tempGameData.currency = 0;

        player.playerStats.currency = 0;
        player.playerStats.haveDied = true;

        enemiesAllScene.ReviveAllEnemy();

        SaveManager.instance.SaveGame();
    }
    private void LoadScene() => SceneManager.LoadScene(tempGameData.currentScene);
    public void BackToMainMenu()
    {
        LoadingScene.instance.gameObject.SetActive(true);
        LoadingScene.instance.FadeIn();
        StartCoroutine(LoadMenuScene());
    }
    private IEnumerator LoadMenuScene()
    {
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(.5f);
        SceneManager.LoadSceneAsync(0);
    }
    public void OpenInventory()
    {
        inventoryUI.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void CloseInventory()
    {
        inventoryUI.CloseAllLoreUI();
        inventoryUI.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ShowPlaceUI()
    {
        placeUI.SetActive(true);
    }
    public void HideAllInGameUI()
    {
        mainUI.SetActive(false);
        currencyUI.SetActive(false);
        controlUI.SetActive(false);
    }
    public void ShowAllInGameUI()
    {
        mainUI.SetActive(true);
        currencyUI.SetActive(true);
        controlUI.SetActive(true);
    }
    public void CreateScreenShakeFx(Vector3 value)
    {
        screenShake.m_DefaultVelocity = value;
        screenShake.GenerateImpulse();
    }
    //public void StrongEarthQuake()
    //{
    //    screenShake.m_DefaultVelocity = earthQuake_Boss1 * 2f;
    //    screenShake.GenerateImpulse();
    //}
}
