using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [Header("Spawn dontDestroyOnLoad GO to test in specific scene")]
    [SerializeField] private Transform enemiesActiveInSceneTransfToTest;
    [SerializeField] private Transform audioManagerTransfToTest;
    [SerializeField] private Transform saveManagerTransfToTest;
    [SerializeField] private Transform skillManagerTransfToTest;
    [SerializeField] private Transform frameRateTransfToTest;
    [SerializeField] private Transform inventoryTransfToTest;
    [SerializeField] private Transform loadSceneCanvasTransfToTest;
    [SerializeField] private Transform playerEffectSpawnerToTest;
    [SerializeField] private Transform enemiesEffectSpawnerTransfToTest;
    [SerializeField] private Transform damageTxtSpawnerTransfToTest;
    [SerializeField] private Transform mobileInputManagerTransfToTest;
    [SerializeField] private Transform notificationUITransfToTest;
    public static GameManager Instance;
    private Player player;
    private GameDatas tempGameData;
    private EnemiesActiveInScene enemiesAllScene;
    public bool pressedResetGame { get; private set; }
    private float playedTimeFloat;
    private InventoryUI inventoryUI;
    //[SerializeField] private GameObject placeUI;

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

        tempGameData = Resources.Load<GameDatas>("TempGameData");
#if UNITY_EDITOR
        SpawnPersistentGO();
#endif
    }
    private void SpawnPersistentGO()
    {
        if(EnemiesActiveInScene.instance == null)
            Instantiate(enemiesActiveInSceneTransfToTest);
        if(AudioManager.instance == null)
            Instantiate(audioManagerTransfToTest);
        if(SaveManager.instance == null)
            Instantiate(saveManagerTransfToTest);
        if(SkillManager.instance == null)
            Instantiate(skillManagerTransfToTest);
        if(FrameRateManager.Instance == null)
            Instantiate(frameRateTransfToTest);
        if(Inventory.Instance == null)
            Instantiate(inventoryTransfToTest);
        if(LoadingScene.instance == null)
            Instantiate(loadSceneCanvasTransfToTest);
        if(PlayerEffectSpawner.instance == null)
            Instantiate(playerEffectSpawnerToTest);
        if(EnemiesEffectSpawner.Instance == null)
            Instantiate(enemiesEffectSpawnerTransfToTest);
        if (DamageTextSpawner.Instance == null)
            Instantiate(damageTxtSpawnerTransfToTest);
        if (InputManager.Instance == null)
            Instantiate(mobileInputManagerTransfToTest);
        //if (InventoryUI.Instance == null && SceneManager.GetActiveScene().name != "MainMenu") 
        //{ 
        //    Instantiate(inventoryUITransfToTest);
        //    Debug.Log("Instantiate InventoryUI");
        //}
        //if (InGameCanvas.Instance == null && SceneManager.GetActiveScene().name != "MainMenu")
        //{
        //    Instantiate(inGameUITransfToTest);
        //    Debug.Log("Instantiate IngameUI");
        //}
        if (NotificationCanvas.Instance == null)
        {
            Instantiate(notificationUITransfToTest);
            Debug.Log("Instantiate NotificationUI");
        }
    }
    private void Start()
    {
        player = Player.Instance;
        enemiesAllScene = EnemiesActiveInScene.instance;
        screenShake = GetComponent<CinemachineImpulseSource>();
        inventoryUI = InventoryUI.Instance;
        if (tempGameData.tempCurrentScene == tempGameData.soulScene)
        {
            Instantiate(player.playerSoul, tempGameData.soulPos, Quaternion.identity);
        }
        pressedResetGame = false;
        playedTimeFloat = tempGameData.playedTime;
        //if (placeUI != null)
        //{
        //    placeUI.SetActive(false);
        //}
        LoadingScene.instance.StartFadeOut(.15f);
        SetPlayerPosBetweenScene();
    }
    private void SetPlayerPosBetweenScene()
    {
        if(player == null)
            return;
        player.gameObject.SetActive(true);
        if (player.stateMachine.currentState != null)
            player.stateMachine.ChangeState(player.idleState);
        else
            player.stateMachine.Initialize(player.idleState);
        player.transform.position = tempGameData.initializePos;
        if (player.facingDir != tempGameData.facingDir)
            player.Flip();
    }
    private void Update()
    {
        playedTimeFloat += Time.deltaTime;
        tempGameData.playedTime = (int)playedTimeFloat;

        if(Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory();
        }
    }
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

        SaveManager.instance.SaveGame();
    }
    private void LoadScene() => SceneManager.LoadScene(tempGameData.currentScene);
    public void BackToMainMenu()
    {
        LoadingScene.instance.StartFadeIn(1 / 6f, true);
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
        if(inventoryUI == null || inventoryUI.gameObject.activeInHierarchy) return;
        inventoryUI.Open();
        Player.Instance.DisablePlayerControl();
        Time.timeScale = 0f;
    }
    public void CloseInventory()
    {
        //inventoryUI.CloseAllLoreUI();
        inventoryUI.Close();
        Player.Instance.EnablePlayerControl();
        Time.timeScale = 1f;
    }
    public void ShowPlaceUI()
    {
        //placeUI.SetActive(true);
    }
    public void HideAllInGameUI()
    {
        InGameCanvas.Instance.gameObject.SetActive(false);
    }
    public void ShowAllInGameUI()
    {
        InGameCanvas.Instance.gameObject.SetActive(true);
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
