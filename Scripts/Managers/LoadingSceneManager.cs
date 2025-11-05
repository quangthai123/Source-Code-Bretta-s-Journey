using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Load Finshed!");
        LoadingScene.instance.StartFadeOut(1/6f);
    }
}
