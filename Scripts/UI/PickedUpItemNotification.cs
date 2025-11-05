using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickedUpItemNotification : MonoBehaviour
{
    public static PickedUpItemNotification Instance;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private float showTime = 3f;
    private FadeEffectHandler fadeEffectHandler;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        fadeEffectHandler = GetComponent<FadeEffectHandler>();
        gameObject.SetActive(false);
    }
    public void ShowPickedUpItemNotification(Sprite _itemImage, string _itemName)
    {
        itemImage.sprite = _itemImage;
        itemName.text = _itemName;
        itemImage.useSpriteMesh = false;
        itemImage.rectTransform.pivot = new Vector2(.5f, .5f);
        itemImage.rectTransform.localScale = Vector2.one;
        itemImage.rectTransform.rotation = Quaternion.identity;
        fadeEffectHandler.StartFadeIn(() => Invoke(nameof(StartFadeOut), showTime));
    }
    public void ShowPickedUpSwordPieceNotification(Sprite _itemImage, Vector2 pivot, Quaternion rotation, Vector2 scale, string _itemName)
    {
        itemImage.sprite = _itemImage;
        itemImage.useSpriteMesh = true;
        itemImage.rectTransform.pivot = pivot;
        itemImage.rectTransform.rotation = rotation;
        itemImage.rectTransform.localScale = scale * 1.65f;
        itemName.text = _itemName;
        fadeEffectHandler.StartFadeIn(() => Invoke(nameof(StartFadeOut), showTime));
    }
    private void StartFadeOut()
    {
        fadeEffectHandler.StartFadeOut(() => gameObject.SetActive(false));
    }
}
