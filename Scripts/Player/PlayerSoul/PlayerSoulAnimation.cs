using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoulAnimation : MonoBehaviour
{
    private PlayerSoulController playerSoul;
    private void Start()
    {
        playerSoul = GetComponentInParent<PlayerSoulController>();
    }
    private void ConvertToChaseAnim()
    {
        playerSoul.ConvertToChaseAnim();
    }
    private void AttackTrigger()
    {
        playerSoul.DoDamagePlayer(0);
    }
}
