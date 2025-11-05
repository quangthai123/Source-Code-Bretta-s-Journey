using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityGiver : NPC
{
    [SerializeField] private int abilityIndex;
    private Canvas canvas;
    private GameDatas tempGameData;
    private Transform tutorialImage;
    private void Awake()
    {
        tempGameData = Resources.Load<GameDatas>("TempGameData");
        if (tempGameData.GainedAbilities[abilityIndex])
            GetComponent<BoxCollider2D>().enabled = false;
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        tutorialImage = canvas.transform.GetChild(abilityIndex + 1);
        DisableAllUIOnThis();
        AddListenerForTutorialBackBtn();
    }
    private void DisableAllUIOnThis()
    {
        foreach(Transform child in canvas.transform)
        {
            child.gameObject.SetActive(false);
        }
        canvas.gameObject.SetActive(false);
    }
    protected override void OnInteract()
    {
        base.OnInteract();
        if (tempGameData.GainedAbilities[abilityIndex])
            return;
        tempGameData.GainedAbilities[abilityIndex] = true;
        GetComponent<BoxCollider2D>().enabled = false;
        PlayScreenUI.instance.HideControlUI();
        canvas.gameObject.SetActive(true);
        Transform gainedNoti = canvas.transform.GetChild(0);
        gainedNoti.Find("NotiText").GetComponent<TextMeshProUGUI>().text = GetAbilityNameByGiverIndex();
        gainedNoti.gameObject.SetActive(true);
        StartCoroutine(ShowUIEffect(gainedNoti, DisableGainedNoti));
    }
    private IEnumerator ShowUIEffect(Transform ui, Action callBack = null)
    {
        while(ui.GetComponent<CanvasGroup>().alpha < 1)
        {
            yield return new WaitForSeconds(.025f);
            ui.GetComponent<CanvasGroup>().alpha += .1f;
        }
        callBack?.Invoke();
    }
    private IEnumerator DisableUIEffect(Transform ui, Action callBack = null)
    {
        while (ui.GetComponent<CanvasGroup>().alpha > 0)
        {
            yield return new WaitForSeconds(.025f);
            ui.GetComponent<CanvasGroup>().alpha -= .1f;
        }
        callBack?.Invoke();
    }
    private void DisableGainedNoti()
    {
        Invoke("StartDisableGainedNoti", 4f);
    }
    private void StartDisableGainedNoti() => StartCoroutine(DisableUIEffect(canvas.transform.GetChild(0), StartShowTutirial));
    private void StartShowTutirial()
    {
        tutorialImage.gameObject.SetActive(true);
        StartCoroutine(ShowUIEffect(tutorialImage));
    }
    private void AddListenerForTutorialBackBtn()
    {
        tutorialImage.Find("Back").GetComponent<Button>().onClick.AddListener(() => 
        StartCoroutine(DisableUIEffect(tutorialImage, ShowControlUIAndDisableCanvas)));
    }
    private void ShowControlUIAndDisableCanvas()
    {
        PlayScreenUI.instance.ShowControlUI();
        canvas.gameObject.SetActive(false);
    }
    private string GetAbilityNameByGiverIndex()
    {
        string name = "";
        switch (abilityIndex) 
        {
            case 0:
                name = "Passage of Ash";
                break;
            case 1:
                name = "Dashing of God";
                break;
            default:
                Debug.Log("Wrong Giver Index!");
                break;
        }
        return name;
    }
}
