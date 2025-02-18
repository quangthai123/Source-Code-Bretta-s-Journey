using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBar : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;
    private void Awake()
    {
        scrollbar = GetComponent<Scrollbar>();
    }
    private void Start()
    {
        scrollbar.value = 1;
    }
    void OnEnable()
    {
        scrollbar.value = 1;
        Debug.Log("Set Scroll Bar");
    }
}
