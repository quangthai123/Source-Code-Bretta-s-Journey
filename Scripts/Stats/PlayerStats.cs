using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Stat damage; // player: original 10 update 40 // nang kiem tai lv: 1 3 4 5 6 7 8
    public Stat maxHealth; // player: original 100 update 250 // item nang hp tai lv: 1 3 5 6 8// item nang so binh mau tai 1-8
    public Stat maxMana; // original 100 update 200 // lv: 2 4 5 7 8 
    public Stat resistantRate;
    public float currentHealth;
    public float currentMana;
    [Header("Flask Infor")]
    [Range(1, 10)] public int flaskQuantity;
    public int fullHealFlaskQuantity; 
    public int flaskLv;
    [Range(0, 7)] public int swordLv;
    public int currency;
    private GameDatas tempGameData;
    public bool haveDied = false;
    public int maxArmorialSlot;
    public int currentArmorialSlot;
    public int maxSwordPieceSlot;
    public int currentSwordPieceSlot;
    private void Awake()
    {
        tempGameData = Resources.Load<GameDatas>("TempGameData");
        currency = tempGameData.currency;
        maxHealth = tempGameData.maxHealth;
        currentHealth = tempGameData.currentHealth;
        maxMana = tempGameData.maxMana;
        currentMana = tempGameData.currentMana;
        swordLv = tempGameData.currentSwordLv;
        haveDied = tempGameData.haveDied;
        if (haveDied && currentMana > maxMana.GetValue() / 2f)
            currentMana = maxMana.GetValue() / 2f;

        flaskLv = tempGameData.flaskLv;
        flaskQuantity = tempGameData.flaskQuantity;
        fullHealFlaskQuantity = tempGameData.fullHealFlaskQuantity;
        maxArmorialSlot = 9;
        currentArmorialSlot = tempGameData.currenArmorialSlot;
        maxSwordPieceSlot = 32;
        currentSwordPieceSlot = tempGameData.currentSwordPieceSlot;
        InitializeDamageBaseValueBySwordLv();
    }
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    UpdateSwordLv();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2) && maxHealth.GetValue() < 250)
        //{
        //    AddMaxHealth(30);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3) && maxMana.GetValue() < 200)
        //{
        //    AddMaxMana(20);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4)) // update flask quanity
        //{
        //    UpdateFlaskQuantity();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5)) // update flask lv
        //{
        //    UpdateHpPerFlask();
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    EnhanceArmorialSlot();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    EnhanceSwordPieceSlot();
        //}
    }
    public void UpdateHealth()
    {
        if (maxHealth.GetValue() >= 250)
            return;
        AddMaxHealth(30);
    }
    public void UpdateMana()
    {
        if (maxMana.GetValue() >= 200)
            return;
        AddMaxMana(20);
    }
    public void InitializeDamageBaseValueBySwordLv()
    {
        switch (swordLv)
        {
            case 0:
                damage.baseValue = 10; break;
            case 1:
                damage.baseValue = 12; break;
            case 2:
                damage.baseValue = 15; break; // 14.4
            case 3:
                damage.baseValue = 19; break; //17.28
            case 4:
                damage.baseValue = 24; break;
            case 5:
                damage.baseValue = 30; break;
            case 6:
                damage.baseValue = 37; break;
            case 7:
                damage.baseValue = 45; break;
        }
    }
    public void Resting()
    {
        currentHealth = maxHealth.GetValue();
        tempGameData.currentHealth = (int)currentHealth;
        RefillAllFlask();
    }
    public void EnhanceArmorialSlot()
    {
        if (currentArmorialSlot >= maxArmorialSlot)
            return;
        currentArmorialSlot++;
        tempGameData.currenArmorialSlot = currentArmorialSlot;
        ArmorialUI.Instance.LoadEquippedArmorialSlot();
    }
    public void EnhanceSwordPieceSlot()
    {
        if (currentSwordPieceSlot >= maxSwordPieceSlot)
            return;
        currentSwordPieceSlot += 4;
        tempGameData.currentSwordPieceSlot = currentSwordPieceSlot;
        SwordPieceUI.Instance.UpdateBigSlotUIOnEnhance();
    }
    public void GetDamageStat(int _damage)
    {
        float finalDamage = _damage;
        finalDamage -= finalDamage*resistantRate.GetValue()/100f;
        Debug.Log(finalDamage);
        currentHealth -= finalDamage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        tempGameData.currentHealth = currentHealth;
    }
    public void AddCurrency(int _toAdd)
    {
        currency += _toAdd;
        if (currency > 999999)
            currency = 999999;
        tempGameData.currency = this.currency;
    }
    //private IEnumerator StartAddCurrency(int _toAdd)
    //{
    //    while()
    //}
    public bool CanDeductCurrency(int _toDeduct)
    {
        if(currency >= _toDeduct)
        {
            currency -= _toDeduct;
            tempGameData.currency = this.currency;
            return true;
        }
        return false;
    }
    public void AddMaxHealth(float _hp)
    {
        maxHealth.AddModifier(_hp);
        tempGameData.maxHealth = maxHealth;
    }
    public void DeductMaxHealthByItem(float _hp)
    {
        maxHealth.RemoveModifier(_hp);
        tempGameData.maxHealth = maxHealth;
    }
    public void UpdateFlaskQuantity()
    {
        switch (flaskLv)
        {
            case 0:
                {
                    if (flaskQuantity == 10)
                        return;
                } break;
            case 1:
                {
                    if (flaskQuantity == 9)
                        return;
                }
                break;
            case 2:
                {
                    if (flaskQuantity == 8)
                        return;
                }
                break;
            case 3:
                {
                    if (flaskQuantity == 7)
                        return;
                }
                break;
            case 4:
                {
                    if (flaskQuantity == 6)
                        return;
                }
                break;
            case 5:
                {
                    if (flaskQuantity == 5)
                        return;
                }
                break;
        }
        bool canAddMoreFlask = false;
        int flaskIndexToRemove = -1;
        foreach(int itemIndex in Inventory.Instance.importantHadItems)
        {
            // index binh mau tu 0 - 7
            if (itemIndex >= 0 && itemIndex <= 7)
            {
                canAddMoreFlask = true;
                flaskIndexToRemove = itemIndex;
                break;
            }
        }
        if (!canAddMoreFlask)
        {
            Debug.Log("Not Enough Flask!");
            return;
        }
        Debug.Log("Added Flask!");
        flaskQuantity += 1;
        if(tempGameData.flaskQuantity != flaskQuantity)
            RefillAllFlask();
        tempGameData.flaskQuantity = flaskQuantity;
        ImportantItemUI.Instance.ReloadNewSignsOnUpgrade(flaskIndexToRemove);
        Inventory.Instance.importantHadItems.Remove(flaskIndexToRemove);
        Inventory.Instance.usedImportantItems.Add(flaskIndexToRemove);
        SaveManager.instance.tempGameData.importantHadItems = Inventory.Instance.importantHadItems;
        SaveManager.instance.tempGameData.usedImportantItems = Inventory.Instance.usedImportantItems;
        ImportantItemUI.Instance.LoadHadItemUI();
        //ImportantItemUI.Instance.RemoveAllNewSignOnEmptySlot();
        ImportantItemUI.Instance.CloseInforUIOnUpgrade();
    }
    public void UpdateHpPerFlask()
    {
        if (flaskQuantity > 1 && flaskLv < 5)
        {
            flaskQuantity--;
            flaskLv++;
            tempGameData.flaskQuantity = flaskQuantity;
            tempGameData.flaskLv = flaskLv;
            RefillAllFlask();
        }
    }
    public void Healing()
    {
        if (fullHealFlaskQuantity <= 0)
            return;
        switch (flaskLv)
        {
            case 0:
                currentHealth += 50; break; // 10 500
            case 1:
                currentHealth += 67; break; // 9 600 // 17
            case 2:
                currentHealth += 88; break; // 8 700 // 21
            case 3:
                currentHealth += 114; break; // 7 800 // 26
            case 4:
                currentHealth += 150; break; // 6 900 // 36
            case 5:
                currentHealth += 200; break; // 5 1000 // 50
        }
        if(currentHealth > maxHealth.GetValue())
            currentHealth = maxHealth.GetValue();
        fullHealFlaskQuantity--;
        tempGameData.currentHealth = (int)currentHealth;
        tempGameData.fullHealFlaskQuantity = fullHealFlaskQuantity;
    }
    public void RefillAllFlask()
    {
        fullHealFlaskQuantity = flaskQuantity;
        tempGameData.fullHealFlaskQuantity = fullHealFlaskQuantity;
    }
    public void Refill2FlaskByRevive()
    {
        if (flaskQuantity < 2)
            fullHealFlaskQuantity = 1;
        else if (fullHealFlaskQuantity < 2 && flaskQuantity >= 2)
            fullHealFlaskQuantity = 2;
        tempGameData.fullHealFlaskQuantity = fullHealFlaskQuantity;
    }
    public void UpdateSwordLv()
    {
        if (swordLv < 7)
        {
            swordLv++;
            switch(swordLv)
            {
                case 0:
                    damage.baseValue = 10; break;
                case 1:
                    damage.baseValue = 12; break;
                case 2:
                    damage.baseValue = 15; break; // 14.4
                case 3:
                    damage.baseValue = 19; break; //17.28
                case 4:
                    damage.baseValue = 24; break;
                case 5:
                    damage.baseValue = 30; break;
                case 6:
                    damage.baseValue = 37; break;
                case 7:
                    damage.baseValue = 45; break;
            }
            tempGameData.currentSwordLv = swordLv;
        }
    }
    public void AddMaxMana(float _mana)
    {
        maxMana.AddModifier(_mana);
        tempGameData.maxMana = maxMana;
    }
    public void DeductMaxManaByItem(float _mana)
    {
        maxMana.RemoveModifier(_mana);
        tempGameData.maxMana = maxMana;
    }
    public void GetManaByAttack()
    {
        currentMana += 2;
        tempGameData.currentMana = (int)currentMana;
        if(!haveDied && currentMana > maxMana.GetValue())
        {
            currentMana = maxMana.GetValue();
        } else if(haveDied && currentMana > maxMana.GetValue()/2f)
        {
            currentMana = maxMana.GetValue() / 2f;
        }
    }
    public void DeductManaByMagicSkill(int _manaUsed)
    {
        currentMana -= _manaUsed;
        tempGameData.currentMana = (int)currentMana;
    }
}
