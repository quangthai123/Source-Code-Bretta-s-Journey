using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    public static LoadingScene instance;
    private Animator anim; 
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        DontDestroyOnLoad(gameObject);
        anim = GetComponent<Animator>();
    }
    public void FadeIn()
    {
        anim.SetBool("FadeOut", false);
        anim.SetBool("FadeIn", true);
    }
    public void FadeOut()
    {
        anim.SetBool("FadeIn", false);
        anim.SetBool("FadeOut", true);
    }
    private void OnFinishFadeOut()
    {
        gameObject.SetActive(false);
    }
    public void StartFadeIn()
    {
        gameObject.SetActive(true);
        FadeIn();
    }
}
