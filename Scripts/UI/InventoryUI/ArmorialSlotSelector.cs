using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorialSlotSelector : SlotSelectorController
{
    [SerializeField] private bool isMovingOnHadItems = true;
    [SerializeField] protected Image[] equipSlots;
    public int equipRowCount = 2;      
    public int equipColumnCount = 1;      
    private int equipTotalSlots = 2;    
    [SerializeField] private int equipCurrentIndex = 0;
    [SerializeField] private ArmorialUI armorialUI;
    protected override void Start()
    {
        base.Start();
        equipSlots = currentInventory.equippedItemImage.ToArray();
        SetEquipColumnRow();
        isMovingOnHadItems = true;
    }
    protected void OnEnable()
    {
        if (SaveManager.instance != null && SaveManager.instance.tempGameData != null)
        {
            SetEquipColumnRow();
        }
    }

    private void SetEquipColumnRow()
    {
        equipTotalSlots = SaveManager.instance.tempGameData.currenArmorialSlot;
        equipRowCount = equipTotalSlots > 2 ? 3 : 2;
        equipColumnCount = Mathf.CeilToInt((float)equipTotalSlots / equipRowCount);
    }

    protected override void Update()
    {
        if (InventoryUI.Instance.FrezeeInventoryAction)
            return;
        if(isMovingOnHadItems) 
        {
            if (Input.GetKeyDown(KeyCode.A))
                Move(-1);
            else if (Input.GetKeyDown(KeyCode.D))
                Move(1);
            else if (Input.GetKeyDown(KeyCode.W))
                Move(-columnCount);
            else if (Input.GetKeyDown(KeyCode.S))
                Move(columnCount);
            if(Input.GetKeyDown(KeyCode.Return)) 
            {
                EquipArmorial();
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.A))
                MoveOnEquipItems(-1);
            else if (Input.GetKeyDown(KeyCode.D))
                MoveOnEquipItems(1);
            else if (Input.GetKeyDown(KeyCode.W))
                MoveOnEquipItems(-equipRowCount);
            else if (Input.GetKeyDown(KeyCode.S))
                MoveOnEquipItems(equipRowCount);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                UnEquipArmorial();
            }
        }
    }
    protected override void Move(int offset)
    {
        if (!doFirstMove)
        {
            doFirstMove = true;
            MoveSelector(0);
            return;
        }
        int rowCount = Mathf.CeilToInt((float)totalSlots / columnCount);

        int row = currentIndex / columnCount;
        int col = currentIndex % columnCount;

        if (offset == -1) // trái
        {
            col--;
            if (col < 0)
            {
                isMovingOnHadItems = false;
                int equipIndex = 0;
                if(equipTotalSlots >= 7)
                    equipIndex = 6;
                else if(equipTotalSlots >= 4)
                    equipIndex = 3;
                equipCurrentIndex = equipIndex;
                MoveEquipSelector(equipIndex);
                return;
            }
        }
        else if (offset == 1) // phải
        {
            col++;
            if (col >= columnCount) 
            {
                isMovingOnHadItems = false;
                equipCurrentIndex = 0;
                MoveEquipSelector(equipCurrentIndex);
                return;
            }
        }
        else if (offset == -columnCount) // lên
        {
            row--;
            if (row < 0) row = rowCount - 1;
        }
        else if (offset == columnCount) // xuống
        {
            row++;
            if (row >= rowCount) row = 0;
        }

        int newIndex = row * columnCount + col;

        // Nếu slot vượt quá totalSlots (ví dụ slot trống cuối grid), thì vòng lại slot hợp lệ
        if (newIndex >= totalSlots) newIndex = totalSlots - 1;

        currentIndex = newIndex;
        MoveSelector(currentIndex);
    }
    private void MoveOnEquipItems(int offset)
    {
        int col = equipCurrentIndex / equipRowCount;
        int row = equipCurrentIndex % equipRowCount;

        if (offset == -1) // trái
        {
            col--;
            if (col < 0)
            {
                isMovingOnHadItems = true;
                currentIndex = columnCount - 1;
                MoveSelector(currentIndex);
                return;
            }
        }
        else if (offset == 1) // phải
        {
            col++;
            if (col >= equipColumnCount)
            {
                isMovingOnHadItems = true;
                currentIndex = 0;
                MoveSelector(currentIndex);
                return;
            }
        }
        else if (offset == -equipRowCount) // lên
        {
            row--;
            if (row < 0) row = equipRowCount - 1;
        }
        else if (offset == equipRowCount) // xuống
        {
            row++;
            if (row >= equipRowCount) row = 0;
        }

        int newIndex = col * equipRowCount + row;

        // Nếu slot vượt quá totalSlots (ví dụ slot trống cuối grid), thì vòng lại slot hợp lệ
        if (newIndex >= equipTotalSlots) newIndex = equipTotalSlots - 1;

        equipCurrentIndex = newIndex;
        MoveEquipSelector(equipCurrentIndex);
    }
    private void MoveEquipSelector(int index)
    {
        Image selectedSlot = equipSlots[index];

        currentInventory.OnMoveSlotSelector?.Invoke(selectedSlot);
    }
    private void EquipArmorial()
    {
        armorialUI.EquipArmorialUI();
    }
    private void UnEquipArmorial()
    {
        armorialUI.UnequipArmorialUI();
    }
    public void SetEquipIndexOnEquipped(int index)
    {
        isMovingOnHadItems = false;
        equipCurrentIndex = index;
    }
    public void SetHadIndexOnUnEquipped(int index)
    {
        isMovingOnHadItems = true;
        currentIndex = index;
    }
}
