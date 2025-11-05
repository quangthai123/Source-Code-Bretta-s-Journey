using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationCanvas : PersistentObject
{
    public static NotificationCanvas Instance { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        if(Instance != null)
            Destroy(gameObject);
        else
            Instance = this;    
    }
}
