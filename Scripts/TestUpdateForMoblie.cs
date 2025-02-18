using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUpdateForMoblie : MonoBehaviour
{
    public void UpdateSword()
    {
        Player.Instance.playerStats.UpdateSwordLv();
    }
    public void UpdateHealth()
    {
        if(Player.Instance.playerStats.maxHealth.GetValue() < 250)
            Player.Instance.playerStats.AddMaxHealth(30);
    }
    public void UpdateMana()
    {
        if (Player.Instance.playerStats.maxMana.GetValue() < 200)
            Player.Instance.playerStats.AddMaxMana(20);
    }
    public void UpdateFlaskQuantity()
    {
        Player.Instance.playerStats.UpdateFlaskQuantity();
    }
    public void UpdateHpPerFlask()
    {
        Player.Instance.playerStats.UpdateHpPerFlask();
    }
}
