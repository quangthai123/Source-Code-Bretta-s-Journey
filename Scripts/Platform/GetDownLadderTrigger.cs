using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDownLadderTrigger : MonoBehaviour
{
    private Transform getDownPos;
    private Transform getUpPos;
    //private bool getDownToLadder = false;
    private void Start()
    {
        getDownPos = transform.parent.Find("Player Get Down Pos");
        getUpPos = transform.parent.Find("Player Get Up Pos");
    }
    //private void Update()
    //{
    //    if(!getDownToLadder && Player.Instance.stateMachine.currentState == Player.Instance.ladderState
    //        && (Input.GetAxisRaw("Vertical") < 0f || InputManager.Instance.moveDir.y == -1))
    //    {
    //        getDownToLadder = true;
    //        Player.Instance.transform.position = getDownPos.position;
    //    }
    //}
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && (Input.GetAxisRaw("Vertical") < 0f || InputManager.Instance.moveDir.y == -1) && Player.Instance.stateMachine.currentState != Player.Instance.ladderState)
        {
            Debug.Log("Get Down!");
            Player.Instance.stateMachine.ChangeState(Player.Instance.ladderState);
            Player.Instance.transform.position = getDownPos.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && (Input.GetAxisRaw("Vertical") > 0f || InputManager.Instance.jumped) && Player.Instance.stateMachine.currentState == Player.Instance.ladderState)
        {
            Debug.Log("Get Up!");
            Player.Instance.transform.position = getUpPos.position;
            Player.Instance.stateMachine.ChangeState(Player.Instance.ladderOutState);
        }
    }
}
