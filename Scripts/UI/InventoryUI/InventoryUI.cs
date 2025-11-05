using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    [SerializeField] private Transform[] tabs;
    [SerializeField] private GameObject[] tabsUI;
    private bool loadedInventoryUI;

    [SerializeField] private int selectedTab;
    [SerializeField] private Transform selector;
    [SerializeField] private List<GameObject> newItemImagesOnTabs;

    private Transform rightArrow;
    private Transform leftArrow;
    private Coroutine rightArrowCorou;
    private Coroutine leftArrowCorou;

    [SerializeField] private Transform horizontalSelectTab;
    [SerializeField] private Transform closeInventoryBtn;
    private FadeEffectHandler fadeEffectHandler;

    public bool FrezeeInventoryAction { get; set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        //DontDestroyOnLoad(gameObject);
        loadedInventoryUI = false;
        rightArrow = transform.Find("Right Arrow");
        leftArrow = transform.Find("Left Arrow");
        rightArrowCorou = null;
        leftArrowCorou = null;
        FrezeeInventoryAction = false;
        fadeEffectHandler = GetComponent<FadeEffectHandler>();
        InitializeAllInventoryUI();
    }
    public void InitializeAllInventoryUI()
    {
        Debug.Log("So Tabs UI " + tabsUI.Length);
        foreach (GameObject tabUI in tabsUI)
        {
            Debug.Log("Init Tab UI " + tabUI.name);
            tabUI.SetActive(true);
        }
    }
    private void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Q) && !FrezeeInventoryAction)
            ClickLeftArrow();
        if(Input.GetKeyDown(KeyCode.E) && !FrezeeInventoryAction)
            ClickRightArrow();
        if(Input.GetKeyDown(KeyCode.Escape) && !FrezeeInventoryAction)
            GameManager.Instance.CloseInventory();
    }
    private void LateUpdate()
    {
        if (!loadedInventoryUI)
        {
            Debug.Log("Load Inventory UI!!!!!!!!!");
            loadedInventoryUI = true;
            LoadUIBySelectedTab();
            gameObject.SetActive(false);
        }
    }
    public void OnClickUITab(int index)
    {
        selectedTab = index;
        SaveManager.instance.tempGameData.selectingTab = selectedTab;
        CloseAllLoreUI();
        LoadUIBySelectedTab();
    }

    public void CloseAllLoreUI()
    {
        foreach (GameObject go in tabsUI)
        {
            if (go.GetComponent<InventoryLogic>() != null)
            {
                go.GetComponent<InventoryLogic>().CloseLoreUI();
            }
        }
    }

    private void LoadUIBySelectedTab()
    {
        for (int i = 0; i < tabsUI.Length; i++)
        {
            if (i == selectedTab)
            {
                tabsUI[i].gameObject.SetActive(true);
                tabs[i].Find("MainImage_Normal").gameObject.SetActive(false);
                tabs[i].Find("MainImage_Selected").gameObject.SetActive(true);
                selector.SetParent(tabs[i], true);
                selector.SetSiblingIndex(3);
                selector.localPosition = new Vector2(0f, 19.5f);
            }
            else
            {
                tabs[i].Find("MainImage_Normal").gameObject.SetActive(true);
                tabs[i].Find("MainImage_Selected").gameObject.SetActive(false);
                tabsUI[i].gameObject.SetActive(false);
            }
        }
    }
    public void ShowNewSignOnTab(int _index)
    {
        newItemImagesOnTabs[_index].gameObject.SetActive(true);
    }
    public void RemoveNewSignOnTab(int _index)
    {
        newItemImagesOnTabs[_index].gameObject.SetActive(false);  
    }
    public void ClickRightArrow()
    {
        rightArrow.transform.localScale = new Vector3(12f, 12f, 1f);
        if(rightArrowCorou == null)
            rightArrowCorou = StartCoroutine(ReturnArrowSizeToNormal(rightArrow, true));
        else
        {
            StopCoroutine(rightArrowCorou);
            rightArrowCorou = null;
        }
        if (selectedTab == tabs.Length - 1)
            selectedTab = 0;
        else
            selectedTab++;
        LoadUIBySelectedTab();
        //CloseAllLoreUI();
    }
    public void ClickLeftArrow()
    {
        Debug.Log("Click LEft arrow");
        leftArrow.transform.localScale = new Vector3(12f, 12f, 1f);
        if(leftArrowCorou == null)
        {
            leftArrowCorou = StartCoroutine(ReturnArrowSizeToNormal(leftArrow, false));
        }
        else
        {
            StopCoroutine(leftArrowCorou);
            leftArrowCorou = null;
        }
        if (selectedTab == 0)
            selectedTab = tabs.Length - 1;
        else
            selectedTab--;
        LoadUIBySelectedTab();
        //CloseAllLoreUI();
    }
    public void GetUpLeftArrow()
    {
        if(leftArrow.transform.localScale == new Vector3(12f, 12f, 1f) && leftArrowCorou == null)
            leftArrowCorou = StartCoroutine(ReturnArrowSizeToNormal(leftArrow, false));
    }
    public void GetUpRightArrow()
    {
        if (rightArrow.transform.localScale == new Vector3(12f, 12f, 1f) && rightArrowCorou == null)
            rightArrowCorou = StartCoroutine(ReturnArrowSizeToNormal(rightArrow, false));
    }
    private IEnumerator ReturnArrowSizeToNormal(Transform arrow, bool _rightArrow)
    {
        yield return new WaitForSecondsRealtime(0.15f);
        Debug.Log("Back Arrow");
        arrow.localScale = new Vector3(7.5f, 7.5f, 1f);
        if (_rightArrow)
            rightArrowCorou = null;
        else
            leftArrowCorou = null;
    }
    public void SetInventoryUIUninteractable()
    {
        FrezeeInventoryAction = true;
        closeInventoryBtn.GetComponent<CanvasGroup>().interactable = false;
        horizontalSelectTab.GetComponent<CanvasGroup>().interactable = false;
        //leftArrow.gameObject.SetActive(false);
        //rightArrow.gameObject.SetActive(false);
    }
    public void SetInventoryUIInteractable()
    {
        FrezeeInventoryAction = false;
        closeInventoryBtn.GetComponent<CanvasGroup>().interactable = true;
        horizontalSelectTab.GetComponent<CanvasGroup>().interactable = true;
        //leftArrow.gameObject.SetActive(true);
        //rightArrow.gameObject.SetActive(true);
    }
    public void Open()
    {
        FrezeeInventoryAction = true;
        fadeEffectHandler.StartFadeIn(() => FrezeeInventoryAction = false);
    }
    public void Close()
    {
        FrezeeInventoryAction = true;
        fadeEffectHandler.StartFadeOut(() => gameObject.SetActive(false));
    }
}

