using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneIndexManager : MonoBehaviour
{
    public static SceneIndexManager Instance;
    public int sceneIndex;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
}
