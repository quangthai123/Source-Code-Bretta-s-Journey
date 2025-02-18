using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFakeSoul : MonoBehaviour
{
    public static PlayerFakeSoul instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
}
