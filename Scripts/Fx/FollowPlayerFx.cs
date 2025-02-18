using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerFx : MonoBehaviour
{
    void LateUpdate()
    {
        if((gameObject.name == "souldDeadFx" || gameObject.name == "souldDeadFx(Clone)") && PlayerSoulController.instance != null)
        {
            if (Vector2.Distance(transform.position, Player.Instance.transform.position) > .1f)
            {
                Debug.Log("spawn in soul");
                return;
            }
        } 
        if ((Player.Instance.stateMachine.currentState == Player.Instance.hurtState) || (Player.Instance.stateMachine.currentState == Player.Instance.strongHurtState)
            || (Player.Instance.stateMachine.currentState == Player.Instance.knockoutState))
            transform.position = Player.Instance.hitEffectPos.position;
        else
            transform.position = Player.Instance.transform.position;
    }
}