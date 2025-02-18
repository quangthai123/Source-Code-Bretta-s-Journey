using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = Player.Instance;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && (Input.GetAxisRaw("Vertical") != 0 || InputManager.Instance.moveDir.y == -1 || InputManager.Instance.jumped) && player.stateMachine.currentState != player.jumpState)
        {
            if (player.canLadder)
            {
                player.transform.position = new Vector2(transform.position.x, player.transform.position.y);
                player.stateMachine.ChangeState(player.ladderState);
            }
        }
    }
}
