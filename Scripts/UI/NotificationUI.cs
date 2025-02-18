using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationUI : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    [SerializeField] protected float showNotiDuration;
    void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        StartCoroutine(ShowNoti());
    }
    protected IEnumerator ShowNoti()
    {
        InventoryUI.Instance.SetInventoryUIUninteractable();
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
        InventoryUI.Instance.SetInventoryUIInteractable();
        gameObject.SetActive(false);
    }
    public void OnCloseNoti()
    {
        StartCoroutine(CloseNoti());
    }
}
