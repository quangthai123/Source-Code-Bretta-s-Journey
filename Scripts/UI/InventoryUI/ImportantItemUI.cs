using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImportantItemUI : InventoryLogic
{
    public static ImportantItemUI Instance;
    private bool isUpdating = false; // for upgrade player stats on this UI 
    protected void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        OnMoveSlotSelector += GetItemIndexByImage;
    }
    protected override void LoadData()
    {
        base.LoadData();
        if (tempGameData.importantHadItems != null)
            hadItems = tempGameData.importantHadItems;
        if (tempGameData.newImportantItems != null)
            newItems = tempGameData.newImportantItems;
    }
    protected override void Start()
    {
        base.Start();
        isUpdating = false;
    }
    public override void AddNewItemSign()
    {
        base.AddNewItemSign();
        tempGameData.newImportantItems = this.newItems;
    }
    protected override void LoadNewItemsSign()
    {
        if (isUpdating)
        {
            DeactiveAllNewSignOnReload();
            isUpdating = false;
        }
        base.LoadNewItemsSign();
    }
    public override void LoadHadItemUI()
    {
        foreach (var image in itemHadImage)
        {
            image.gameObject.SetActive(true);
            image.enabled = true;
            image.sprite = null;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
            Image border = image.transform.parent.GetComponent<Image>();
            border.material = null;
            border.color = new Color(1f, 1f, 1f, 1f);
        }
        base.LoadHadItemUI();
    }
    public void GetItemIndexByImage(Image image)
    {
        if (image.color.a == 0f)
        {
            selectedItemImage.gameObject.SetActive(false);
            itemInforUI.SetActive(false);
            selectedItemLoreTxt.text = "";
            selector.SetActive(true);
            selector.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithHadItem);
            selector.transform.localScale = new Vector2(3.67f, 3.65f);
            return;
        }
        Debug.Log("Click important!");
        selector.SetActive(true);
        selector.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithHadItem);
        selector.transform.localScale = new Vector2(3.67f, 3.65f);
        foreach (Item item in Inventory.Instance.allImportantItemsList)
        {
            if (image.sprite == item.itemImage.sprite)
            {
                selectedItemImage.sprite = item.itemImage.sprite;
                selectedItemName.text = item.itemName;
                selectedItemDescription.text = item.itemDescription;
                selectedItemLoreTxt.text = item.itemLore;
                selectedLore = item.itemLore;
                break;
            }
        }
        selectedItemImage.gameObject.SetActive(true);
        itemInforUI.SetActive(true);
        RemoveNewSign(itemHadImage.IndexOf(image), false);

        StartCoroutine(scrollViewHandler.ResetScrollView());
    }
    private void RemoveNewSign(int _indexToRemove, bool _isUpdating)
    {
        if (itemHadImage[_indexToRemove].transform.parent.Find("New Sign Image(Clone)"))
        {
            itemHadImage[_indexToRemove].transform.parent.Find("New Sign Image(Clone)").gameObject.SetActive(false);
            newItems.Remove(_indexToRemove);  
        }
        if(_isUpdating)
        {
            for(int i=0; i<newItems.Count; i++)
            {
                if (newItems[i] > _indexToRemove)
                    newItems[i]--;
            }
        }
        tempGameData.newImportantItems = this.newItems;
        if (newItems.Count < 1)
            InventoryUI.Instance.RemoveNewSignOnTab(tabIndex);
    }
    private void DeactiveAllNewSignOnReload()
    {
        Debug.Log("Destroy All new signs to load them again!");
        for(int i=0; i<itemHadImage.Count; i++)
        {
            if(itemHadImage[i].transform.parent.Find("New Sign Image(Clone)"))
                itemHadImage[i].transform.parent.Find("New Sign Image(Clone)").gameObject.SetActive(false);
        }
    }
    public void ReloadNewSignsOnUpgrade(int _itemIndex)
    {
        Debug.Log(GetIndexOnUIByItemIndex(_itemIndex));
        isUpdating = true;
        RemoveNewSign(GetIndexOnUIByItemIndex(_itemIndex), true);
    }

    private int GetIndexOnUIByItemIndex(int _itemIndex)
    {
        for(int i=0; i<tempGameData.importantHadItems.Count; i++)
        {
            if (tempGameData.importantHadItems[i] == _itemIndex)
                return i;
        }
        return -1;
    }
    public void CloseInforUIOnUpgrade()
    {
        itemInforUI.SetActive(false);
        selectedItemImage.gameObject.SetActive(false);
        selector.SetActive(false);
    }
}