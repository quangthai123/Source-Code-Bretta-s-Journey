using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAwake : MonoBehaviour
{
    public TestAwake Instance;
    public delegate void Testing();
    public event Testing OnTesting;
    public Action OnAction;
    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        Debug.Log("Awake: " + gameObject.name);
    }
    void OnEnable()
    {
        Debug.Log("OnEnable: " + gameObject.name);
    }
}
