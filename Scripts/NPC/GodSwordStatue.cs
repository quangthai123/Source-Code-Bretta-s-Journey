using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodSwordStatue : NPC
{
    private Canvas swordUIcanvas;
    private void Awake()
    {
        swordUIcanvas = transform.GetChild(0).GetComponent<Canvas>();
        swordUIcanvas.gameObject.SetActive(false);
    }
    protected override void OnInteract()
    {
        swordUIcanvas.gameObject.SetActive(true);
    }
}
