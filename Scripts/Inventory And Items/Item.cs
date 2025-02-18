using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Important,
    Armorial,
    SwordPiece,
    PerfectSword
}
public class Item : MonoBehaviour
{
    public ItemType itemType;
    public string itemName;
    public int itemIndex;
    [Header("Lore And Function")]
    [TextArea] public string itemDescription;
    [TextArea] public string itemLore;
    public string itemFunction;
    public SpriteRenderer itemImage;
    public GameObject pickupIndicator;
    public bool canPickup;
    private void Start()
    {
        canPickup = false;
        itemImage = GetComponentInChildren<SpriteRenderer>();
        pickupIndicator.gameObject.SetActive(false);
        if (itemType == ItemType.Armorial)
            CheckHadThisArmorialOrNot();
        else if (itemType == ItemType.Important)
            CheckHadThisImportantItemOrNot();
        else if (itemType == ItemType.SwordPiece)
            CheckHadThisSwordPieceOrNot();
    }

    private void CheckHadThisArmorialOrNot()
    {
        if (SaveManager.instance.tempGameData.amorialHadItems != null)
        {
            foreach (int i in SaveManager.instance.tempGameData.amorialHadItems)
            {
                if (itemIndex == i)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
        }
        if (SaveManager.instance.tempGameData.amorialEquippedItems != null)
        {
            foreach (int i in SaveManager.instance.tempGameData.amorialEquippedItems)
            {
                if (itemIndex == i)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
        }
    }
    private void CheckHadThisImportantItemOrNot()
    {
        if (SaveManager.instance.tempGameData.importantHadItems != null)
        {
            foreach (int i in SaveManager.instance.tempGameData.importantHadItems)
            {
                if (itemIndex == i)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
        }
        if(SaveManager.instance.tempGameData.usedImportantItems != null)
        {
            foreach (int i in SaveManager.instance.tempGameData.usedImportantItems)
            {
                if (itemIndex == i)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
        }
    }
    private void CheckHadThisSwordPieceOrNot()
    {
        if (SaveManager.instance.tempGameData.swordPieceHadItems != null)
        {
            foreach (int i in SaveManager.instance.tempGameData.swordPieceHadItems)
            {
                if (itemIndex == i)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
        }
        if (SaveManager.instance.tempGameData.swordPieceEquippedItems != null)
        {
            foreach (int i in SaveManager.instance.tempGameData.swordPieceEquippedItems)
            {
                if (itemIndex == i)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
        }
        if (SaveManager.instance.tempGameData.swordPieceMergedItems != null)
        {
            foreach (int i in SaveManager.instance.tempGameData.swordPieceMergedItems)
            {
                if (itemIndex == i)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

    private void Update()
    {
        if(Player.Instance.currentItemPosX == transform.position.x)
        {
            if (Mathf.Abs(Player.Instance.transform.position.x - transform.position.x) > 1f)
            {
                canPickup = false;
                Player.Instance.canPickup = false;
            }
        }
        if (canPickup)
            pickupIndicator.SetActive(true);
        else
            pickupIndicator.SetActive(false);
        PickUpItem();
    }

    private void PickUpItem()
    {
        if (Player.Instance.pickupedItem && canPickup)
        {
            Player.Instance.pickupedItem = false;
            Inventory.Instance.AddItem(itemType, itemIndex);
            PickedUpItemNotification.Instance.ShowPickedUpItemNotification(Inventory.Instance.GetSpriteByItemIndex(this.itemType, this.itemIndex), itemName);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canPickup = true;
            Player.Instance.canPickup = true;
            Player.Instance.currentItemPosX = transform.position.x;
        }
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        canPickup = false;
    //        Player.Instance.canPickup = false;
    //    }
    //}
}

