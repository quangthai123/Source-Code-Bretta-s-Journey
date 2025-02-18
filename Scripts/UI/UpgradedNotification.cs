using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradedNotification : MonoBehaviour
{
    [SerializeField] private List<Image> bgImage;
    [SerializeField] private TextMeshProUGUI notificationText;
    public static UpgradedNotification Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void ShowUpgradeFlaskQuantityNotification()
    {
        GameManager.Instance.HideAllInGameUI();
        StartCoroutine(ActiveUIByTime());
    }
    private IEnumerator ActiveUIByTime()
    {
        yield return new WaitForSeconds(.05f);
        foreach (Image image in bgImage)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + .1f);
        }
        notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b,
            notificationText.color.a + .1f);
        if (notificationText.color.a < 1f)
            StartCoroutine(ActiveUIByTime());
        else
            Invoke("ShowNotificationDuration", 2.5f);
    }
    private void ShowNotificationDuration()
    {
        StartCoroutine(DeactiveUIByTime());
    }
    private IEnumerator DeactiveUIByTime()
    {
        yield return new WaitForSeconds(.05f);
        foreach (Image image in bgImage)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - .1f);
        }
        notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b,
            notificationText.color.a - .1f);
        if (notificationText.color.a > 0f)
            StartCoroutine(DeactiveUIByTime());
    }
}
