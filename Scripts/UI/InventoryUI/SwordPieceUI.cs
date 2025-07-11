using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwordPieceUI : InventoryLogic
{
    public static SwordPieceUI Instance;
    public List<Transform> swordPieceData;
    public List<Transform> bigSlot;
    [SerializeField] int currentSwordPieceSlot;
    [SerializeField] private TextMeshProUGUI selectedItemFunction;
    [SerializeField] private float selectImageOffSetYWithEquippedItem;

    [Header("Select Images for equipped sword piece")]
    public Transform selectBigSlot;
    public Transform selectImage2;

    [Header("Buttons")]
    [SerializeField] private Transform equipButton;
    [SerializeField] private Transform unEquipButton;
    [SerializeField] private Transform equipText;
    [SerializeField] private Transform unEquipText;

    [SerializeField] private Transform currentSelectHadSwordPiece;
    [SerializeField] private Transform currentSelectEquippedSwordPieceParent;
    public Transform currentSelectPerfectSwordHad;
    public Transform currentSelectPerfectSwordEquipped;
    [SerializeField] private float selectedImageScaleModifier;
    private bool selectedASwordPieceHadFirstTime;
    private int currentBigSlot;
    private PerfectSwordChecker perfectSwordChecker;
    [SerializeField] private List<Image> newItemImages;
    [Header("Had Item Tab")]
    public Transform swordPieceHadUI;
    public Transform perfectSwordHadUI;
    [SerializeField] private Transform swordPieceHadTab;
    [SerializeField] private Transform perfectSwordHadTab;
    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        perfectSwordChecker = GetComponent<PerfectSwordChecker>();
        swordPieceHadUI.gameObject.SetActive(true);
        perfectSwordHadUI.gameObject.SetActive(false);
        OnClickSwordPieceHadTab();
    }
    protected override void Start()
    {
        base.Start();
        LoadBigSlotByCurrentSwordPieceSlot();
        //Time.timeScale = .75f;
    }
    protected override void InitializeUIOnLoadScene()
    {
        selectImage.SetActive(false);
        selectImage2.gameObject.SetActive(false);
        loreUI.SetActive(false);
        itemInforUI.SetActive(false);
        base.selectedItemImage.useSpriteMesh = true;
        base.selectedItemImage.gameObject.SetActive(false);
        selectBigSlot.gameObject.SetActive(false);
        DeactivateEquipButton();
        currentSelectHadSwordPiece = null;
        currentSelectEquippedSwordPieceParent = null;
        selectedASwordPieceHadFirstTime = false;
        currentBigSlot = 0;
    }
    protected override void LoadData()
    {
        base.LoadData();
        if (tempGameData.swordPieceHadItems != null)
            hadItems = tempGameData.swordPieceHadItems;
        if (tempGameData.swordPieceEquippedItems != null)
            equippedItems = tempGameData.swordPieceEquippedItems;
        if (tempGameData.newSwordPieceItems != null)
            newItems = tempGameData.newSwordPieceItems;
        currentSwordPieceSlot = tempGameData.currentSwordPieceSlot;
    }
    public override void AddNewItemSign()
    {
        base.AddNewItemSign();
        newItemImages.Add(itemHadImage[hadItems.Count - 1].transform.GetChild(1).GetComponent<Image>());
        tempGameData.newSwordPieceItems = this.newItems;
        LoadNewItemsSign();
    }
    protected override void RemoveNewSign(Image _image)
    {
        newItemImages.Remove(_image);
        int selectedIndex = itemHadImage.IndexOf(_image.transform.parent.GetComponent<Image>());
        Debug.Log(selectedIndex);
        if (newItems.Contains(selectedIndex))
        {
            itemHadImage[selectedIndex].transform.Find("New Sign Image(Clone)").gameObject.SetActive(false);
            newItems.Remove(selectedIndex);
        }
        if (newItems.Count < 1)
            InventoryUI.Instance.RemoveNewSignOnTab(tabIndex);
        tempGameData.newSwordPieceItems = this.newItems;
    }
    public void ReloadNewSignsOnMergedSword()
    {
        for(int i=0; i<itemHadImage.Count; i++)
        {
            if(itemHadImage[i].transform.Find("New Sign Image(Clone)"))
            {
                itemHadImage[i].transform.Find("New Sign Image(Clone)").gameObject.SetActive(false);
            }
        }
        newItems.Clear();
        foreach (Image item in newItemImages)
        {
            if (!item.transform.parent.Find("New Sign Image(Clone)"))
            {
                GameObject newSign = Instantiate(newSignImage, newSignImagePos, Quaternion.identity, item.transform.parent);
                newSign.transform.localPosition = newSignImagePos;
                newSign.SetActive(true);
            }
            else
            {
                GameObject newSign = item.transform.parent.Find("New Sign Image(Clone)").gameObject;
                if (!newSign.activeSelf)
                    newSign.SetActive(true);
            }
            newItems.Add(itemHadImage.IndexOf(item.transform.parent.GetComponent<Image>()));
        }
        tempGameData.newSwordPieceItems = newItems;
    }
    private void LoadBigSlotByCurrentSwordPieceSlot()
    {
        foreach(Transform go in bigSlot)
        {
            go.GetComponent<CanvasGroup>().alpha = 1f;
            go.GetComponent<CanvasGroup>().interactable = false;
        }
        for(int i=0; i<currentSwordPieceSlot/4; i++)
        {
            bigSlot[i].GetComponent<CanvasGroup>().alpha = 1f;
            bigSlot[i].GetComponent<CanvasGroup>().interactable = true;
            currentBigSlot++;
        }
    }
    public void UpdateBigSlotUIOnEnhance()
    {
        bigSlot[currentBigSlot].GetComponent<CanvasGroup>().alpha = 1f;
        bigSlot[currentBigSlot].GetComponent<CanvasGroup>().interactable = true;
        currentBigSlot++;
    }
    public override void LoadHadItemUI()
    {
        LoadData();
        foreach (var image in itemHadImage)
        {
            image.material = null;
            image.color = new Color(1f, 1f, 1f, 1f);
            image.transform.Find("Borders").gameObject.SetActive(false);
        }
        if (hadItems.Count < 1)
            return;
        int cnt = 0;
        foreach (int i in hadItems)
        {
            itemHadImage[cnt].transform.Find("Borders").gameObject.SetActive(true);
            if (CheckEquippedItem(i))
            {
                cnt++;
                continue;
            }
            swordPieceData[i].SetParent(itemHadImage[cnt].transform, true);
            swordPieceData[i].localPosition = Vector2.zero;
            swordPieceData[i].gameObject.SetActive(true);
            cnt++;
        }
        LoadNewItemsSign();
    }
    public override void LoadEquippedItemUI()
    {
        LoadData();
        if (equippedItems.Count < 1)
            return;
        int cnt = 0;
        foreach (int i in equippedItems)
        {
            if (i != -1)
            {
                swordPieceData[i].SetParent(equippedItemImage[cnt].transform, true);
                swordPieceData[i].localPosition = Vector2.zero;
                swordPieceData[i].gameObject.SetActive(true);
                numOfEquippedItem++;
            }
            cnt++;
        }
    }
    protected override void LoadNewItemsSign()
    {
        if (newItems == null || (newItems != null && newItems.Count < 1)) return;
        else
            InventoryUI.Instance.ShowNewSignOnTab(tabIndex);
        foreach (int i in newItems)
        {
            if (!itemHadImage[i].transform.Find("New Sign Image(Clone)"))
            {
                GameObject newSign = Instantiate(newSignImage, newSignImagePos, Quaternion.identity, itemHadImage[i].transform);
                newSign.transform.localPosition = newSignImagePos;
                newSign.SetActive(true);
            }
            else
            {
                GameObject newSign = itemHadImage[i].transform.Find("New Sign Image(Clone)").gameObject;
                if (!newSign.activeSelf)
                    newSign.SetActive(true);
            }
        }
    }
    private bool CheckHadItemOnSlot(Transform itemHadSlot, bool checkOnEquipSlot)
    {
        if (!checkOnEquipSlot)
        {
            if (itemHadSlot.childCount == 3 || (itemHadSlot.childCount == 2 && itemHadSlot.GetChild(1).name != "New Sign Image(Clone)"))
                return true;
            return false;
        } else
        {
            if (itemHadSlot.childCount == 1)
                return true;
            return false;
        }
    }
    public void GetItemIndexByImage(Transform itemHadSlot)
    {
        selectedASwordPieceHadFirstTime = true;
        selectImage.transform.SetParent(itemHadSlot.parent.parent, false);
        selectImage.transform.position = new Vector2(itemHadSlot.position.x, itemHadSlot.position.y + selectImageOffSetYWithHadItem);
        selectImage.transform.localScale = new Vector2(9.1f, 16.67f);
        selectImage.SetActive(true);

        currentSelectPerfectSwordEquipped = null;
        if(selectBigSlot.parent.Find("Sword Perfect").gameObject.activeInHierarchy)
            selectBigSlot.gameObject.SetActive(false);

        if (!CheckHadItemOnSlot(itemHadSlot, false))
        {
            DeactivateEquipButton();
            itemInforUI.SetActive(false);
            base.selectedItemImage.sprite = null;
            currentSelectHadSwordPiece = null;
            return;
        }
        Image itemImage;
        if (itemHadSlot.childCount == 2 || 
            (itemHadSlot.childCount == 3 && itemHadSlot.GetChild(1).name != "New Sign Image(Clone)"))
            itemImage = itemHadSlot.GetChild(1).GetComponent<Image>();
        else
            itemImage = itemHadSlot.GetChild(2).GetComponent<Image>();
        ChangeSelectedImageTransformBySelectedSwordPiece(itemImage, true);
        currentSelectHadSwordPiece = itemImage.transform;
        if (numOfEquippedItem < tempGameData.currentSwordPieceSlot && currentSelectEquippedSwordPieceParent != null
            && selectImage2.gameObject.activeInHierarchy)
            ActivateEquipButton();
        else
            DeactivateEquipButton();
        if (currentSelectEquippedSwordPieceParent != null)
            if (currentSelectEquippedSwordPieceParent.childCount > 0)
                DeactivateEquipButton();
        GetSwordPieceInforByItsImage(itemImage);
        itemInforUI.SetActive(true);
        RemoveNewSign(itemImage);
    }

    private void GetSwordPieceInforByItsImage(Image itemImage)
    {
        selectedItemFunction.gameObject.SetActive(false);
        foreach (Item item in Inventory.Instance.allSwordPieceItemsList)
        {
            if (itemImage.sprite == item.itemImage.sprite)
            {
                selectedItemName.text = item.itemName;
                //selectedItemDescription.text = item.itemDescription;
                selectedItemLore.text = item.itemLore;
                break;
            }
        }
    }
    public void GetPerfectSwordInforByItsImage(Image itemImage)
    {
        base.selectedItemImage.rectTransform.localScale = new Vector3(0.9f, 0.9f, 1f);
        base.selectedItemImage.rectTransform.pivot = itemImage.rectTransform.pivot;
        base.selectedItemImage.rectTransform.localRotation = Quaternion.identity;
        base.selectedItemImage.sprite = itemImage.sprite;
        base.selectedItemImage.gameObject.SetActive(true);
        selectedItemFunction.gameObject.SetActive(true);
        foreach (PerfectSwordSO item in Inventory.Instance.allPerfectSwordList)
        {
            if (itemImage.sprite == item.image)
            {
                selectedItemName.text = item.swordName;
                //selectedItemDescription.text = item.itemDescription;
                selectedItemFunction.text = item.originalFunction;
                int swordIndex = Inventory.Instance.GetItemIndexBySprite(ItemType.PerfectSword, itemImage.sprite);
                int pairIndex = perfectSwordChecker.GetPairIndexByOneSwordIndex(swordIndex);
                foreach(int pairId in tempGameData.swordPairsActivated)
                {
                    if(pairId == pairIndex)
                    {
                        selectedItemFunction.text = item.effectPairFunction;
                        break;
                    }
                }
                selectedItemLore.text = item.lore;
                break;
            }
        }
        itemInforUI.SetActive(true);
        DeactivateUnEquipButton();
    }
    public void SetSwordFunctionTMPOnChangePairEffect(bool active)
    {
        foreach(PerfectSwordSO item in Inventory.Instance.allPerfectSwordList)
        {
            if(selectedItemImage.sprite == item.image)
            {
                if(active)
                    selectedItemFunction.text = item.effectPairFunction;
                else 
                    selectedItemFunction.text = item.originalFunction;
                break;
            }
        }
    }
    public void CloseInforUI() => itemInforUI.SetActive(false);
    public void OpenInforUI() => itemInforUI.SetActive(true); 
    public void ChangeItemInforByPerfectSwordOnSuccessMerge(Image image)
    {
        selectImage2.gameObject.SetActive(false);
        DeactivateUnEquipButton();
        GetPerfectSwordInforByItsImage(image);
    }
    public void OnClickEquipSlot(Transform equipSlot)
    {
        if(perfectSwordChecker.isMerging)
            return;
        selectImage2.SetParent(equipSlot.parent, false);
        selectImage2.localPosition = new Vector2(equipSlot.localPosition.x, equipSlot.localPosition.y + selectImageOffSetYWithEquippedItem);
        selectImage2.gameObject.SetActive(true);

        selectBigSlot.SetParent(equipSlot.parent, true);
        selectBigSlot.SetSiblingIndex(2);
        selectBigSlot.localPosition = Vector2.zero;
        selectBigSlot.gameObject.SetActive(true);
        currentSelectEquippedSwordPieceParent = equipSlot;
        currentSelectPerfectSwordEquipped = null;
        if (!CheckHadItemOnSlot(equipSlot, true))
        {
            bool canActivateEquipButton = true;
            //if ((!selectImage.activeInHierarchy && selectedASwordPieceHadFirstTime)
            //    || (currentSelectPerfectSwordHad != null && !selectedASwordPieceHadFirstTime))
            if ((!selectImage.activeInHierarchy && selectedASwordPieceHadFirstTime) || currentSelectHadSwordPiece == null)
            {
                itemInforUI.SetActive(false);
                currentSelectHadSwordPiece = null;
                DeactivateEquipButton();
                canActivateEquipButton = false;
            }
            DeactivateUnEquipButton();
            if (currentSelectHadSwordPiece != null && currentSelectEquippedSwordPieceParent != null && canActivateEquipButton)
            {
                ActivateEquipButton();
                ChangeSelectedImageTransformBySelectedSwordPiece(currentSelectHadSwordPiece.GetComponent<Image>(), true);
                GetSwordPieceInforByItsImage(currentSelectHadSwordPiece.GetComponent<Image>());
            }
            return;
        }
        DeactivateEquipButton();
        ActivateUnEquipButton();
        Image itemImage = equipSlot.GetChild(0).GetComponent<Image>();
        ChangeSelectedImageTransformBySelectedSwordPiece(itemImage, false);
        currentSelectEquippedSwordPieceParent = equipSlot;
        GetSwordPieceInforByItsImage(itemImage);
        itemInforUI.SetActive(true);
    }
    private void ChangeSelectedImageTransformBySelectedSwordPiece(Image itemImage, bool selectOnHadItems)
    {
        if(selectOnHadItems)
            base.selectedItemImage.rectTransform.localScale = itemImage.rectTransform.localScale * selectedImageScaleModifier;
        else
            base.selectedItemImage.rectTransform.localScale = itemImage.rectTransform.localScale;
        base.selectedItemImage.rectTransform.pivot = itemImage.rectTransform.pivot;
        base.selectedItemImage.rectTransform.localRotation = itemImage.rectTransform.localRotation;
        base.selectedItemImage.sprite = itemImage.sprite;
        base.selectedItemImage.gameObject.SetActive(true);
    }
    public void DeactivateEquipButton()
    {
        Image equipBtnImage = equipButton.GetComponent<Image>();
        equipBtnImage.color = new Color(equipBtnImage.color.r, equipBtnImage.color.g, equipBtnImage.color.b, 50f / 255f);
        TextMeshProUGUI equipTxt = equipText.GetComponent<TextMeshProUGUI>();
        equipTxt.color = new Color(equipTxt.color.r, equipTxt.color.g, equipTxt.color.b, 50f / 255f);

        equipButton.GetComponent<Button>().enabled = false;
    }
    public void ActivateEquipButton()
    {
        Image equipBtnImage = equipButton.GetComponent<Image>();
        equipBtnImage.color = new Color(equipBtnImage.color.r, equipBtnImage.color.g, equipBtnImage.color.b, 255f / 255f);
        TextMeshProUGUI equipTxt = equipText.GetComponent<TextMeshProUGUI>();
        equipTxt.color = new Color(equipTxt.color.r, equipTxt.color.g, equipTxt.color.b, 255f / 255f);

        equipButton.GetComponent<Button>().enabled = true;
    }
    public void DeactivateUnEquipButton()
    {
        Image unEquipBtnImage = unEquipButton.GetComponent<Image>();
        unEquipBtnImage.color = new Color(unEquipBtnImage.color.r, unEquipBtnImage.color.g, unEquipBtnImage.color.b, 50f / 255f);
        TextMeshProUGUI unEquipTxt = unEquipText.GetComponent<TextMeshProUGUI>();
        unEquipTxt.color = new Color(unEquipTxt.color.r, unEquipTxt.color.g, unEquipTxt.color.b, 50f / 255f);

        unEquipButton.GetComponent<Button>().enabled = false;
    }
    public void ActivateUnEquipButton()
    {
        Image unEquipBtnImage = unEquipButton.GetComponent<Image>();
        unEquipBtnImage.color = new Color(unEquipBtnImage.color.r, unEquipBtnImage.color.g, unEquipBtnImage.color.b, 255f / 255f);
        TextMeshProUGUI unEquipTxt = unEquipText.GetComponent<TextMeshProUGUI>();
        unEquipTxt.color = new Color(unEquipTxt.color.r, unEquipTxt.color.g, unEquipTxt.color.b, 255f / 255f);

        unEquipButton.GetComponent<Button>().enabled = true;
    }
    public void EquipSwordPiece()
    {
        if(currentSelectPerfectSwordHad != null)
        {
            perfectSwordChecker.OnClickEquipPefectSword();
            return;
        }
        if(currentSelectEquippedSwordPieceParent != null)
        {
            if(currentSelectEquippedSwordPieceParent.childCount == 0 && currentSelectHadSwordPiece != null)
            {
                currentSelectHadSwordPiece.SetParent(currentSelectEquippedSwordPieceParent, true);
                currentSelectHadSwordPiece.localPosition = Vector2.zero;

                CloseInforUI();
                selectImage.SetActive(false);
                DeactivateEquipButton();
                DeactivateUnEquipButton();

                Inventory.Instance.EquipSwordPiece(swordPieceData.IndexOf(currentSelectHadSwordPiece), 
                    equippedItemImage.IndexOf(currentSelectEquippedSwordPieceParent.GetComponent<Image>()));
                numOfEquippedItem++;
                perfectSwordChecker.CheckEnoughPieceAndStartMerging();
                if (!perfectSwordChecker.isMerging)
                {
                    Transform nextSlot = currentSelectEquippedSwordPieceParent;
                    if (nextSlot.gameObject.name == "Slot4")
                        nextSlot = nextSlot.parent.GetChild(nextSlot.GetSiblingIndex() - 3);
                    else
                        nextSlot = nextSlot.parent.GetChild(nextSlot.GetSiblingIndex() + 1);
                    if (!CheckEquippedFullPieceOnBigSlot(nextSlot.parent))
                    {
                        while (nextSlot.childCount != 0)
                        {
                            if(nextSlot.gameObject.name == "Slot4")
                                nextSlot = nextSlot.parent.GetChild(nextSlot.GetSiblingIndex() - 3);
                            else
                                nextSlot = nextSlot.parent.GetChild(nextSlot.GetSiblingIndex()+1);  
                        }
                        selectImage2.SetParent(nextSlot.parent, false);
                        selectImage2.localPosition = new Vector2(nextSlot.localPosition.x, nextSlot.localPosition.y + selectImageOffSetYWithEquippedItem);
                        selectImage2.gameObject.SetActive(true);
                        currentSelectEquippedSwordPieceParent = nextSlot;
                    } else
                    {
                        ActivateUnEquipButton();
                        OpenInforUI();
                    }
                }
                else
                {
                    currentSelectEquippedSwordPieceParent = null;
                    selectImage2.gameObject.SetActive(false);
                }

                Debug.Log("Equip Sword piece!");
            }
        }
    }
    public bool CheckEquippedFullPieceOnBigSlot(Transform bigSlot)
    {
        for(int i=1; i<=4; i++)
        {
            if (bigSlot.Find("Slot" + i).childCount == 0)
                return false;
        }
        return true;
    }
    public void UnEquipSwordPiece()
    {
        if(selectBigSlot.parent.Find("Sword Perfect").gameObject.activeInHierarchy)
        {
            perfectSwordChecker.OnClickUnEquipPerfectSword();
            return;
        }
        if (currentSelectEquippedSwordPieceParent != null)
        {
            if (currentSelectEquippedSwordPieceParent.childCount != 0)
            {
                foreach (Image image in itemHadImage)
                {
                    if (image.transform.childCount == 1 || 
                        (image.transform.Find("New Sign Image(Clone)") && image.transform.childCount == 2))
                    {
                        Transform swordPieceToUnEquip = currentSelectEquippedSwordPieceParent.GetChild(0);
                        GetSwordPieceInforByItsImage(swordPieceToUnEquip.GetComponent<Image>());
                        ChangeSelectedImageTransformBySelectedSwordPiece(swordPieceToUnEquip.GetComponent<Image>(), false);
                        swordPieceToUnEquip.SetParent(image.transform, true);
                        swordPieceToUnEquip.localPosition = Vector2.zero;
                        currentSelectHadSwordPiece = swordPieceToUnEquip;
                        selectImage.transform.position = new Vector2(image.transform.position.x,
                            image.transform.position.y + selectImageOffSetYWithHadItem);
                        selectImage.transform.localScale = new Vector2(9.1f, 16.67f);
                        selectImage.gameObject.SetActive(true);
                        break;
                    }
                }
                ActivateEquipButton();
                DeactivateUnEquipButton();
                Debug.Log("UnEquip Sword piece!");
                Inventory.Instance.UnequipSwordPiece(equippedItemImage.IndexOf(currentSelectEquippedSwordPieceParent.GetComponent<Image>()));
                numOfEquippedItem--;
            }
        }
    }
    public void OnDestroyBlurSwordFx() => perfectSwordChecker.OnDestroyBlurSwordFx();
    public void OnCloseMergedSwordNoti() => perfectSwordChecker.OnCloseMergedSwordNoti();
    public void SetSwordPieceUIInteractable() => perfectSwordChecker.SetSwordPieceUIInteractable();
    public void SetSwordPieceUIUniteractable() => perfectSwordChecker.SetSwordPieceUIUninteractable();
    public void DeactivateSelectImage() => selectImage.gameObject.SetActive(false);
    public void OnClickSwordPieceHadTab()
    {
        if (swordPieceHadTab.localScale == new Vector3(1.25f, 1.25f, 1f))
            return;
        currentSelectPerfectSwordHad = null;
        DeactivateEquipButton();
        if (currentSelectPerfectSwordEquipped == null)
        {
            CloseInforUI();
            selectBigSlot.gameObject.SetActive(false);
        } else
        {
            GetPerfectSwordInforByItsImage(currentSelectPerfectSwordEquipped.GetComponent<Image>());
        }
        selectImage.SetActive(false);
        selectImage2.gameObject.SetActive(false);
        ActivateAllPieceSlotOnSwordPieceTabOpen();
        swordPieceHadUI.gameObject.SetActive(true);
        perfectSwordHadUI.gameObject.SetActive(false);
        swordPieceHadTab.localScale = new Vector3(1.25f, 1.25f, 1f);
        swordPieceHadTab.Find("CoverOnSelect").gameObject.SetActive(true);
        swordPieceHadTab.Find("SelectImage").gameObject.SetActive(true);

        perfectSwordHadTab.localScale = new Vector3(1f, 1f, 1f);
        perfectSwordHadTab.Find("CoverOnSelect").gameObject.SetActive(false);
        perfectSwordHadTab.Find("SelectImage").gameObject.SetActive(false);
    }
    public void OnClickPerfectSwordHadTab()
    {
        if (perfectSwordHadTab.localScale == new Vector3(1.25f, 1.25f, 1f))
            return;
        currentSelectHadSwordPiece = null;
        DeactivateEquipButton();
        perfectSwordChecker.perfectSwordSelectImage.gameObject.SetActive(false);
        if(currentSelectPerfectSwordEquipped == null && currentSelectPerfectSwordHad == null)
        {
            CloseInforUI();
            selectBigSlot.gameObject.SetActive(false);
        }
        DeactivateAllPieceSlotOnPerfectSwordTabOpen();
        perfectSwordHadUI.gameObject.SetActive(true);
        swordPieceHadUI.gameObject.SetActive(false);
        perfectSwordHadTab.localScale = new Vector3(1.25f, 1.25f, 1f);
        perfectSwordHadTab.Find("CoverOnSelect").gameObject.SetActive(true);
        perfectSwordHadTab.Find("SelectImage").gameObject.SetActive(true);

        swordPieceHadTab.localScale = new Vector3(1f, 1f, 1f);
        swordPieceHadTab.Find("CoverOnSelect").gameObject.SetActive(false);
        swordPieceHadTab.Find("SelectImage").gameObject.SetActive(false);
    }
    private void DeactivateAllPieceSlotOnPerfectSwordTabOpen()
    {
        selectImage2.gameObject.SetActive(false);
        foreach(Transform slot in bigSlot)
        {
            if (slot.Find("Sword Perfect").gameObject.activeSelf)
                continue;
            bool mustDeactivate = false;
            for (int i=1; i<=4; i++)
            {
                if(slot.Find("Slot" + i).childCount != 0)
                {
                    mustDeactivate = true;
                    break;
                }
                slot.Find("Slot" + i).gameObject.SetActive(false);
            }
            if (mustDeactivate)
            {
                for (int i = 1; i <= 4; i++)
                {
                    slot.Find("Slot" + i).gameObject.SetActive(true);
                    if(slot.Find("Slot" + i).childCount != 0)                
                        slot.Find("Slot" + i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 100f / 255f);

                }
                slot.GetComponent<CanvasGroup>().interactable = false;
            }
        }
    }
    private void ActivateAllPieceSlotOnSwordPieceTabOpen()
    {
        int cnt = -1;
        foreach (Transform slot in bigSlot)
        {
            cnt++;
            if(cnt < currentBigSlot)
                slot.GetComponent<CanvasGroup>().interactable = true;
            if (slot.Find("Sword Perfect").gameObject.activeSelf)
            {
                continue;
            }
            for(int i=1; i<=4; i++)
            {
                slot.Find("Slot" + i).gameObject.SetActive(true);
                if(slot.Find("Slot"+i).childCount != 0)
                    slot.Find("Slot" + i).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
    public void ChangeNumOfEquippedItem(bool isEquipPerfectSword)
    {
        if (isEquipPerfectSword)
            numOfEquippedItem += 4;
        else
            numOfEquippedItem -= 4;
    }
}
