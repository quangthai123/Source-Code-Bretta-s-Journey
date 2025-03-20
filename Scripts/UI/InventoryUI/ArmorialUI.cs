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

    
    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    protected override void LoadData()
    {
        base.LoadData();
        if(tempGameData.amorialHadItems != null)
            hadItems = tempGameData.amorialHadItems;
        if(tempGameData.amorialEquippedItems != null)
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
        for (int i=0; i<tempGameData.currenArmorialSlot; i++)
        {
            equippedArmorialSlot[i].gameObject.SetActive(true);
        }
        if(!Inventory.Instance.CheckSelectedItemIsEquipped(itemType, selectedItemImage.sprite))
        {
            ActivateEquipButton();
        }
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
            selectImage.SetActive(true);
            selectImage.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithHadItem);
            selectImage.transform.localScale = new Vector2(3.67f, 3.65f);
            return;
        }
        Debug.Log("Click armorial!");
        selectImage.SetActive(true);
        selectedItemImage.gameObject.SetActive(true);
        bool canRemoveNewSign = false;
        if (Inventory.Instance.CheckSelectedItemIsEquipped(itemType, image.sprite))
        {
            ActivateUnEquipButton();
            DeactivateEquipButton();
            selectImage.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithEquippedItem);
            selectImage.transform.localScale = new Vector2(5.35f, 5.35f);
        } else
        {
            selectImage.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithHadItem);
            selectImage.transform.localScale = new Vector2(3.67f, 3.65f);
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
                selectedItemLore.text = item.itemLore;
                break;
            }
        }
        itemInforUI.SetActive(true);
        if (canRemoveNewSign)
            RemoveNewSign(image);
    }
    public void EquipArmorialUI()
    {
        if(selectedItemImage.sprite == null) return;
        bool canEquip = false;
        int cnt = 0;
        foreach(Image image in equippedItemImage)
        {
            if (!image.enabled)
                canEquip = true;
            else
                cnt++;
            if(image.sprite == selectedItemImage.sprite)
            {
                canEquip = false;
                break;
            }
        }
        if (!canEquip || cnt >= tempGameData.currenArmorialSlot)
            return;
        foreach(Image image in itemHadImage)
        {
            if(selectedItemImage.sprite == image.sprite)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
                break;
            }
        }
        foreach (Image image in equippedItemImage)
        {
            if (!image.enabled)
            {
                image.sprite = selectedItemImage.sprite;
                image.enabled = true;
                selectImage.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithEquippedItem);
                selectImage.transform.localScale = new Vector2(5.35f, 5.35f);
                break;
            }
        }
        ActivateUnEquipButton();
        DeactivateEquipButton();
        Inventory.Instance.EquipArmorial(Inventory.Instance.GetItemIndexBySprite(itemType, selectedItemImage.sprite));
        numOfEquippedItem++;
    }
    public void UnequipArmorialUI()
    {
        if (selectedItemImage.sprite == null) return;
        bool canUnequip = false;
        foreach(Image i in equippedItemImage)
        {
            if(selectedItemImage.sprite == i.sprite)
            {
                i.sprite = null;
                i.enabled = false;
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
        image.sprite = selectedItemImage.sprite;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        selectImage.transform.position = new Vector2(image.transform.position.x, image.transform.position.y + selectImageOffSetYWithHadItem);
        selectImage.transform.localScale = new Vector2(3.67f, 3.65f);
        DeactivateUnEquipButton();
        ActivateEquipButton();
        Inventory.Instance.UnequipArmorial(Inventory.Instance.GetItemIndexBySprite(itemType, selectedItemImage.sprite));
        numOfEquippedItem--;
    }
}
