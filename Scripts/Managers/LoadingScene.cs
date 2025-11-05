using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    public static LoadingScene instance;
    private CanvasGroup canvasGroup;
    [SerializeField] private GameObject logoImage;
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        DontDestroyOnLoad(gameObject);
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
    private IEnumerator FadeIn(float fadeInTime, bool showLogo, Action callBack = null)
    {
        if (showLogo)
            logoImage.SetActive(true);
        else
            logoImage.SetActive(false);
        canvasGroup.alpha = 0f;
        while (canvasGroup.alpha < 1f)
        {
            yield return new WaitForSecondsRealtime(.01f);
            canvasGroup.alpha += 1/fadeInTime/ 100f;
        }
        canvasGroup.alpha = 1f;
        callBack?.Invoke();
    }
    private IEnumerator FadeOut(float fadeOutTime, Action callBack = null)
    {
        canvasGroup.alpha = 1f;
        while (canvasGroup.alpha > 0f)
        {
            yield return new WaitForSecondsRealtime(.01f);
            canvasGroup.alpha -= 1 / fadeOutTime / 100f;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        callBack?.Invoke();
    }
    public void StartFadeIn(float fadeInTime, bool showLogo, Action callBack = null)
    {
        canvasGroup.blocksRaycasts = true;
        StopAllCoroutines();
        StartCoroutine(FadeIn(fadeInTime, showLogo, callBack));
    }
    public void StartFadeOut(float fadeOutTime, Action callBack = null)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(fadeOutTime, callBack));
    }
}
