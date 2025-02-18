using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayScreenUI : MonoBehaviour
{
    public static PlayScreenUI instance;
    [SerializeField] private List<GameObject> swordAvatarUIList;
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
    public float currencyValueUI { get; private set; }
    private float tempCurrencyValueUI;
    public bool finishManaResist = false;
    public bool finishDeductCurrencyUI = false;
    [SerializeField] private List<Image> manaFillImageList;
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
    void Update()
    {
        if (Player.Instance.isDead)
            controlUI.SetActive(false);
        else if (SceneScenarioSelectLv.instance == null)
            controlUI.SetActive(true);

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
            tempCurrencyValueUI = currencyValueUI;
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
            LoadingScene.instance.gameObject.SetActive(true);
            LoadingScene.instance.FadeIn();
            Player.Instance.playerStats.Resting();
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
        currencyText.text = (int)currencyValueUI+"";
        yield return new WaitForSeconds(.1f);
        AddCurrencyUI();
    }
    private void AddCurrencyUI() 
    { 
        currencyValueUI += (float)(playerStats.currency - tempCurrencyValueUI) / 10f;
        Debug.Log(currencyValueUI);
        if ((currencyValueUI >= playerStats.currency && playerStats.currency > tempCurrencyValueUI)
            || (currencyValueUI <= playerStats.currency && playerStats.currency < tempCurrencyValueUI)) 
        {
            Debug.Log("startAddCurrencyUICoroutine = null");
            currencyValueUI = playerStats.currency;
            currencyText.text = currencyValueUI + "";
            StopCoroutine(startAddCurrencyUICoroutine);
            startAddCurrencyUICoroutine = null;
            //startAddCurrencyUICoroutine = false;
            finishDeductCurrencyUI = true;
            if(startResistManaCoroutine == null && GameManager.Instance.pressedResetGame)
                finishManaResist = true;
        } else {
            Debug.Log("Continue change currency");
            StartCoroutine(StartChangeCurrencyValueUI());
        }
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
    public void IndicateWhenOutOfManaToUseSkill() 
    {
        if (indicatedNotEnoughMana) 
            return;
        indicatedNotEnoughMana = true;
        foreach(Image i in manaFillImageList) 
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 55f/255f);
        }
        Invoke("BackToNormalColorManaBar", .5f);
    }
    private void BackToNormalColorManaBar() 
    {
        foreach (Image i in manaFillImageList) 
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 255f / 255f);
        }
        indicatedNotEnoughMana = false;
    }
}
