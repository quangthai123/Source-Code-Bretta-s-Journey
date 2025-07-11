using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIController : MonoBehaviour
{
    [SerializeField] private Transform swordSkillUI;
    [SerializeField] private Transform magicSkillUI;
    [SerializeField] private Transform swordSkillUITab;
    [SerializeField] private Transform magicSkillUITab;
    [SerializeField] private SwordSkillPreview swordSkillPreview;
    public void OnClickSwordSkillUITab()
    {
        if (swordSkillUITab.localScale == new Vector3(1.25f, 1.25f, 1f))
            return;
        swordSkillUI.gameObject.SetActive(true);
        magicSkillUI.gameObject.SetActive(false);

        swordSkillUITab.localScale = new Vector3(1.25f, 1.25f, 1f);
        swordSkillUITab.Find("CoverOnSelect").gameObject.SetActive(true);
        swordSkillUITab.Find("SelectImage").gameObject.SetActive(true);

        magicSkillUITab.localScale = new Vector3(1f, 1f, 1f);
        magicSkillUITab.Find("CoverOnSelect").gameObject.SetActive(false);
        magicSkillUITab.Find("SelectImage").gameObject.SetActive(false);
    }
    public void OnClickMagicSkillUITab()
    {
        if (magicSkillUITab.localScale == new Vector3(1.25f, 1.25f, 1f))
            return;

        magicSkillUI.gameObject.SetActive(true);
        swordSkillUI.gameObject.SetActive(false);

        magicSkillUITab.localScale = new Vector3(1.25f, 1.25f, 1f);
        magicSkillUITab.Find("CoverOnSelect").gameObject.SetActive(true);
        magicSkillUITab.Find("SelectImage").gameObject.SetActive(true);

        swordSkillUITab.localScale = new Vector3(1f, 1f, 1f);
        swordSkillUITab.Find("CoverOnSelect").gameObject.SetActive(false);
        swordSkillUITab.Find("SelectImage").gameObject.SetActive(false);

        //swordSkillPreview.StopPreview();
    }
}
