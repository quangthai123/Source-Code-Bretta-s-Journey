using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class FadeEffectHandler : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float FadeInTime = 1/6f;
    public float FadeOutTime = 1/6f;
    private bool IsRunningCoroutine = false;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private IEnumerator FadeIn(Action callBack = null)
    {
        IsRunningCoroutine = true;
        canvasGroup.alpha = 0f;
        while (canvasGroup.alpha < 1f)
        {
            yield return new WaitForSecondsRealtime(.01f);
            canvasGroup.alpha += 1 / FadeInTime / 100f;
        }
        IsRunningCoroutine = false;
        canvasGroup.alpha = 1f;
        callBack?.Invoke();
    }
    private IEnumerator FadeOut(Action callBack = null)
    {
        IsRunningCoroutine = true;
        canvasGroup.alpha = 1f;
        while (canvasGroup.alpha > 0f)
        {
            yield return new WaitForSecondsRealtime(.01f);
            canvasGroup.alpha -= 1 / FadeOutTime / 100f;
        }
        IsRunningCoroutine = false;
        canvasGroup.alpha = 0f;
        callBack?.Invoke();
    }
    public void StartFadeIn(Action callBack = null)
    {
        if(IsRunningCoroutine) return;
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeIn(callBack));
    }
    public void StartFadeOut(Action callBack = null)
    {
        if (IsRunningCoroutine) return;
        StopAllCoroutines();
        StartCoroutine(FadeOut(callBack));
    }
}
