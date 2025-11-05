using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private BoxCollider2D boxCol;
    private Transform player;
    [SerializeField] private bool canJumpDown = false;
    private bool jumpedDown = false;
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        player = Player.Instance.transform;
    }
    private void Update()
    {
        if (player.position.y >= transform.position.y + 1.4f && !jumpedDown && Player.Instance.stateMachine.currentState != Player.Instance.ladderState)
        {
            boxCol.enabled = true;
        } else if(player.position.y < transform.position.y + 1.4f)
        {
            boxCol.enabled = false;
            canJumpDown = false;
            jumpedDown = false;
        }
        if(Player.Instance.canLadder && (Input.GetAxisRaw("Vertical") != 0f || InputManager.Instance.moveDir.y != 0))
            boxCol.enabled = false;
        if(canJumpDown && (Player.Instance.stateMachine.currentState == Player.Instance.crouchState ||
            Player.Instance.stateMachine.currentState == Player.Instance.enterCrouchState))
        {
            if(InputManager.Instance.jumped || Input.GetKeyDown(KeyCode.Space))
            {
                boxCol.enabled = false;
                jumpedDown = true;
                Debug.Log("Jump down!");
            }
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (Player.Instance.stateMachine.currentState == Player.Instance.jumpState)
    //    {
    //        Debug.Log("Bo may chuyen ho state day");
    //        Player.Instance.stateMachine.ChangeState(Player.Instance.idleState);
    //    }     
    //}
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
            canJumpDown = true;
            //jumpedDown = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canJumpDown = false;
        }
    }
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player" && (Input.GetKey(KeyCode.S) || InputManager.Instance.moveDir.y==-1))
    //    {
    //        canJumpDown = true;
    //    }
    //}
}
