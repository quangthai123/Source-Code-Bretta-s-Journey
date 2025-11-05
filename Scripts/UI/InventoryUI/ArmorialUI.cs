using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorialUI : InventoryLogic
{
    public static ArmorialUI Instance { get; private set; }

    [SerializeField] private List<GameObject> equippedArmorialSlot;

    [SerializeField] private TextMeshProUGUI selectedItemFunction;

    [SerializeField] private float selectImageOffSetYWithEquippedItem;
    [Header("Buttons")]
    [SerializeField] private Transform equipButton;
    [SerializeField] private Transform unEquipButton;
    [SerializeField] private Transform equipText;
    [SerializeField] private Transform unEquipText;
    [SerializeField]
    private ArmorialSlotSelector slotSelector;


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
        if (tempGameData.amorialHadItems != null)
            hadItems = tempGameData.amorialHadItems;
        if (tempGameData.amorialEquippedItems != null)
            equippedItems = tempGameData.amorialEquippedItems;
        if (tempGameData.newArmorialItems != null)
            newItems = tempGameData.newArmorialItems;
    }
    protected override void Start()
    {
        base.Start();
        LoadEquippedArmorialSlot();

        DeactivateEquipButton();
        DeactivateUnEquipButton();
    }

    public void LoadEquippedArmorialSlot()
    {
        for (int i = 0; i < 9; i++)
        {
            equippedArmorialSlot[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < tempGameData.currenArmorialSlot; i++)
        {
            equippedArmorialSlot[i].gameObject.SetActive(true);
        }
        if (!CheckSelectedItemIsEquipped(selectedItemImage.sprite))
        {
            ActivateEquipButton();
        }
    }
    public bool CheckSelectedItemIsEquipped(Sprite sprite)
    {
        int index = Inventory.Instance.GetItemIndexBySprite(itemType, sprite);
        if (tempGameData.amorialEquippedItems == null ||
            tempGameData.amorialEquippedItems.Count < 1)
            return false;
        foreach (int i in tempGameData.amorialEquippedItems)
        {
            if (index == i)
                return true;
        }
        return false;
    }
    public override void AddNewItemSign()
    {
        base.AddNewItemSign();
        tempGameData.newArmorialItems = this.newItems;
    }
    protected override void RemoveNewSign(Image _image)
    {
        base.RemoveNewSign(_image);
        tempGameData.newArmorialItems = this.newItems;
    }
    private void DeactivateEquipButton()
    {
        Image equipBtnImage = equipButton.GetComponent<Image>();
        equipBtnImage.color = new Color(equipBtnImage.color.r, equipBtnImage.color.g, equipBtnImage.color.b, 50f / 255f);
        TextMeshProUGUI equipTxt = equipText.GetComponent<TextMeshProUGUI>();
        equipTxt.color = new Color(equipTxt.color.r, equipTxt.color.g, equipTxt.color.b, 50f / 255f);

        equipButton.GetComponent<Button>().enabled = false;
    }
    private void ActivateEquipButton()
    {
        Image equipBtnImage = equipButton.GetComponent<Image>();
        equipBtnImage.color = new Color(equipBtnImage.color.r, equipBtnImage.color.g, equipBtnImage.color.b, 255f / 255f);
        TextMeshProUGUI equipTxt = equipText.GetComponent<TextMeshProUGUI>();
        equipTxt.color = new Color(equipTxt.color.r, equipTxt.color.g, equipTxt.color.b, 255f / 255f);

        equipButton.GetComponent<Button>().enabled = true;
    }
    private void DeactivateUnEquipButton()
    {
        Image unEquipBtnImage = unEquipButton.GetComponent<Image>();
        unEquipBtnImage.color = new Color(unEquipBtnImage.color.r, unEquipBtnImage.color.g, unEquipBtnImage.color.b, 50f / 255f);
        TextMeshProUGUI unEquipTxt = unEquipText.GetComponent<TextMeshProUGUI>();
        unEquipTxt.color = new Color(unEquipTxt.color.r, unEquipTxt.color.g, unEquipTxt.color.b, 50f / 255f);

        unEquipButton.GetComponent<Button>().enabled = false;
    }
    private void ActivateUnEquipButton()
    {
        Image unEquipBtnImage = unEquipButton.GetComponent<Image>();
        unEquipBtnImage.color = new Color(unEquipBtnImage.color.r, unEquipBtnImage.color.g, unEquipBtnImage.color.b, 255f / 255f);
        TextMeshProUGUI unEquipTxt = unEquipText.GetComponent<TextMeshProUGUI>();
        unEquipTxt.color = new Color(unEquipTxt.color.r, unEquipTxt.color.g, unEquipTxt.color.b, 255f / 255f);

        unEquipButton.GetComponent<Button>().enabled = true;
    }
    public void GetItemIndexByImage(Image image)
    {
        if (image.color.a == 0f)
        {
            DeactivateEquipButton();
            DeactivateUnEquipButton();
            itemInforUI.SetActive(false);
            selectedItemImage.sprite = null;
            selector.SetActive(true);
            selector.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithHadItem);
            selector.transform.localScale = new Vector2(3.67f, 3.65f);
            return;
        }
        if (!image.enabled)
        {
            itemInforUI.SetActive(false);
            selectedItemImage.sprite = null;
            selector.SetActive(true);
            selector.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithEquippedItem);
            selector.transform.localScale = new Vector2(5.35f, 5.35f);
            return;
        }
        Debug.Log("Click armorial!");
        selector.SetActive(true);
        selectedItemImage.gameObject.SetActive(true);
        bool canRemoveNewSign = false;
        if (CheckSelectedItemIsEquipped(image.sprite))
        {
            ActivateUnEquipButton();
            DeactivateEquipButton();
            selector.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithEquippedItem);
            selector.transform.localScale = new Vector2(5.35f, 5.35f);
        }
        else
        {
            selector.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithHadItem);
            selector.transform.localScale = new Vector2(3.67f, 3.65f);
            DeactivateUnEquipButton();
            if (numOfEquippedItem < tempGameData.currenArmorialSlot)
                ActivateEquipButton();
            else
                DeactivateEquipButton();
            canRemoveNewSign = true;
        }
        foreach (Item item in Inventory.Instance.allArmorialItemsList)
        {
            if (image.sprite == item.itemImage.sprite)
            {
                selectedItemImage.sprite = item.itemImage.sprite;
                selectedItemName.text = item.itemName;
                selectedItemDescription.text = item.itemDescription;
                selectedItemFunction.text = item.itemFunction;
                selectedItemLoreTxt.text = item.itemLore;
                selectedLore = item.itemLore;
                break;
            }
        }
        itemInforUI.SetActive(true);
        if (canRemoveNewSign)
            RemoveNewSign(image);
        if (scrollViewHandler != null)
            StartCoroutine(scrollViewHandler.ResetScrollView());
    }
    public void EquipArmorialUI()
    {
        if (!itemInforUI.activeInHierarchy) return;
        bool canEquip = false;
        int cnt = 0;
        foreach (Image image in equippedItemImage)
        {
            if (!image.enabled)
                canEquip = true;
            else
                cnt++;
            if (image.sprite == selectedItemImage.sprite)
            {
                canEquip = false;
                break;
            }
        }
        if (!canEquip || cnt >= tempGameData.currenArmorialSlot)
            return;
        RectTransform startItem = null;
        foreach (Image image in itemHadImage)
        {
            if (selectedItemImage.sprite == image.sprite)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
                startItem = image.rectTransform;
                break;
            }
        }
        foreach (Image image in equippedItemImage)
        {
            if (image.sprite == null)
            {
                image.sprite = selectedItemImage.sprite;
                itemInforUI.SetActive(false);
                DeactivateEquipButton();
                DeactivateUnEquipButton();
                Inventory.Instance.EquipArmorial(Inventory.Instance.GetItemIndexBySprite(itemType, selectedItemImage.sprite));
                numOfEquippedItem++;
                StartCoroutine(DoFlyItemEffect(startItem, image.rectTransform,
                    () =>
                    {
                        image.enabled = true;
                    }));
                //selector.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithEquippedItem);
                //selector.transform.localScale = new Vector2(5.35f, 5.35f);
                break;
            }
        }
        //slotSelector.SetEquipIndexOnEquipped(numOfEquippedItem - 1);
    }
    public void UnequipArmorialUI()
    {
        if (!itemInforUI.activeInHierarchy) return;
        bool canUnequip = false;
        RectTransform startItem = null;
        foreach (Image i in equippedItemImage)
        {
            if (selectedItemImage.sprite == i.sprite)
            {
                //i.sprite = null;
                i.enabled = false;
                startItem = i.rectTransform;
                canUnequip = true;
                break;
            }
        }
        if (!canUnequip)
            return;
        Debug.Log($"Unequip {selectedItemImage.sprite.name} Succesful");
        int itemIndex = Inventory.Instance.GetItemIndexBySprite(ItemType.Armorial, selectedItemImage.sprite);
        int index = tempGameData.amorialHadItems.IndexOf(itemIndex);
        Image image = itemHadImage[index];
        StartCoroutine(DoFlyItemEffect(startItem, image.rectTransform,
            () =>
            {
                startItem.GetComponent<Image>().sprite = null;
                image.sprite = selectedItemImage.sprite;
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            }));
        //selector.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithHadItem);
        //selector.transform.localScale = new Vector2(3.67f, 3.65f);
        itemInforUI.SetActive(false);
        DeactivateEquipButton();
        DeactivateUnEquipButton();
        Inventory.Instance.UnequipArmorial(Inventory.Instance.GetItemIndexBySprite(itemType, selectedItemImage.sprite));
        numOfEquippedItem--;

        //slotSelector.SetHadIndexOnUnEquipped(index);
    }
}
