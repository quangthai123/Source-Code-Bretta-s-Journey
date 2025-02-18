using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFx : MonoBehaviour
{
    public Material originalMat { get; private set; }
    private SpriteRenderer sr;
    [SerializeField] private Material flashFxMat;
    public Material redFxForBossMat;
    [SerializeField] private float flashFxDuration;
    private Color originalColor;
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
        originalColor = sr.color;
    }
    public IEnumerator FlashFX()
    {
        sr.material = flashFxMat;
        yield return new WaitForSeconds(flashFxDuration);
        sr.material = originalMat;
    }
    public IEnumerator QuickFlashFX()
    {
        sr.material = flashFxMat;
        yield return new WaitForSeconds(flashFxDuration /2f);
        sr.material = originalMat;
    }
    public IEnumerator RedFXForBoss()
    {
        sr.material = redFxForBossMat;
        yield return new WaitForSeconds(.75f);
        //if(gameObject.activeInHierarchy)
        sr.material = originalMat;
    }
    public IEnumerator BeCounterAttackedFlashFx()
    {
        sr.color = new Color(255f / 255f, 0f, 0f);
        yield return new WaitForSeconds(flashFxDuration);
        sr.color = originalColor;
    } 
}
