using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/Dialogue Data")]
public class DialogueTextDataSO : ScriptableObject
{
    [TextArea] public List<string> sentences;
}
