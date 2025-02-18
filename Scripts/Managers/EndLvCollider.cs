using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLvCollider : MonoBehaviour
{
    //[SerializeField] private int endLv = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            SaveManager.instance.tempGameData.finishLv1 = true;
    }
}
