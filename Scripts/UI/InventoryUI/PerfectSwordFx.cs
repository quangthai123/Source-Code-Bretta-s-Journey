using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectSwordFx : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private bool isBlurSwordFx;
    void OnEnable()
    {
        anim = GetComponent<Animator>();
    }
    private void DesstroyOnFinishAnim()
    {
        if (isBlurSwordFx)
            SwordPieceUI.Instance.OnDestroyBlurSwordFx();
        Destroy(gameObject);
    }
}
