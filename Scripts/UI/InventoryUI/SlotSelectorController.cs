using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class SlotSelectorController : MonoBehaviour
{
    [SerializeField] protected Image[] slots;
    [SerializeField] protected InventoryLogic currentInventory;

    public int columnCount = 4;      // Số cột trong grid
    protected int totalSlots = 20;      // Tổng số slot
    protected int currentIndex = 0;    // Slot đang chọn
    protected bool doFirstMove = false;
    protected virtual void Start()
    {
        slots = currentInventory.itemHadImage.ToArray();
        totalSlots = slots.Length;
        currentInventory.OnPickUpNewItem += () => { doFirstMove = false; currentIndex = 0; };
    }

    protected virtual void Update()
    {
        if(InventoryUI.Instance.FrezeeInventoryAction)
            return;
        if (Input.GetKeyDown(KeyCode.A))
            Move(-1);
        else if (Input.GetKeyDown(KeyCode.D))
            Move(1);
        else if (Input.GetKeyDown(KeyCode.W))
            Move(-columnCount);
        else if (Input.GetKeyDown(KeyCode.S))
            Move(columnCount);
    }
    protected virtual void Move(int offset)
    {
        if(!doFirstMove)
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
            if (col < 0) col = columnCount - 1;
        }
        else if (offset == 1) // phải
        {
            col++;
            if (col >= columnCount) col = 0;
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
    protected void MoveSelector(int index)
    {
        Image selectedSlot = slots[index];

        currentInventory.OnMoveSlotSelector?.Invoke(selectedSlot);
    }
}
