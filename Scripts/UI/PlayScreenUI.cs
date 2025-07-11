using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayScreenUI : MonoBehaviour
{
    public static PlayScreenUI instance;
    public List<GameObject> swordAvatarUIList;
    private PlayerStats playerStats;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform manaBar;
    [SerializeField] private TextMeshProUGUI currencyText;
    public RectTransform resistManaBar;
    public RectTransform resistManaBar2;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject showAdBG;
    [SerializeField] private GameObject showAdButton;
    [SerializeField] private GameObject xBG;
    [SerializeField] private GameObject xButton;
    [Space]
    private int currentSwordLv;
    private float amountOfResistManaPerSecond;
    private Coroutine startResistManaCoroutine;
    private Coroutine startBreakResistManaCoroutine;
    public Coroutine startAddCurrencyUICoroutine;
    private bool haveDiedd;
    private float currencyValueUI;
    private float tempCurrencyValueUI;
    public bool finishManaResist = false;
    public bool finishDeductCurrencyUI = false;
    [SerializeField] private RectTransform notEnoughManaIndicator;
    private int fps;
    [SerializeField] private TextMeshProUGUI fpsText;
    private bool indicatedNotEnoughMana = false;
    [SerializeField] private GameObject controlUI;
    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;
        //tempGameData = Resources.Load<GameDatas>("TempGameData");
        foreach (GameObject go in swordAvatarUIList)
        {
            go.SetActive(false);
        }
        deathUI.SetActive(false);
        //amountOfBreakResistManaPerSecond = 2.4f;
        startResistManaCoroutine = null;
        startBreakResistManaCoroutine = null;
        startAddCurrencyUICoroutine = null;
        finishManaResist = false;
        finishDeductCurrencyUI = false;
        resistManaBar.gameObject.SetActive(true);
        resistManaBar2.gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        Observer.AddListener(GameEvent.OnResetGame, this.OnResetGame);
    }
    private void Start()
    {
        playerStats = Player.Instance.playerStats;
        this.currentSwordLv = playerStats.swordLv;
        swordAvatarUIList[this.currentSwordLv].SetActive(true);       
        haveDiedd = SaveManager.instance.tempGameData.haveDied;
        currencyValueUI = playerStats.currency;
        currencyText.text = currencyValueUI + "";
        if (SaveManager.instance.tempGameData.haveDied) 
        {
            resistManaBar.localScale = new Vector3(1f, 2.4f, 1f);
            resistManaBar2.localScale = new Vector3(-1f, 2.4f, 1f);
            amountOfResistManaPerSecond = 2.4f;
        }
        else 
        {
            resistManaBar.localScale = new Vector3(1f, 0, 1f);
            resistManaBar2.localScale = new Vector3(-1f, 0f, 1f);
            amountOfResistManaPerSecond = 0;
        }
        InvokeRepeating("GetFrameRate", .5f, .5f);

    }
    public void HideControlUI() => controlUI.SetActive(false);
    public void ShowControlUI() => controlUI.SetActive(true);
    void Update()
    {
        if (Player.Instance.isDead)
            HideControlUI();
        //else if (SceneScenarioSelectLv.instance == null)
        //    ShowControlUI();

        if (playerStats.haveDied && startResistManaCoroutine == null && resistManaBar.localScale.y < 2.4f)
        {
            Debug.Log("Start Resist Mana");
            startResistManaCoroutine = StartCoroutine(StartResistMana());
        }
        else if (!playerStats.haveDied && this.haveDiedd && startBreakResistManaCoroutine == null && resistManaBar.localScale.y > 0)
        {
            haveDiedd = false;
            startBreakResistManaCoroutine = StartCoroutine(BreakResistMana());
        }
        if (currencyValueUI != playerStats.currency && startAddCurrencyUICoroutine == null)
        {
            Debug.Log("Start Change Currency UI");
            //tempCurrencyValueUI = currencyValueUI;
            startAddCurrencyUICoroutine = StartCoroutine(StartChangeCurrencyValueUI());
        }
        healthBar.localScale = new Vector3(playerStats.maxHealth.GetValue() / playerStats.maxHealth.baseValue, 1f, 1f);
        manaBar.localScale = new Vector3(playerStats.maxMana.GetValue() / playerStats.maxMana.baseValue, 1f, 1f);
        if (this.currentSwordLv != playerStats.swordLv)
        {
            swordAvatarUIList[this.currentSwordLv].SetActive(false);
            swordAvatarUIList[playerStats.swordLv].SetActive(true);
            currentSwordLv = playerStats.swordLv;
        }
        if (finishDeductCurrencyUI && finishManaResist)
        {
            Debug.Log("Load lai scene bang playscreenUI script");
            finishDeductCurrencyUI = false;
            finishManaResist = false;
            LoadingScene.instance.StartFadeIn();
            Player.Instance.playerStats.Resting();
            Player.Instance.isKnocked = true;
            Invoke("LoadScene", .5f);
        }
    }
    private void LoadScene() => SceneManager.LoadScene(SaveManager.instance.tempGameData.currentScene);
    private void GetFrameRate()
    {
        fps = (int)(1f / Time.deltaTime);
        fpsText.text = fps + "";
    }

    private IEnumerator StartChangeCurrencyValueUI() 
    {
        tempCurrencyValueUI = currencyValueUI;
        while ((currencyValueUI < playerStats.currency && playerStats.currency > tempCurrencyValueUI)
            || (currencyValueUI > playerStats.currency && playerStats.currency < tempCurrencyValueUI))
        {
            currencyValueUI += (float)(playerStats.currency - tempCurrencyValueUI) / 20f;
            yield return new WaitForSeconds(.1f);
            currencyText.text = (int)currencyValueUI+"";
        }
        AddCurrencyUI();
    }
    private void AddCurrencyUI() 
    {
        Debug.Log(currencyValueUI);
        currencyValueUI = playerStats.currency;
        currencyText.text = currencyValueUI + "";
        StopCoroutine(startAddCurrencyUICoroutine);
        startAddCurrencyUICoroutine = null;
        finishDeductCurrencyUI = true;
        if (startResistManaCoroutine == null && GameManager.Instance.pressedResetGame)
            finishManaResist = true;
    } 
    public void ActiveDeathUI()
    {
        deathUI.SetActive(true);
    }
    public void DeactiveDeathUI()
    {
        deathUI.SetActive(false);
        showAdBG.SetActive(false);
        showAdButton.SetActive(false); 
        xBG.SetActive(false);
        xButton.SetActive(false);
    }
    private IEnumerator StartResistMana()
    {
        resistManaBar.localScale = new Vector3(1f, amountOfResistManaPerSecond, 1f);
        resistManaBar2.localScale = new Vector3(-1f, amountOfResistManaPerSecond, 1f);
        yield return new WaitForSeconds(.0025f);
        EnhanceAmountOfResistMana();
    }
    private void EnhanceAmountOfResistMana()
    {
        amountOfResistManaPerSecond += .03f;
        if (amountOfResistManaPerSecond >= 2.4f)
        {
            amountOfResistManaPerSecond = 2.4f;
            resistManaBar.localScale = new Vector3(1f, 2.4f, 1f);
            resistManaBar2.localScale = new Vector3(-1f, 2.4f, 1f);
            StopCoroutine(startResistManaCoroutine);
            startBreakResistManaCoroutine = null;
            finishManaResist = true;
            if (startAddCurrencyUICoroutine == null && GameManager.Instance.pressedResetGame)
                finishDeductCurrencyUI = true;
        }
        else
            StartCoroutine(StartResistMana());
    }
    private IEnumerator BreakResistMana()
    {
        resistManaBar.localScale = new Vector3(1f, amountOfResistManaPerSecond, 1f);
        resistManaBar2.localScale = new Vector3(-1f, amountOfResistManaPerSecond, 1f);
        yield return new WaitForSeconds(.0025f);
        DeductAmountOfResistMana();
    }
    private void DeductAmountOfResistMana()
    {
        amountOfResistManaPerSecond -= .03f;
        if (amountOfResistManaPerSecond <= 0)
        {
            amountOfResistManaPerSecond = 0;
            resistManaBar.localScale = new Vector3(1f, 0f, 1f);
            resistManaBar2.localScale = new Vector3(-1f, 0f, 1f);
            StopCoroutine(startBreakResistManaCoroutine);
            startBreakResistManaCoroutine = null;
        }
        else
            StartCoroutine(BreakResistMana());
    }
    public void IndicateWhenOutOfManaToUseSkill(int manaToUse) 
    {
        if (indicatedNotEnoughMana) 
            return;
        indicatedNotEnoughMana = true;
        float scaleX = manaToUse / playerStats.maxMana.GetValue();
        Vector3 localScale = notEnoughManaIndicator.localScale;
        localScale.x = scaleX;
        notEnoughManaIndicator.localScale = localScale;
        notEnoughManaIndicator.gameObject.SetActive(true);
        Invoke("BackToNormalColorManaBar", .15f);
    }
    private void BackToNormalColorManaBar() 
    {
        notEnoughManaIndicator.gameObject.SetActive(false);
        indicatedNotEnoughMana = false;
        notEnoughManaIndicator.localScale = Vector3.one;
    }
    private void OnResetGame(object[] datas)
    {
        DeactiveDeathUI();
        finishManaResist = false;
        finishDeductCurrencyUI = false;
        if (resistManaBar.localScale.y == 2.4f && currencyValueUI == Player.Instance.playerStats.currency) 
        {
            LoadingScene.instance.StartFadeIn();
            playerStats.Resting();
            Player.Instance.isKnocked = true;
            Invoke("LoadScene", .5f);
        }
    }
    private void OnDisable() => Observer.RemoveListener(GameEvent.OnResetGame, this.OnResetGame);
    private void OnDestroy() => Observer.RemoveListener(GameEvent.OnResetGame, this.OnResetGame);
}
