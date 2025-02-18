using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeToGrab : MonoBehaviour
{
    public Vector2 offSetForGrabPos;
    private void Reset()
    {
        offSetForGrabPos = new Vector2(.799671f, .75f);
    }
}
