using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffect : MonoBehaviour
{
    [SerializeField] protected float showNotiDuration;
    protected CanvasGroup canvasGroup;
    protected void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        StopCoroutine(ShowLayer());
        StartCoroutine(ShowLayer());
        PlayScreenUI.instance.HideControlUI();
    }
    protected IEnumerator ShowLayer()
    {
        while (canvasGroup.alpha < 1f)
        {
            yield return new WaitForSecondsRealtime(showNotiDuration / 10f);
            canvasGroup.alpha += .1f;
        }
    }
    protected IEnumerator CloseLayer()
    {
        while (canvasGroup.alpha > 0f)
        {
            yield return new WaitForSecondsRealtime(showNotiDuration / 10f);
            canvasGroup.alpha -= .1f;
        }
        PlayScreenUI.instance.ShowControlUI();
        gameObject.SetActive(false);
    }
    public void OnCloseLayer()
    {
        StartCoroutine(CloseLayer());
    }
}
