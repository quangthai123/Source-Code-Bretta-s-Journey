using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword1", menuName = "ScriptableObjects/Perfect Sword")]
public class PerfectSwordSO : ScriptableObject
{
    public int index;
    public string swordName;
    public Sprite image;
    [Header("Lore And Function")]
    [TextArea] public string description;
    [TextArea] public string lore;
    public string originalFunction;
    public string effectPairFunction;
    public string effectPairFunctionForNoti;
}
