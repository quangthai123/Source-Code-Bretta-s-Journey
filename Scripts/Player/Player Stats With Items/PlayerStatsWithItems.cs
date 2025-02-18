using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsWithItems : MonoBehaviour
{
    private PlayerStats playerStats;
    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }
    private void Start()
    {
        if(SaveManager.instance.tempGameData.amorialEquippedItems != null)
        {
            foreach(int index in SaveManager.instance.tempGameData.amorialEquippedItems)
            {
                EquipArmorial(index);
            }
        }
        if (SaveManager.instance.tempGameData.perfectSwordsEquipped != null)
        {
            foreach (int index in SaveManager.instance.tempGameData.perfectSwordsEquipped)
            {
                EquipPerfectSword(index);
            }
        }
        if (SaveManager.instance.tempGameData.swordPairsActivated != null)
        {
            foreach (int index in SaveManager.instance.tempGameData.swordPairsActivated)
            {
                ActivateSwordPair(index);
            }
        }
    }
    public void EquipArmorial(int armorialIndex)
    {
        switch(armorialIndex)
        {
            case 0: // khang 10% vat ly
                playerStats.resistantRate.AddModifier(10f); break;
            case 2: // Tang thoi gian su dung khien: 0.2f => 0.5f 
                Player.Instance.shieldDuration = 0.6f; break;
            case 3: // Giam thoi gian cho cua ky nang luot: 0.6f => 0.3f
                Player.Instance.dashCooldown = 0.3f; break;
            case 4: // Tang khoang cach cua ky nang luot: 0.3f => 0.45f
                Player.Instance.dashDuration = 0.45f; break;
            case 5: // Tang toc do di chuyen: 9f => 11f
                Player.Instance.moveSpeed = 11f; break;
            case 6: // Giam thoi gian su dung binh mau: 1.8f => 0.9f
                Player.Instance.healingDuration = 1f; break;
            // 7 la Mien moi sat thuong trong khi su dung binh mau
            case 8: // Khang 15% vat ly
                playerStats.resistantRate.AddModifier(15f); break;
            case 9: // Khang 25% vat ly
                playerStats.resistantRate.AddModifier(25f); break;

        }
    }
    public void UnequipArmorial(int armorialIndex)
    {
        switch(armorialIndex)
        {
            case 0:
                playerStats.resistantRate.RemoveModifier(10f); break;
            case 2: // Giam lai thoi gian su dung khien: 0.5f => 0.2f 
                Player.Instance.shieldDuration = 0.4f; break;
            case 3: // Tang lai thoi gian cho cua ky nang luot: 0.3f => 0.8f
                Player.Instance.dashCooldown = 0.6f; break;
            case 4: // Giam lai khoang cach cua ky nang luot: 0.45f => 0.32f
                Player.Instance.dashDuration = 0.32f; break;
            case 5: // Giam lai toc do di chuyen: 11f => 9f
                Player.Instance.moveSpeed = 9f; break;
            case 6: // Tang lai thoi gian su dung binh mau: 0.9f => 1.8f
                Player.Instance.healingDuration = 1.8f; break;
            // 7 la Mien moi sat thuong trong khi su dung binh mau
            case 8: // Khang 15% vat ly
                playerStats.resistantRate.RemoveModifier(15f); break;
            case 9: // Khang 25% vat ly
                playerStats.resistantRate.RemoveModifier(25f); break;
        }
    }
    public void EquipPerfectSword(int perfectSwordIndex)
    {
        switch (perfectSwordIndex)
        {
            case 0:
                playerStats.damage.AddRateModifier(10);
                break;

        }
    }
    public void UnEquipPerfectSword(int index)
    {
        switch (index)
        {
            case 0:
                playerStats.damage.RemoveRateModifier(10);
                break;

        }
    }
    public void ActivateSwordPair(int pairIndex)
    {
        switch (pairIndex)
        {
            case 0:
                playerStats.damage.AddRateModifier(5);
                break;

        }
    }
    public void DeactivateSwordPair(int index)
    {
        switch (index)
        {
            case 0:
                playerStats.damage.RemoveRateModifier(5);
                break;

        }
    }
    public bool CheckEquippedArmorial(int index)
    {
        if (SaveManager.instance.tempGameData.amorialEquippedItems == null ||
            (SaveManager.instance.tempGameData.amorialEquippedItems != null && SaveManager.instance.tempGameData.amorialEquippedItems.Count < 1))
            return false;
        foreach (int i in SaveManager.instance.tempGameData.amorialEquippedItems)
        {
            if(index == i)
                return true;
        }
        return false;
    }
    public bool CheckEquippedPerfectSword(int index)
    {
        foreach (int i in SaveManager.instance.tempGameData.perfectSwordsEquipped)
        {
            if (index == i)
                return true;
        }
        return false;
    }
    public bool CheckActivatedSwordPair(int index)
    {
        foreach (int i in SaveManager.instance.tempGameData.swordPairsActivated)
        {
            if (index == i)
                return true;
        }
        return false;
    }
}
