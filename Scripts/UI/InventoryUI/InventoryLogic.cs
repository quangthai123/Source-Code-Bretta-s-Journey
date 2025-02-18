using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class InventoryLogic : MonoBehaviour
{
    [SerializeField] protected ItemType itemType;
    public List<Image> itemHadImage;
    public List<Image> equippedItemImage;
    [SerializeField][Range(0, 6)] protected int tabIndex;
    [SerializeField] protected GameObject loreUI;
    [SerializeField] protected Material hadItemBorderMaterial;
    [Header("Item Inventory Infor")]
    [SerializeField] protected List<int> hadItems;
    [SerializeField] protected List<int> equippedItems;
    [SerializeField] protected int numOfEquippedItem;
    [Header("Item UI Infor")]
    [SerializeField] protected GameObject itemInforUI;
    [SerializeField] protected Image selectedItemImage;
    [SerializeField] protected TextMeshProUGUI selectedItemName;
    [SerializeField] protected TextMeshProUGUI selectedItemDescription;
    [SerializeField] protected TextMeshProUGUI selectedItemLore;
    [SerializeField] protected GameObject selectImage;

    [Header("SelectImage Offset")]
    [SerializeField] protected float selectImageOffSetYWithHadItem;
    [Header("New Sign Image")]
    [SerializeField] protected GameObject newSignImage;
    [SerializeField] protected Vector2 newSignImagePos = new Vector2(34.54f, 34.54f);
    [SerializeField] protected List<int> newItems; // cac item da nhat nhung nguoi choi chua xem va co ky hieu ! 
    protected virtual void Start()
    {
        LoadData();
        InitializeUIOnLoadScene();
        LoadHadItemUI();
        LoadEquippedItemUI();
    }
    protected virtual void LoadData()
    {
        hadItems = new List<int>();
        equippedItems = new List<int>();
        newItems = new List<int>();
    }
    protected virtual void InitializeUIOnLoadScene()
    {
        foreach (var image in itemHadImage)
        {
            image.gameObject.SetActive(true);
            image.sprite = null;
            image.enabled = true;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        }
        if(equippedItemImage.Count > 0)
        {
            foreach (var image in equippedItemImage)
            {
                image.gameObject.SetActive(true);
                image.sprite = null;
                image.enabled = false;
            }
        }
        selectImage.SetActive(false);
        loreUI.SetActive(false);
        itemInforUI.SetActive(false);
        selectedItemImage.gameObject.SetActive(false);
    }
    public virtual void LoadHadItemUI()
    {
        LoadData();
        if (hadItems.Count < 1)
            return;
        int cnt = 0;
        foreach (int i in hadItems)
        {
            Image border = itemHadImage[cnt].transform.parent.GetComponent<Image>();
            border.material = hadItemBorderMaterial;
            border.color = new Color(242f / 255f, 219f / 255f, 181f / 255f, 1f);
            if (CheckEquippedItem(i))
            {
                cnt++;
                continue;
            }
            itemHadImage[cnt].sprite = Inventory.Instance.GetSpriteByItemIndex(this.itemType, i);
            itemHadImage[cnt].enabled = true;
            itemHadImage[cnt].color = new Color(itemHadImage[cnt].color.r, itemHadImage[cnt].color.g,
                itemHadImage[cnt].color.b, 1f);
            cnt++;
        }
        LoadNewItemsSign();
    }
    protected bool CheckEquippedItem(int index)
    {
        foreach(int i in equippedItems)
        {
            if(i==index) 
                return true;
        }
        return false;
    }
    public virtual void LoadEquippedItemUI()
    {
        if (equippedItems.Count < 1)
            return;
        int cnt = 0;
        foreach (int i in equippedItems)
        {
            if (i != -1)
            {
                equippedItemImage[cnt].sprite = Inventory.Instance.GetSpriteByItemIndex(this.itemType, i);
                equippedItemImage[cnt].enabled = true;
                numOfEquippedItem++;

                int index = GetIndexOnHadItemUIByItemIndex(i);
                itemHadImage[index].color = new Color(itemHadImage[index].color.r, itemHadImage[index].color.g,
                itemHadImage[index].color.b, 0f);
            }
            cnt++;
        }
    }
    //protected void DeactiveEquippedItemOnHadItem()
    //{
    //    if (equippedItems.Count >= 1)
    //    {
    //        foreach (int j in equippedItems)
    //        {
    //            if(j != -1)
    //            { 
    //                int index = GetIndexOnHadItemUIByItemIndex(j);
    //                itemHadImage[index].color = new Color(itemHadImage[index].color.r, itemHadImage[index].color.g,
    //                itemHadImage[index].color.b, 0f);               
    //            }
    //        }
    //    }
    //}
    public void OpenLoreUI()
    {
        loreUI.SetActive(true);
    }
    public void CloseLoreUI()
    {
        loreUI.SetActive(false);
    }
    public virtual void AddNewItemSign()
    {
        LoadData();
        newItems.Add(hadItems.Count - 1);
    }
    protected virtual void LoadNewItemsSign()
    {
        if (newItems == null || (newItems != null && newItems.Count < 1)) return;
        else
            InventoryUI.Instance.ShowNewSignOnTab(tabIndex);
        foreach (int i in newItems)
        {
            if (!itemHadImage[i].transform.parent.Find("New Sign Image(Clone)"))
            {
                GameObject newSign = Instantiate(newSignImage, newSignImagePos, Quaternion.identity, itemHadImage[i].transform.parent);
                newSign.transform.localPosition = newSignImagePos;
                newSign.SetActive(true);
            }
            else
            {
                GameObject newSign = itemHadImage[i].transform.parent.Find("New Sign Image(Clone)").gameObject;
                if (!newSign.activeSelf)
                    newSign.SetActive(true);
            }
        }
    }
    protected virtual void RemoveNewSign(Image _image)
    {
        int selectedIndex = itemHadImage.IndexOf(_image);
        if (newItems.Contains(selectedIndex))
        {
            itemHadImage[selectedIndex].transform.parent.Find("New Sign Image(Clone)").gameObject.SetActive(false);
            newItems.Remove(selectedIndex);
        }
        if (newItems.Count < 1)
            InventoryUI.Instance.RemoveNewSignOnTab(tabIndex);
    }
    protected int GetIndexOnHadItemUIByItemIndex(int _itemIndex)
    {
        for (int i = 0; i < hadItems.Count; i++)
        {
            if (hadItems[i] == _itemIndex)
                return i;
        }
        return -1;
    }
    public void DeactiveSelectNullImageOnPickupNewItem()
    {
        if (!itemInforUI.activeSelf)
            selectImage.SetActive(false);
    }
}
