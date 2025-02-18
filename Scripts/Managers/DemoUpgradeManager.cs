using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoUpgradeManager : MonoBehaviour
{
    private GameObject updateUI;
    private Transform arrow;
    private void Awake()
    {
        updateUI = transform.Find("UpdateUI").gameObject;
        arrow = transform.Find("ArrowUpdate");
    }
    public void OnClickOpenUpdateListUI()
    {
        if(!updateUI.gameObject.activeSelf)
            updateUI.gameObject.SetActive(true);
        else
            updateUI.gameObject.SetActive(false);
        arrow.Rotate(0, 0, 180f);
    }
    public void OnClickUpdateSwordLv() => Player.Instance.playerStats.UpdateSwordLv();
    public void OnClickUpdateHealth()
    {
        Player.Instance.playerStats.UpdateHealth();
    }
    public void OnClickUpdateMana()
    {
        Player.Instance.playerStats.UpdateMana();
    }
    public void OnClickUpdateFlaskQuantity()
    {
        Player.Instance.playerStats.UpdateFlaskQuantity();
    }
    public void OnClickUpdateHpPerFlask()
    {
        Player.Instance.playerStats.UpdateHpPerFlask();
    }
    public void OnClickEnhanceArmorialSlot()
    {
        Player.Instance.playerStats.EnhanceArmorialSlot();
    }
    public void OnClickEnhanceSwordPieceSlot()
    {
        Player.Instance.playerStats.EnhanceSwordPieceSlot();
    }
}
