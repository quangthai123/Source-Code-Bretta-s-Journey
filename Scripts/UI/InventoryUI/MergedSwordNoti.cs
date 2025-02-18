using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MergedSwordNoti : NotificationUI
{
    [SerializeField] private Image perfectSwordImage;
    [SerializeField] private TextMeshProUGUI perfectSwordName;
    protected override IEnumerator CloseNoti()
    {
        while (canvasGroup.alpha > 0f)
        {
            yield return new WaitForSecondsRealtime(showNotiDuration / 10f);
            canvasGroup.alpha -= .1f;
        }
        SwordPieceUI.Instance.OnCloseMergedSwordNoti();
        gameObject.SetActive(false);
    }
    public void GetPerfectSwordInfor(Sprite sprite, string name)
    {
        perfectSwordImage.sprite = sprite;
        perfectSwordName.text = name;
        Debug.Log("Assign for noti!");
    }
}
