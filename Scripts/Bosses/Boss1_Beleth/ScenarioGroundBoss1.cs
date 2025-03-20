using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScenarioGroundBoss1 : MonoBehaviour
{
    public bool startScenario;
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private Rigidbody2D leftGroundRb;
    [SerializeField] private Rigidbody2D rightGroundRb;
    [SerializeField] private List<GameObject> dirtiesFromGroundScenario;
    [SerializeField] private Transform cinemachine;
    [SerializeField] private GameObject boss1;
    [SerializeField] private GameObject bossHpUI;

    private bool seeLeftGround = false;

    private void Start()
    {
        foreach (GameObject go in dirtiesFromGroundScenario)
        {
            go.SetActive(false);
        }
        //leftGroundRb.transform.position = new Vector3(0f, 25f, 0f);
        //rightGroundRb.transform.position = new Vector3(0f, 25f, 0f);
        leftGroundRb.gravityScale = 0f;
        rightGroundRb.gravityScale = 0f;
        boss1.SetActive(false);
        bossHpUI.SetActive(false);  
    }
    private void Update()
    {
        if (boss1.GetComponent<Boss1>().isDead) 
        { 
            bossHpUI.SetActive(false);
            leftGroundRb.gameObject.SetActive(false);
            rightGroundRb.gameObject.SetActive(false);
        }
        if (startScenario && !seeLeftGround)
        {
            seeLeftGround = true;
            GameManager.Instance.HideAllInGameUI();
            cinemachine.GetComponent<CinemachineVirtualCamera>().Follow = leftPoint;
            cinemachine.GetComponent<CinemachineVirtualCamera>().LookAt = leftPoint;
            leftGroundRb.gravityScale = 5f;
            Invoke("LookAtRightPoint", 3f);
            //cinemachine.transform.position = Vector2.MoveTowards(cinemachine.position, leftPoint.position, moveSpeed * Time.deltaTime);
        }
    }
    public void StartBossPhase()
    {
        startScenario = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Can Collide Player"))
        {
            GameManager.Instance.CreateScreenShakeFx(GameManager.Instance.strongEarthQuake);
            foreach(GameObject go in dirtiesFromGroundScenario)
            {
                go.SetActive(true);
            }       
        }
    }
    private void LookAtRightPoint()
    {
        cinemachine.GetComponent<CinemachineVirtualCamera>().Follow = rightPoint;
        cinemachine.GetComponent<CinemachineVirtualCamera>().LookAt = rightPoint;
        rightGroundRb.gravityScale = 5f;
        if(Player.Instance.facingDir == -1)
            Player.Instance.Flip();
        Invoke("BackToLookAtPlayer", 3f);
    }
    private void BackToLookAtPlayer()
    {
        cinemachine.GetComponent<CinemachineVirtualCamera>().Follow = Player.Instance.transform;
        cinemachine.GetComponent<CinemachineVirtualCamera>().LookAt = Player.Instance.transform;
        leftGroundRb.bodyType = RigidbodyType2D.Static;
        rightGroundRb.bodyType = RigidbodyType2D.Static;
        Invoke("ActiveBoss1", 3f);
    }
    private void ActiveBoss1()
    {
        boss1.SetActive(true);
        Invoke("ShowBoss1HpBar", 3f);
    }
    private void StartBoss1Phase()
    {
        GameManager.Instance.ShowAllInGameUI();
        boss1.GetComponent<Boss1>().startBoss1Phase = true;
        AudioManager.instance.PlayBGM(0);
    }
    private void ShowBoss1HpBar()
    {
        bossHpUI.SetActive(true);
        Invoke("StartBoss1Phase", 3f);
    }
}
