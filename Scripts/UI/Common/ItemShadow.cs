using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ItemShadow : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float losingAlphaColorCooldown = .03f;
    public void SetSprite(RectTransform item, Sprite sprite)
    {
        image.color = new Color(210f / 255f, 140f / 255f, 240f / 255f, 1f);
        image.sprite = sprite;
        RectTransform imageRect = image.GetComponent<RectTransform>();
        imageRect.sizeDelta = item.sizeDelta;
        imageRect.localScale = item.localScale;  
        StartCoroutine("LosingAlphaColor");
    }
    private void Update()
    {
        if (image.color.a <= 0)
        {
            ItemShadowEffectSpawner.Instance.Despawn(transform);
        }
    }
    private IEnumerator LosingAlphaColor()
    {
        while (image.color.a > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.1f);
            yield return new WaitForSecondsRealtime(losingAlphaColorCooldown);
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        ItemShadowEffectSpawner.Instance.Despawn(transform);
    }
}
