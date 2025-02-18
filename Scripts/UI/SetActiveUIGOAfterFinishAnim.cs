using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveUIGOAfterFinishAnim : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void DeactiveGOAfterFinishAnim()
    {
        gameObject.SetActive(false);
        if(SceneScenarioSelectLv.instance != null && !SceneScenarioSelectLv.instance.isScenario && gameObject.name == "PlaceUI")
        {
            SceneScenarioSelectLv.instance.FinishScenarioSelectLv();
            CheckPoint.instance.SaveGameByCheckPoint();
        }
    }
}
