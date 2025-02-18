using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    void Start()
    {
        rightArrow = transform.Find("Right Arrow");
        leftArrow = transform.Find("Left Arrow");
        selectedTab = SaveManager.instance.tempGameData.selectingTab;
        loadedInventoryUI = false;
        for (int i = 0; i < tabsUI.Length; i++)
        {
            tabsUI[i].SetActive(true);
        }
        rightArrowCorou = null;
        leftArrowCorou = null;
    }
    private void Update()
    {
        if (!loadedInventoryUI)
        {
            loadedInventoryUI = true;
            LoadUIBySelectedTab();
        }
        //selector.position = new Vector2(tabs[selectedTab].position.x, selector.transform.position.y);

        if (Input.GetKeyDown(KeyCode.Q))
            ClickLeftArrow();
        if(Input.GetKeyDown(KeyCode.E))
            ClickRightArrow();
        if(Input.GetKeyDown(KeyCode.Escape))
            GameManager.Instance.CloseInventory();
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
                selector.position = new Vector2(tabs[i].position.x, selector.transform.position.y);
                selector.SetParent(tabs[i], true);
                selector.SetSiblingIndex(3);
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
        if (selectedTab == 6)
            selectedTab = 0;
        else
            selectedTab++;
        LoadUIBySelectedTab();
        CloseAllLoreUI();
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
            selectedTab = 6;
        else
            selectedTab--;
        LoadUIBySelectedTab();
        CloseAllLoreUI();
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
        closeInventoryBtn.GetComponent<CanvasGroup>().interactable = false;
        horizontalSelectTab.GetComponent<CanvasGroup>().interactable = false;
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
    }
    public void SetInventoryUIInteractable()
    {
        closeInventoryBtn.GetComponent<CanvasGroup>().interactable = true;
        horizontalSelectTab.GetComponent<CanvasGroup>().interactable = true;
        leftArrow.gameObject.SetActive(true);
        rightArrow.gameObject.SetActive(true);
    }
}

