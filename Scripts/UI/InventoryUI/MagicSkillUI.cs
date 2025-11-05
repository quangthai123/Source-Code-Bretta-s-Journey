using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSkillUI : InventoryLogic
{
    public static MagicSkillUI Instance { get; private set; }
    [Header("Buttons")]
    [SerializeField] private Transform equipButton;
    [SerializeField] private Transform unEquipButton;
    [SerializeField] private Transform selectedMagicGemEquippedTransf;
    [SerializeField] private Transform selectedMagicGemHadTransf;
    [SerializeField] private Transform equipSlotSelector;
    public GameObject LockGemSlot2Image;
    [SerializeField] private int selectedSkillNum;
    protected void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    protected override void Start()
    {
        base.Start();
        LoadEquipGemSlot();
        DeactivateEquipButton();
        DeactivateUnEquipButton();
    }
    private void LoadEquipGemSlot()
    {
        // Neu da mo slot skill 2 => mac dinh ko chon slot nao khi load scene
        if (tempGameData.learnedSkill[17])
        {
            equipSlotSelector.gameObject.SetActive(false);
            LockGemSlot2Image.SetActive(false);
            selectedSkillNum = -1;
            DeactivateEquipButton();
            DeactivateUnEquipButton();
            return;
        }
        // Neu chua mo slot skill 2 => mac dinh chon slot 1 khi load scene
        equipSlotSelector.localPosition = new Vector2(equippedItemImage[0].transform.parent.localPosition.x - .23f,
            equippedItemImage[0].transform.parent.localPosition.y + .15f);
        equipSlotSelector.gameObject.SetActive(true);
        selectedMagicGemEquippedTransf = equippedItemImage[0].transform;
        selectedSkillNum = 0;
        if (selectedMagicGemEquippedTransf.gameObject.activeSelf)
        {
            DeactivateEquipButton();
            ActivateUnEquipButton();
        }
        else
        {
            ActivateEquipButton();
            DeactivateUnEquipButton();
        }

    }

    protected override void LoadData()
    {
        base.LoadData();
        if (tempGameData.magicGemHadItems.Count >= 1)
            hadItems = tempGameData.magicGemHadItems;
        if (tempGameData.magicGemEquippedItems.Count >= 1)
            equippedItems = tempGameData.magicGemEquippedItems;
        if (tempGameData.newMagicGemItems != null)
            newItems = tempGameData.newMagicGemItems;
    }
    public override void AddNewItemSign()
    {
        base.AddNewItemSign();
        tempGameData.newMagicGemItems = this.newItems;
        GetComponent<SkillUIController>().ShowNewSignOnMagicTab();
    }
    protected override void RemoveNewSign(Image _image)
    {
        base.RemoveNewSign(_image);
        tempGameData.newMagicGemItems = this.newItems;
        if (newItems.Count < 1)
            GetComponent<SkillUIController>().DisableNewSignOnMagicTab();
    }
    private void DeactivateEquipButton()
    {
        equipButton.GetComponent<CanvasGroup>().alpha = .2f;
        equipButton.GetComponent<Button>().enabled = false;
    }
    private void ActivateEquipButton()
    {
        equipButton.GetComponent<CanvasGroup>().alpha = 1f;
        equipButton.GetComponent<Button>().enabled = true;
    }
    private void DeactivateUnEquipButton()
    {
        unEquipButton.GetComponent<CanvasGroup>().alpha = .2f;
        unEquipButton.GetComponent<Button>().enabled = false;
    }
    private void ActivateUnEquipButton()
    {
        unEquipButton.GetComponent<CanvasGroup>().alpha = 1f;
        unEquipButton.GetComponent<Button>().enabled = true;
    }
    public void OnClickMagicGem(Image image)
    {
        SetHadGemSelectorPos(image.transform);

        selectedMagicGemHadTransf = image.transform;
        if (image.color.a == 0f)
        {
            DeactivateEquipButton();
            itemInforUI.SetActive(false);
            selectedItemImage.sprite = null;
            return;
        }
        Debug.Log("Click magic gem!");
        selector.SetActive(true);
        selectedItemImage.gameObject.SetActive(true);

        if (selectedMagicGemEquippedTransf != null && selectedMagicGemEquippedTransf.GetComponent<Image>().color.a == 0f)
            ActivateEquipButton();
        GetGemInforByItsImage(image);
        RemoveNewSign(image);
    }
    protected override void InitializeUIOnLoadScene()
    {
        foreach (var image in itemHadImage)
        {
            image.gameObject.SetActive(true);
            image.sprite = null;
            image.enabled = true;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        }
        if (equippedItemImage.Count > 0)
        {
            foreach (var image in equippedItemImage)
            {
                image.gameObject.SetActive(true);
                image.sprite = null;
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
            }
        }
        selector.SetActive(false);
        itemInforUI.SetActive(false);
        selectedItemImage.gameObject.SetActive(false);
    }
    public override void LoadEquippedItemUI()
    {
        if (equippedItems.Count < 1)
            return;
        int cnt = 0;
        foreach (int i in equippedItems)
        {
            if (i != -1)
            {
                equippedItemImage[cnt].sprite = Inventory.Instance.GetSpriteByItemIndex(this.itemType, i);
                equippedItemImage[cnt].color = new Color(1f, 1f, 1f, 1f);
                numOfEquippedItem++;

                int index = GetIndexOnHadItemUIByItemIndex(i);
                itemHadImage[index].color = new Color(itemHadImage[index].color.r, itemHadImage[index].color.g,
                itemHadImage[index].color.b, 0f);
            }
            cnt++;
        }
    }
    private void GetGemInforByItsImage(Image image)
    {
        foreach (Item item in Inventory.Instance.allMagicGemItemsList)
        {
            if (image.sprite == item.itemImage.sprite)
            {
                selectedItemImage.sprite = item.itemImage.sprite;
                selectedItemImage.gameObject.SetActive(true);
                selectedItemName.text = item.itemName;
                selectedItemDescription.text = item.itemDescription;
                selectedItemLoreTxt.text = item.itemLore;
                selectedLore = item.itemLore;
                break;
            }
        }
        itemInforUI.SetActive(true);
        if(scrollViewHandler != null)
            StartCoroutine(scrollViewHandler.ResetScrollView());
    }

    public void OnClickEquippedGem(Image image)
    {
        transform.GetComponent<SkillUIController>().OnClickMagicSkillUITab();
        equipSlotSelector.localPosition = new Vector2(image.transform.parent.localPosition.x - .23f, 
            image.transform.parent.localPosition.y + .15f);
        equipSlotSelector.gameObject.SetActive(true);

        selectedMagicGemEquippedTransf = image.transform;
        if(image.transform.parent.name == "Magic Gem Slot 1")
            selectedSkillNum = 0;
        else
            selectedSkillNum = 1;
        if (image.color.a == 0f)
        {
            DeactivateUnEquipButton();
            if (selectedMagicGemHadTransf != null && selectedMagicGemHadTransf.GetComponent<Image>().color.a != 0)
            {
                ActivateEquipButton();
                GetGemInforByItsImage(selectedMagicGemHadTransf.GetComponent<Image>());
            } else
            {
                itemInforUI.SetActive(false);
            }
            return;
        }
        GetGemInforByItsImage(image);
        ActivateUnEquipButton();
        DeactivateEquipButton();
    }
    public void OnClickEquipGem()
    {
        selectedMagicGemEquippedTransf.GetComponent<Image>().sprite = selectedMagicGemHadTransf.GetComponent<Image>().sprite;
        selectedMagicGemEquippedTransf.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        selectedMagicGemHadTransf.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0);
        //selectedMagicGemHadTransf = null;
        selector.gameObject.SetActive(false);
        DeactivateEquipButton();
        ActivateUnEquipButton();
        tempGameData.magicGemEquippedItems[selectedSkillNum] = Inventory.Instance.GetItemIndexBySprite(ItemType.MagicGem, selectedMagicGemEquippedTransf.GetComponent<Image>().sprite);
        SkillManager.instance.SetUsingSkill(selectedSkillNum, tempGameData.magicGemEquippedItems[selectedSkillNum]);
        MobileInputTesting.Instance.SetStateOfMagicBtn();
        MagicSkillAvatars.Instance.LoadSkillAvatars();
    }
    public void OnClickUnequipGem()
    {
        int gemIndex = Inventory.Instance.GetItemIndexBySprite(ItemType.MagicGem, selectedMagicGemEquippedTransf.GetComponent<Image>().sprite);
        selectedMagicGemHadTransf = itemHadImage[tempGameData.magicGemHadItems.IndexOf(gemIndex)].transform;
        selectedMagicGemHadTransf.GetComponent<Image>().sprite = selectedMagicGemEquippedTransf.GetComponent<Image>().sprite;
        selectedMagicGemHadTransf.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        selectedMagicGemEquippedTransf.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0);
        SetHadGemSelectorPos(selectedMagicGemHadTransf);
        ActivateEquipButton();
        DeactivateUnEquipButton();
        tempGameData.magicGemEquippedItems[selectedSkillNum] = -1;
        MobileInputTesting.Instance.SetStateOfMagicBtn();
        MagicSkillAvatars.Instance.LoadSkillAvatars();
    }

    private void SetHadGemSelectorPos(Transform transf)
    {
        selector.SetActive(true);
        selector.transform.position = new Vector2(transf.position.x, transf.position.y + selectImageOffSetYWithHadItem);
        selector.transform.localScale = new Vector2(3.67f, 3.65f);
    }
    protected override void LoadNewItemsSign()
    {
        base.LoadNewItemsSign();
        if (newItems != null && newItems.Count >= 1)
            GetComponent<SkillUIController>().ShowNewSignOnMagicTab();
        else
            GetComponent<SkillUIController>().DisableNewSignOnMagicTab();
    }
}
