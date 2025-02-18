using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLvSceneManager : MonoBehaviour
{
    public GameObject wayToLv2PortalContrainter;
    void Start()
    {
        if (SaveManager.instance.tempGameData.finishLv1)
            wayToLv2PortalContrainter.SetActive(false);
        else
            wayToLv2PortalContrainter.SetActive(true);
    }
}
