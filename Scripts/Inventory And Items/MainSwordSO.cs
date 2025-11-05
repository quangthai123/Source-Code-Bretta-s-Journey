using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainSwordData", menuName = "ScriptableObjects/MainSwordData")]
public class MainSwordSO : ScriptableObject
{
    public string SwordName;
    public string Description;
    [TextArea] public string Lore;
}
