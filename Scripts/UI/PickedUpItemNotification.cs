using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickedUpItemNotification : MonoBehaviour
{
    public static PickedUpItemNotification Instance;
    [SerializeField] private List<Image> bgImage;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private Image itemBorderImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void ShowPickedUpItemNotification(Sprite _itemImage, string _itemName)
    {
        itemImage.sprite = _itemImage;
        itemName.text = _itemName;
        itemImage.useSpriteMesh = false;
        itemImage.rectTransform.pivot = new Vector2(.5f, .5f);
        itemImage.rectTransform.localScale = Vector2.one;
        itemImage.rectTransform.rotation = Quaternion.identity;
        //StopAllCoroutines();
        StartCoroutine(ActiveUIByTime());
    }
    public void ShowPickedUpSwordPieceNotification(Sprite _itemImage, Vector2 pivot, Quaternion rotation, Vector2 scale, string _itemName)
    {
        itemImage.sprite = _itemImage;
        itemImage.useSpriteMesh = true;
        itemImage.rectTransform.pivot = pivot;
        itemImage.rectTransform.rotation = rotation;
        itemImage.rectTransform.localScale = scale * 1.65f;
        itemName.text = _itemName;
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
        itemBorderImage.color = new Color(itemBorderImage.color.r, itemBorderImage.color.g, itemBorderImage.color.b,
            itemBorderImage.color.a + .1f);
        itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b,
            itemImage.color.a + .1f);
        itemName.color = new Color(itemName.color.r, itemName.color.g, itemName.color.b,
            itemName.color.a + .1f);
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
        itemBorderImage.color = new Color(itemBorderImage.color.r, itemBorderImage.color.g, itemBorderImage.color.b,
            itemBorderImage.color.a - .1f);
        itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b,
            itemImage.color.a - .1f);
        itemName.color = new Color(itemName.color.r, itemName.color.g, itemName.color.b,
            itemName.color.a - .1f);
        if (notificationText.color.a > 0f)
            StartCoroutine(DeactiveUIByTime());
    }
}
