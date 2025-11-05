using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
