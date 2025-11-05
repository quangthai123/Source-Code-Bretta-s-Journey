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
        targetPosX = transform.position.x;
    }
    protected override void OnInteract()
    {
        base.OnInteract();
        swordUIcanvas.gameObject.SetActive(true);
    }
}
