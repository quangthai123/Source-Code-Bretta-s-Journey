using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHalo : MonoBehaviour
{
    private SpriteRenderer sr;
    private Player player;
    private SpriteRenderer playerSr;
    void Start()
    {
        player = Player.Instance;
        sr = GetComponent<SpriteRenderer>();
        playerSr = player.transform.Find("Model").GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.stateMachine.currentState == player.magicState)
        {
            sr.enabled = true;
            sr.sprite = playerSr.sprite;
        } else 
            sr.enabled = false;
    }
}
