using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    private void SetActiveFalseAfterFinishFadeOut()
    {
        gameObject.SetActive(false);    
    }
}
