using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyValuePair
{
    public Transform key;
    public bool value;
    public KeyValuePair() { }
    public KeyValuePair(Transform _key, bool _value)
    {
        this.key = _key;
        this.value = _value;
    }
}
