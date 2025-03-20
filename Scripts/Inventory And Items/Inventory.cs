using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<Item> allArmorialItemsList;
    public List<Item> allImportantItemsList;
    public List<Item> allSwordPieceItemsList;
    public List<Item> allMagicGemItemsList;
    public List<PerfectSwordSO> allPerfectSwordList; 

    public List<int> amorialHadItems;
    public List<int> amorialEquippedItems;

    public List<int> importantHadItems;
    public List<int> usedImportantItems;

    public List<int> swordPieceHadItems;
    public List<int> swordPieceEquippedItems;
    private GameDatas tempGameData;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else 
            Instance = this;
        foreach(PerfectSwordSO sw in Resources.LoadAll<PerfectSwordSO>("PerfectSword"))
        {
            allPerfectSwordList.Add(sw);
        }
    }
    private void Start()
    {
        tempGameData = SaveManager.instance.tempGameData;
        if(tempGameData.amorialHadItems != null && tempGameData.amorialHadItems.Count > 0)
            this.amorialHadItems = tempGameData.amorialHadItems;
        if (tempGameData.amorialEquippedItems != null && tempGameData.amorialEquippedItems.Count > 0)
            this.amorialEquippedItems = tempGameData.amorialEquippedItems;
        else
        {
            for(int i=0; i<9; i++)
            {
                amorialEquippedItems.Add(-1);
            }
        }
        if (tempGameData.importantHadItems != null && tempGameData.importantHadItems.Count > 0)
            this.importantHadItems = tempGameData.importantHadItems;
        if (tempGameData.usedImportantItems != null && tempGameData.usedImportantItems.Count > 0)
            this.usedImportantItems = tempGameData.usedImportantItems;
        if (tempGameData.swordPieceHadItems != null && tempGameData.swordPieceHadItems.Count > 0)
            this.swordPieceHadItems = tempGameData.swordPieceHadItems;
        if (tempGameData.swordPieceEquippedItems != null && tempGameData.swordPieceEquippedItems.Count > 0)
            this.swordPieceEquippedItems = tempGameData.swordPieceEquippedItems;
        else
        {
            for (int i = 0; i < 32; i++)
            {
                swordPieceEquippedItems.Add(-1);
            }
        }       
    }
    public void AddItem(ItemType itemType, int itemIndex)
    {
        if (itemType == ItemType.Important)
        {
            importantHadItems.Add(itemIndex);
            tempGameData.importantHadItems = importantHadItems;
            ImportantItemUI.Instance.AddNewItemSign();
            ImportantItemUI.Instance.LoadHadItemUI();
            ImportantItemUI.Instance.DeactiveSelectNullImageOnPickupNewItem();
        }
        else if (itemType == ItemType.Armorial)
        {
            amorialHadItems.Add(itemIndex);
            tempGameData.amorialHadItems = amorialHadItems;
            ArmorialUI.Instance.AddNewItemSign();
            ArmorialUI.Instance.LoadHadItemUI();
            ArmorialUI.Instance.DeactiveSelectNullImageOnPickupNewItem();
        }
        else if (itemType == ItemType.SwordPiece)
        {
            swordPieceHadItems.Add(itemIndex);
            tempGameData.swordPieceHadItems = swordPieceHadItems;
            SwordPieceUI.Instance.LoadHadItemUI();
            SwordPieceUI.Instance.AddNewItemSign();
            SwordPieceUI.Instance.DeactiveSelectNullImageOnPickupNewItem();
        } else 
        { 
            tempGameData.magicGemHadItems.Add(itemIndex);
            MagicSkillUI.Instance.AddNewItemSign();
            MagicSkillUI.Instance.LoadHadItemUI();
            MagicSkillUI.Instance.DeactiveSelectNullImageOnPickupNewItem();
        }
    }
    public Sprite GetSpriteByItemIndex(ItemType type, int index)
    {
        if (type == ItemType.Important)
        {
            foreach (Item item in allImportantItemsList)
            {
                if (item.itemIndex == index)
                    return item.itemImage.sprite;
            }
        }
        else if (type == ItemType.Armorial)
        {
            foreach (Item item in allArmorialItemsList)
            {
                if (item.itemIndex == index)
                    return item.itemImage.sprite;
            }
        }
        else if (type == ItemType.SwordPiece)
        {
            foreach (Item item in allSwordPieceItemsList)
            {
                if (item.itemIndex == index)
                    return item.itemImage.sprite;
            }
        }
        else if (type == ItemType.PerfectSword)
        {
            foreach (PerfectSwordSO item in allPerfectSwordList)
            {
                if (item.index == index)
                    return item.image;
            }
        } else 
        {
            foreach (Item item in allMagicGemItemsList)
            {
                if (item.itemIndex == index)
                    return item.itemImage.sprite;
            }
        }
        return null;
    }
    public int GetItemIndexBySprite(ItemType itemType, Sprite sprite)
    {
        switch (itemType)
        {
            case ItemType.Armorial:
            foreach(Item item in allArmorialItemsList)
            {
                if(item.itemImage.sprite == sprite)
                {
                    return item.itemIndex;
                }
            } break;
            case ItemType.SwordPiece:
                foreach (Item item in allSwordPieceItemsList)
                {
                    if (item.itemImage.sprite == sprite)
                    {
                        return item.itemIndex;
                    }
                }
            break;
            case ItemType.PerfectSword:
                foreach (PerfectSwordSO item in allPerfectSwordList)
                {
                    if (item.image == sprite)
                    {
                        return item.index;
                    }
                }
            break;
            case ItemType.MagicGem:
                foreach (Item item in allMagicGemItemsList)
                {
                    if (item.itemImage.sprite == sprite)
                    {
                        return item.itemIndex;
                    }
                }
            break;
        }
        return -1;
    }
    public bool CheckSelectedItemIsEquipped(ItemType itemType, Sprite sprite)
    {
        int index = GetItemIndexBySprite(itemType, sprite);
        if(tempGameData.amorialEquippedItems == null || 
            tempGameData.amorialEquippedItems.Count < 1)
            return false;
        foreach(int i in tempGameData.amorialEquippedItems)
        {
            if(index == i) 
                return true;
        }
        return false;
    }
    public void EquipArmorial(int index)
    {
        int cnt = 0;
        foreach(int i in amorialEquippedItems)
        {
            if (i == -1)
            {
                amorialEquippedItems[cnt] = index;
                break;
            }
            cnt++;
        }
        tempGameData.amorialEquippedItems = amorialEquippedItems;
        Player.Instance.playerStatsWithItems.EquipArmorial(index);
    }
    public void UnequipArmorial(int index)
    {
        int cnt = 0;
        foreach(int i in amorialEquippedItems)
        {
            if(i == index)
            {
                amorialEquippedItems[cnt] = -1;
                break;
            }
            cnt++;
        }
        tempGameData.amorialEquippedItems = amorialEquippedItems;
        Player.Instance.playerStatsWithItems.UnequipArmorial(index);
    }
    public void EquipSwordPiece(int swordPieceindex, int index)
    {
        swordPieceEquippedItems[index] = swordPieceindex;
        tempGameData.swordPieceEquippedItems = swordPieceEquippedItems;
    }
    public void UnequipSwordPiece(int index)
    {
        swordPieceEquippedItems[index] = -1;
        tempGameData.swordPieceEquippedItems = swordPieceEquippedItems;
    }
}
