using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewHandler : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Scrollbar scrollbar;
    private static ScrollViewHandler currentScrollView;
    void OnEnable()
    {
        StartCoroutine(ResetScrollView());
    }
    public IEnumerator ResetScrollView()
    {
        yield return null;
        scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, 1);
        currentScrollView = this;
    }
    private void Update()
    {
        if(currentScrollView != null && currentScrollView != this) return;
        if (Input.GetKey(KeyCode.Y))
        {
            if (scrollbar.value >= 1f) return;
            scrollbar.value += 1f * Time.unscaledDeltaTime;
        }
        if (Input.GetKey(KeyCode.H))
        {
            if (scrollbar.value <= 0f) return;
            scrollbar.value -= 1f * Time.unscaledDeltaTime;
        }
    }
    private void OnDisable()
    {
        currentScrollView = null;
    }
}
