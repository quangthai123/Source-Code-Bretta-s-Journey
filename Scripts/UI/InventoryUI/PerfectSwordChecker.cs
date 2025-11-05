using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerfectSwordChecker : MonoBehaviour
{
    private SwordPieceUI swordPieceUI;
    private List<int> equippedSwordPieceList;
    //[SerializeField] private List<Transform> perfectSwordData;
    [SerializeField] private int mergeIndex;
    public float mergeSpeed = 1f;
    [SerializeField] int[] swordPiecesToCheck = new int[4];
    [SerializeField] private Transform perfectSwordFxPrefab;
    [SerializeField] private Transform blurSwordFxPrefab;
    [SerializeField] private Transform blurSwordFx2Prefab;
    [SerializeField] private GameObject mergeSuccessNoti;
    [SerializeField] private GameObject mergeFailedNoti;
    private ActivatedSwordPairNoti activatedSwordPairNoti;
    private Transform bigSlotMerging;
    [SerializeField] private ScrollRect hadSwordPieceScrollRect;
    [SerializeField] private ScrollRect hadSwordScrollRect;
    [SerializeField] private List<Transform> swordContainers;
    public Transform perfectSwordSelectImage;
    [SerializeField] private List<Transform> pairs;
    [SerializeField] private float activePairDuration = .5f;
    public bool isMerging { get; private set; } = false;
    private Dictionary<int, int> swordPairsIndex = new Dictionary<int, int>();
    private GameDatas tempGameData;
    void Start()
    {
        tempGameData = SaveManager.instance.tempGameData;
        swordPieceUI = GetComponent<SwordPieceUI>();
        activatedSwordPairNoti = transform.Find("Activate Pair Noti").GetComponent<ActivatedSwordPairNoti>();
        LoadSwordPairsIndex();
        LoadPerfectSwordHad();
        LoadPerfectSwordEquipped();
        LoadHadPerfectSwordBorder();
    }

    private void LoadSwordPairsIndex()
    {
        swordPairsIndex.Add(0, 4);
        swordPairsIndex.Add(8, 12);
        swordPairsIndex.Add(16, 20);
        swordPairsIndex.Add(24, 28);
        swordPairsIndex.Add(32, 36);
        swordPairsIndex.Add(40, 44);
        swordPairsIndex.Add(48, 52);
        swordPairsIndex.Add(56, 60);
    }

    private void Update()
    {
        if(equippedSwordPieceList == null)
            equippedSwordPieceList = tempGameData.swordPieceEquippedItems;       
    }
    private void LoadPerfectSwordEquipped()
    {
        int cnt = 0;
        List<int> perfectSwordsEquipped = tempGameData.perfectSwordsEquipped;
        foreach (int index in perfectSwordsEquipped)
        {
            if(index != -1)
            {
                for (int i = 1; i <= 4; i++)
                {
                    swordPieceUI.bigSlot[cnt].Find("Slot" + i).gameObject.SetActive(false);
                }
                swordPieceUI.bigSlot[cnt].Find("Sword Perfect").GetComponent<Image>().sprite = Inventory.Instance.GetSpriteByItemIndex(ItemType.PerfectSword, index); // index cua perfect sword bang index cua sword piece dau tien
                swordPieceUI.bigSlot[cnt].Find("Sword Perfect").gameObject.SetActive(true);
            }
            cnt++;
        }
        for (int i = 0; i <= 6; i += 2)
        {
            CheckAndActivePair(i, true);
        }
    }
    private void LoadPerfectSwordHad()
    {
        if (tempGameData.perfectSwordsHad.Count < 1)
            return;
        int cnt = 0;
        foreach (Transform transf in swordContainers)
        {
            if(!transf.GetChild(0).gameObject.activeInHierarchy)
            {
                transf.GetChild(0).GetComponent<Image>().sprite = Inventory.Instance.GetSpriteByItemIndex(ItemType.PerfectSword,
                    tempGameData.perfectSwordsHad[cnt++]);
                transf.GetChild(0).gameObject.SetActive(true);
                transf.parent.GetChild(1).gameObject.SetActive(true);
                if (cnt == tempGameData.perfectSwordsHad.Count)
                    break;
            }
        }
        //LoadHadPerfectSwordBorder();
    }
    private void LoadHadPerfectSwordBorder()
    {
        int numEquipped = 0;
        foreach(int i in tempGameData.perfectSwordsEquipped)
        {
            if(i!=-1)
                numEquipped++;
        }
        int num = tempGameData.perfectSwordsHad.Count + numEquipped;
        for(int i = 0; i < num; i++)
        {
            swordContainers[i].parent.GetChild(1).gameObject.SetActive(true);    
        }
    }
    public void CheckEnoughPieceAndStartMerging()
    {
        isMerging = false;
        int cnt = 0;
        mergeIndex = -1;
        foreach (Transform bigSlot in swordPieceUI.bigSlot)
        {
            if (bigSlot.GetComponent<CanvasGroup>().interactable == false)
                continue;
            bool canMerge = true;
            mergeIndex++;
            for (int i = 0; i < 4; i++)
                swordPiecesToCheck[i] = -1;
            for(int i=0; i<4; i++)
            {
                int j = i + 1;
                if(bigSlot.transform.Find("Slot"+j).gameObject.activeInHierarchy)
                {
                    if(bigSlot.transform.Find("Slot"+j).childCount == 0)
                    {
                        canMerge = false;
                        Debug.Log("Cannot merge cause slot empty");
                        cnt += 4-i;
                        break;
                    }
                    else
                    {
                        swordPiecesToCheck[i] = equippedSwordPieceList[cnt];
                    }
                } else
                {
                    canMerge = false;
                    Debug.Log("Cannot merge cause slot be deactive");
                    cnt += 4-i;
                    break;
                }
                cnt++;
            }
            if(canMerge)
            {
                for(int i=0; i<3; i++)
                {
                    if (swordPiecesToCheck[i] != swordPiecesToCheck[i+1]-1)
                    {
                        canMerge = false;
                        //SetSwordPieceUIUninteractable();
                        mergeFailedNoti.SetActive(true);
                        Debug.Log("Have some wrong pieces!");
                        break;
                    }
                }
            }
            if (canMerge)
            {
                Debug.Log("Start Merge");
                isMerging = true;
                MergeToPerfectSword();
                return;
            }
        }
    }
    private void MergeToPerfectSword()
    {
        bigSlotMerging = swordPieceUI.bigSlot[mergeIndex];
        swordPieceUI.currentSelectPerfectSwordEquipped = bigSlotMerging.Find("Sword Perfect");

        swordPieceUI.selectImage2.gameObject.SetActive(false);
        swordPieceUI.CloseInforUI();
        swordPieceUI.DeactivateUnEquipButton();
        SetSwordPieceUIUninteractable();
        bigSlotMerging.GetComponent<CanvasGroup>().ignoreParentGroups = true;
        SetMergeFxAnim();
        //StartCoroutine(MoveSmallSlotToBigSlotCenter());
    }

    public void SetSwordPieceUIUninteractable()
    {
        InventoryUI.Instance.SetInventoryUIUninteractable();      
        swordPieceUI.GetComponent<CanvasGroup>().interactable = false;
        hadSwordPieceScrollRect.enabled = false;
        hadSwordScrollRect.enabled = false;
    }
    public void SetSwordPieceUIInteractable()
    {
        InventoryUI.Instance.SetInventoryUIInteractable();    
        swordPieceUI.GetComponent<CanvasGroup>().interactable = true;
        hadSwordPieceScrollRect.enabled = true;
        hadSwordScrollRect.enabled = true;
    }
    private void SetMergeFxAnim() 
    {
        for(int i = 1; i <= 4; i++)
        {
            //bigSlotMerging.Find("Slot" + i).GetComponent<Image>().enabled = false;
            bigSlotMerging.Find("Slot" + i).GetComponent<Animator>().SetTrigger("Merge");
        }
        StartCoroutine(SetMergeSuccesful());
    }
    private IEnumerator SetMergeSuccesful()
    {
        yield return new WaitForSecondsRealtime(3.2f);
        Debug.Log("Merge Successfully!!!");
        GenerateFxOnMerging();
        ReloadHadSwordPieceAndEquippedSwordPiece();
        LoadHadPerfectSwordBorder();
    }
    private void GenerateFxOnMerging()
    {
        Transform fx = Instantiate(perfectSwordFxPrefab, bigSlotMerging.transform.position, Quaternion.identity);
        fx.SetParent(transform.Find("Swords"), false);
        fx.localPosition = bigSlotMerging.localPosition;
        fx.SetSiblingIndex(8);
        fx.gameObject.SetActive(true);
        Transform fx2;
        if (mergeIndex < 4)
            fx2 = Instantiate(blurSwordFxPrefab, bigSlotMerging.transform.position, Quaternion.Euler(0f, 0f, -45f));
        else
            fx2 = Instantiate(blurSwordFx2Prefab, bigSlotMerging.transform.position, Quaternion.Euler(0f, 0f, -45f));
        fx2.SetParent(transform.Find("Swords"), false);
        fx2.localPosition = bigSlotMerging.localPosition + new Vector3(0f, -30f, 0f);
        fx2.SetSiblingIndex(9);
        fx2.GetComponent<Image>().sprite = Inventory.Instance.GetSpriteByItemIndex(ItemType.PerfectSword, swordPiecesToCheck[0]);
        fx2.gameObject.SetActive(true);
    }

    private void ReloadHadSwordPieceAndEquippedSwordPiece()
    {
        for (int i = 1; i <= 4; i++)
        {
            bigSlotMerging.Find("Slot" + i).gameObject.SetActive(false);
            tempGameData.swordPieceHadItems.Remove(swordPiecesToCheck[i - 1]);
            tempGameData.swordPieceEquippedItems[tempGameData.swordPieceEquippedItems.IndexOf(swordPiecesToCheck[i - 1])] = -1;
            tempGameData.swordPieceMergedItems.Add(swordPiecesToCheck[i - 1]);
        }
        tempGameData.perfectSwordsEquipped[mergeIndex] = swordPiecesToCheck[0];
        Player.Instance.playerStatsWithItems.EquipPerfectSword(swordPiecesToCheck[0]);
        swordPieceUI.LoadHadItemUI();
        swordPieceUI.LoadEquippedItemUI();
        swordPieceUI.selectBigSlot.SetSiblingIndex(1);
        swordPieceUI.ReloadNewSignsOnMergedSword();
    }
    public void OnDestroyBlurSwordFx()
    {
        bigSlotMerging.Find("Sword Perfect").GetComponent<Image>().sprite = Inventory.Instance.GetSpriteByItemIndex(ItemType.PerfectSword, swordPiecesToCheck[0]); // index cua perfect sword bang index cua sword piece dau tien
        bigSlotMerging.Find("Sword Perfect").gameObject.SetActive(true);
        Sprite perfectSwordSprite = bigSlotMerging.Find("Sword Perfect").GetComponent<Image>().sprite;
        string perfectSwordName = string.Empty;
        foreach (PerfectSwordSO item in Inventory.Instance.allPerfectSwordList)
        {
            if (perfectSwordSprite == item.image)
            {
                perfectSwordName = item.swordName;
                break;
            }
        }
        mergeSuccessNoti.GetComponent<MergedSwordNoti>().GetPerfectSwordInfor(perfectSwordSprite, perfectSwordName);
        mergeSuccessNoti.SetActive(true);
        isMerging = false;
    }
    public void OnCloseMergedSwordNoti()
    {
        SetSwordPieceUIInteractable();
        bigSlotMerging.GetComponent<CanvasGroup>().ignoreParentGroups = false;
        swordPieceUI.GetPerfectSwordInforByItsImage(bigSlotMerging.Find("Sword Perfect").GetComponent<Image>());
        swordPieceUI.OpenInforUI();
        swordPieceUI.ActivateUnEquipButton();
        CheckAndActivePair(mergeIndex, false);
    }
    public void OnClickPerfectSwordEquipped(Image image)
    {
        swordPieceUI.selectImage2.gameObject.SetActive(false);
        swordPieceUI.DeactivateSelectImage();
        Image perfectSwordImage = image.transform.parent.Find("Sword Perfect").GetComponent<Image>();
        swordPieceUI.selectBigSlot.SetParent(perfectSwordImage.transform.parent, false);
        swordPieceUI.selectBigSlot.SetSiblingIndex(1);
        swordPieceUI.selectBigSlot.localPosition = Vector2.zero;
        swordPieceUI.selectBigSlot.gameObject.SetActive(true);
        if (!image.transform.parent.Find("Sword Perfect").gameObject.activeSelf)
        {
            swordPieceUI.CloseInforUI();
            swordPieceUI.currentSelectPerfectSwordEquipped = null;
            if(swordPieceUI.currentSelectPerfectSwordHad != null)
            {
                swordPieceUI.ActivateEquipButton();
                swordPieceUI.GetPerfectSwordInforByItsImage(swordPieceUI.currentSelectPerfectSwordHad.GetComponent<Image>());
                swordPieceUI.OpenInforUI();
            }
            swordPieceUI.DeactivateUnEquipButton();
            return;
        }
        swordPieceUI.currentSelectPerfectSwordEquipped = image.transform.parent.Find("Sword Perfect");
        swordPieceUI.GetPerfectSwordInforByItsImage(perfectSwordImage);
        swordPieceUI.DeactivateEquipButton();
        swordPieceUI.ActivateUnEquipButton();
    }
    public void OnClickUnEquipPerfectSword()
    {
        swordPieceUI.currentSelectPerfectSwordEquipped = null;
        Sprite perfectSwordSprite = swordPieceUI.selectBigSlot.parent.Find("Sword Perfect").GetComponent<Image>().sprite;
        swordPieceUI.selectBigSlot.parent.Find("Sword Perfect").gameObject.SetActive(false);
        for (int i = 1; i <= 4; i++)
        {
            Transform slot = swordPieceUI.selectBigSlot.parent.Find("Slot" + i);
            switch(i)
            {
                case 1:
                    slot.localPosition = new Vector3(0f, 135.2f, 0f);
                    break;
                case 2:
                    slot.localPosition = new Vector3(0f, 35.2f, 0f);
                    break;
                case 3:
                    slot.localPosition = new Vector3(0f, -64.8f, 0f);
                    break;
                case 4:
                    slot.localPosition = new Vector3(0f, -164.8f, 0f);
                    break;
            }
            if (slot.childCount != 0)
            {
                slot.GetChild(0).gameObject.SetActive(false);
                slot.GetChild(0).SetParent(slot.parent, true);
            }
            if(!swordPieceUI.perfectSwordHadUI.gameObject.activeInHierarchy)
                slot.gameObject.SetActive(true);
        }
        Transform slotToContain = swordContainers[0];
        foreach(Transform container in swordContainers)
        {
            if(!container.GetChild(0).gameObject.activeSelf) 
            {
                slotToContain = container;
                container.GetChild(0).GetComponent<Image>().sprite = perfectSwordSprite;
                container.GetChild(0).gameObject.SetActive(true);
                perfectSwordSelectImage.transform.position = container.parent.position + new Vector3(0f, 8.67f, 0f);
                perfectSwordSelectImage.gameObject.SetActive(true);
                swordPieceUI.currentSelectPerfectSwordHad = container.GetChild(0);
                swordPieceUI.GetPerfectSwordInforByItsImage(container.GetChild(0).GetComponent<Image>());
                break;
            }
        }
        tempGameData.perfectSwordsHad.Add(Inventory.Instance.GetItemIndexBySprite(ItemType.PerfectSword, perfectSwordSprite));
        for (int i = 0; i < swordPieceUI.bigSlot.Count; i++)
        {
            if (swordPieceUI.bigSlot[i].Find("SelectBigSlot"))
            {
                Debug.Log("Deactivate pair " + i);
                DeactivePair(i);
                break;
            }
        }
        for(int i = 0; i < tempGameData.perfectSwordsEquipped.Count; i++)
        {
            if(tempGameData.perfectSwordsEquipped[i] == Inventory.Instance.GetItemIndexBySprite(ItemType.PerfectSword, perfectSwordSprite))
            {
                tempGameData.perfectSwordsEquipped[i] = -1;
                break;
            }
        }
        swordPieceUI.OnClickPerfectSwordHadTab();
        swordPieceUI.OpenInforUI();
        swordPieceUI.ActivateEquipButton();
        swordPieceUI.ChangeNumOfEquippedItem(false);
        perfectSwordSelectImage.position = slotToContain.position + new Vector3(0f, 8.67f, 0f);
        perfectSwordSelectImage.gameObject.SetActive(true);
        Player.Instance.playerStatsWithItems.UnEquipPerfectSword(Inventory.Instance.GetItemIndexBySprite(ItemType.PerfectSword, perfectSwordSprite));
    }
    public void OnClickPerfectSwordHad(Transform slot)
    {
        perfectSwordSelectImage.position = slot.position + new Vector3(0f, 8.67f, 0f);
        perfectSwordSelectImage.gameObject.SetActive(true);
        if (!slot.GetChild(0).GetChild(0).gameObject.activeSelf)
        {
            swordPieceUI.CloseInforUI();
            swordPieceUI.DeactivateEquipButton();
            swordPieceUI.currentSelectPerfectSwordHad = null;
            return;
        }       
        if(swordPieceUI.currentSelectPerfectSwordEquipped == null && swordPieceUI.selectBigSlot.gameObject.activeInHierarchy)
            swordPieceUI.ActivateEquipButton();
        else
            swordPieceUI.DeactivateEquipButton();
        Image perfectSwordImage = slot.GetChild(0).GetChild(0).GetComponent<Image>();
        swordPieceUI.currentSelectPerfectSwordHad = slot.GetChild(0).GetChild(0);
        swordPieceUI.GetPerfectSwordInforByItsImage(perfectSwordImage);
        swordPieceUI.OpenInforUI();
    }
    public void OnClickEquipPefectSword()
    {
        perfectSwordSelectImage.gameObject.SetActive(false);
        swordPieceUI.DeactivateEquipButton();
        swordPieceUI.ActivateUnEquipButton();
        Image perfectSwordImage = swordPieceUI.currentSelectPerfectSwordHad.GetComponent<Image>();
        swordPieceUI.currentSelectPerfectSwordHad.gameObject.SetActive(false);
        swordPieceUI.currentSelectPerfectSwordHad = null;
        swordPieceUI.currentSelectPerfectSwordEquipped = swordPieceUI.selectBigSlot.parent.Find("Sword Perfect");
        swordPieceUI.currentSelectPerfectSwordEquipped.GetComponent<Image>().sprite = perfectSwordImage.sprite;
        swordPieceUI.currentSelectPerfectSwordEquipped.gameObject.SetActive(true);
        swordPieceUI.ChangeNumOfEquippedItem(true);
        int swordID = Inventory.Instance.GetItemIndexBySprite(ItemType.PerfectSword, perfectSwordImage.sprite);
        tempGameData.perfectSwordsHad.Remove(swordID);
        for (int i=0; i < swordPieceUI.bigSlot.Count; i++)
        {
            if (swordPieceUI.bigSlot[i].Find("SelectBigSlot"))
            {
                tempGameData.perfectSwordsEquipped[i] = swordID;
                CheckAndActivePair(i, false);
                break;
            }
        }
        Player.Instance.playerStatsWithItems.EquipPerfectSword(swordID);
    }
    public void CheckAndActivePair(int index, bool onLoadScene) // index of bigSlot
    {
        bool canActivePair = false;
        int pairIndex = 0;
        int index2 = index + 1;
        List<int> equippedSwords = tempGameData.perfectSwordsEquipped;
        if(index == 0 || index % 2 == 0)
        {
            foreach (KeyValuePair<int, int> pair in swordPairsIndex)
            {
                if ((pair.Key == equippedSwords[index] && pair.Value == equippedSwords[index+1]) 
                    || (pair.Key == equippedSwords[index+1] && pair.Value == equippedSwords[index]))
                {
                    canActivePair = true;
                    break;
                }
                pairIndex++;
            }
        } else
        {
            foreach (KeyValuePair<int, int> pair in swordPairsIndex)
            {
                if ((pair.Key == equippedSwords[index] && pair.Value == equippedSwords[index - 1])
                    || (pair.Key == equippedSwords[index - 1] && pair.Value == equippedSwords[index]))
                {
                    canActivePair = true;
                    index2 = index - 1;
                    break;
                }
                pairIndex++;
            }
        }
        if(canActivePair)
        {
            Debug.Log($"Activate pair {pairIndex}");
            ActiveEffectOnUIAndPlayerStats(index, index2, onLoadScene);
            SetPairPosition(index, pairIndex);
            if(!onLoadScene)
            {
                SetSwordPieceUIUninteractable();
                StartCoroutine(ActivatePair(pairIndex));
            }
            else
                pairs[pairIndex].GetComponent<CanvasGroup>().alpha = 1f;
        } 
    }
    public int GetPairIndexByOneSwordIndex(int index)
    {
        int result = 0;
        switch(index)
        {
            case 8:
            case 12:
                result = 1; 
                break;
        }
        return result;
    }
    private void ActiveEffectOnUIAndPlayerStats(int index, int index2, bool onLoadScene)
    {
        swordPieceUI.SetSwordFunctionTMPOnChangePairEffect(true);
        Image image1 = swordPieceUI.bigSlot[index].Find("Sword Perfect").GetComponent<Image>();
        Image image2 = swordPieceUI.bigSlot[index2].Find("Sword Perfect").GetComponent<Image>();
        foreach (PerfectSwordSO sword in Inventory.Instance.allPerfectSwordList)
        {
            if (sword.image == image1.sprite)
            {
                if(!onLoadScene)
                    activatedSwordPairNoti.SetSword1Infor(sword);
            }
            if (sword.image == image2.sprite)
            {
                if (!onLoadScene)
                    activatedSwordPairNoti.SetSword2Infor(sword);
            }
        }
        List<int> equippedSwords = tempGameData.perfectSwordsEquipped;
        int pairIndex = GetPairIndexByOneSwordIndex(equippedSwords[index]);
        if (!onLoadScene)
            activatedSwordPairNoti.SetPairIndex(pairIndex);
        if (tempGameData.swordPairsActivated.Contains(pairIndex))
            return;
        tempGameData.swordPairsActivated.Add(pairIndex);
        Player.Instance.playerStatsWithItems.ActivateSwordPair(pairIndex);
    }
    private void DeactiveEffectOnUIAndPlayerStats(int index, int index2)
    {
        swordPieceUI.SetSwordFunctionTMPOnChangePairEffect(false);
        Image image1 = swordPieceUI.bigSlot[index].Find("Sword Perfect").GetComponent<Image>();
        Image image2 = swordPieceUI.bigSlot[index2].Find("Sword Perfect").GetComponent<Image>();
        //foreach (PerfectSwordSO sword in Inventory.Instance.allPerfectSwordList)
        //{
        //    if (sword.image == image1.sprite)
        //    {
        //        sword.RemoveEffect();
        //        swordPieceUI.SetSwordFunctionTMPOnActivePairEffect();
        //    }
        //    if (sword.image == image2.sprite)
        //    {
        //        sword.RemoveEffect();
        //        swordPieceUI.SetSwordFunctionTMPOnActivePairEffect(sword.function);
        //    }
        //}
        List<int> equippedSwords = tempGameData.perfectSwordsEquipped;
        int pairIndex = GetPairIndexByOneSwordIndex(equippedSwords[index]);
        tempGameData.swordPairsActivated.Remove(pairIndex);
        Player.Instance.playerStatsWithItems.DeactivateSwordPair(pairIndex);
    }
    private void DeactivePair(int index)
    {
        //InventoryUI.Instance.SetInventoryUIUninteractable();
        SetSwordPieceUIUninteractable();
        if (index==0 || index%2==0)
            DeactiveEffectOnUIAndPlayerStats(index, index+1);
        else
            DeactiveEffectOnUIAndPlayerStats(index, index - 1);
        List<int> equippedSwords = tempGameData.perfectSwordsEquipped;
        StartCoroutine(DeactivatePair(GetPairIndexByOneSwordIndex(equippedSwords[index])));
    }
    private void SetPairPosition(int index, int pairIndex)
    {       
        if (index == 0 || index == 1)
            pairs[pairIndex].localPosition = new Vector2(-280f, 193f);
        else if (index == 2 || index == 3)
            pairs[pairIndex].localPosition = new Vector2(380f, 193f);
        else if (index == 4 || index == 5)
            pairs[pairIndex].localPosition = new Vector2(-280f, -353f);
        else if (index == 6 || index == 7)
            pairs[pairIndex].localPosition = new Vector2(380f, -353f);
    }
    private IEnumerator ActivatePair(int pairIndex)
    {
        while (pairs[pairIndex].GetComponent<CanvasGroup>().alpha != 1) 
        {
            yield return new WaitForSecondsRealtime(this.activePairDuration/10f);
            pairs[pairIndex].GetComponent<CanvasGroup>().alpha += .1f;
        }
        activatedSwordPairNoti.gameObject.SetActive(true);
    }
    private IEnumerator DeactivatePair(int pairIndex)
    {
        while (pairs[pairIndex].GetComponent<CanvasGroup>().alpha != 0)
        {
            yield return new WaitForSecondsRealtime(this.activePairDuration / 10f);
            pairs[pairIndex].GetComponent<CanvasGroup>().alpha -= .1f;
        }
        //InventoryUI.Instance.SetInventoryUIInteractable();
        SetSwordPieceUIInteractable();
    }
}
