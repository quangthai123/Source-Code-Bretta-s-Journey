using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private Transform canLadderTopPos;
    [SerializeField] private Transform canLadderBottomPos;
    [SerializeField] private float canLadderDistance = 1f;
    private Player player;
    void Start()
    {
        player = Player.Instance;
    }
    private void Update()
    {
        if(player.InteractingLadder != null)
        {
            if (player.InteractingLadder != transform)
                return;
        }
        if(Mathf.Abs(player.transform.position.x - transform.position.x) <= canLadderDistance)
        {
            player.LadderPosX = transform.position.x;
            player.LadderBottomPosY = canLadderBottomPos.position.y;
            if ((player.transform.position.y < canLadderTopPos.position.y 
                && player.transform.position.y > canLadderBottomPos.position.y) || player.stateMachine.currentState
                == player.laddderInState || player.stateMachine.currentState == player.enterLadderState
                || player.stateMachine.currentState == player.enterLadder1State)
            {
                player.canLadder = true;
                player.InteractingLadder = transform;
            } else
            {
                player.canLadder = false;
                player.InteractingLadder = null;
            }
        } else 
        { 
            player.canLadder = false;
            player.InteractingLadder = null;
        }
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player" && (Input.GetAxisRaw("Vertical") != 0 || InputManager.Instance.moveDir.y == -1 || InputManager.Instance.jumped) && player.stateMachine.currentState != player.jumpState)
    //    {
    //        if (player.canLadder)
    //        {
    //            player.transform.position = new Vector2(transform.position.x, player.transform.position.y);
    //            player.stateMachine.ChangeState(player.ladderState);
    //        }
    //    }
    //}
}
