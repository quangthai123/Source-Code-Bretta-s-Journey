using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationUI : MonoBehaviour
{
    public static NotificationUI Instance { get; private set; }
    [SerializeField] protected float showNotiDuration;
    [SerializeField] protected TextMeshProUGUI notificationTxt;
    [SerializeField] protected Button escapeBtn;
    protected CanvasGroup canvasGroup;
    protected Action AfterCloseNotificationCallBack;
    void Awake()
    {
        if(Instance != null)
            Destroy(gameObject); 
        else
            Instance = this;
        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        escapeBtn.onClick.RemoveAllListeners();
        escapeBtn.onClick.AddListener(OnCloseNoti);
        StopCoroutine(ShowNoti());
        StartCoroutine(ShowNoti());
        PlayScreenUI.instance.HideControlUI();
    }
    public void SetMessageAndNotify(string msg, Action callBack = null)
    {
        notificationTxt.text = msg;
        gameObject.SetActive(true);
        AfterCloseNotificationCallBack = callBack;
    }
    protected IEnumerator ShowNoti()
    {
        while (canvasGroup.alpha < 1f)
        {
            yield return new WaitForSecondsRealtime(showNotiDuration / 10f);
            canvasGroup.alpha += .1f;
        }
    }
    protected virtual IEnumerator CloseNoti()
    {
        while (canvasGroup.alpha > 0f)
        {
            yield return new WaitForSecondsRealtime(showNotiDuration / 10f);
            canvasGroup.alpha -= .1f;
        }
        PlayScreenUI.instance.ShowControlUI();
        AfterCloseNotificationCallBack?.Invoke();
        gameObject.SetActive(false);
    }
    public void OnCloseNoti()
    {
        StartCoroutine(CloseNoti());
    }
}
