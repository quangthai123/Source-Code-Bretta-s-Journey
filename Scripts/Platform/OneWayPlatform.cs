using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private BoxCollider2D boxCol;
    private Transform player;
    [SerializeField] private bool canJumpDown = false;
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        player = Player.Instance.transform;
    }
    private void Update()
    {
        if (player.position.y >= transform.position.y + 1.4f && !canJumpDown && Player.Instance.stateMachine.currentState != Player.Instance.ladderState)
        {
            boxCol.enabled = true;
        } else if(player.position.y < transform.position.y + 1.4f)
        {
            boxCol.enabled = false;
            canJumpDown = false;
        }
        if(Player.Instance.canLadder && (Input.GetAxisRaw("Vertical") != 0f || InputManager.Instance.moveDir.y != 0))
            boxCol.enabled = false;
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (Player.Instance.stateMachine.currentState == Player.Instance.jumpState)
    //    {
    //        Debug.Log("Bo may chuyen ho state day");
    //        Player.Instance.stateMachine.ChangeState(Player.Instance.idleState);
    //    }     
    //}
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && (Input.GetKey(KeyCode.S) || InputManager.Instance.moveDir.y==-1))
        {
            Debug.Log("Jump down!");
            canJumpDown = true;
            boxCol.enabled = false;
        }
    }
}
