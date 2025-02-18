using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoss1PhaseTrigger : MonoBehaviour
{
    [SerializeField] private GameObject challengeText;
    [SerializeField] private GameObject notifiBoss1PhaseUI;
    [SerializeField] private Boss1 boss1;
    [SerializeField] private ScenarioGroundBoss1 scenarioGroundBoss1;
    private bool canChallenge = false;
    private void Start()
    {
        if(SaveManager.instance.tempGameData.winBoss1)
            gameObject.SetActive(false);
        challengeText.SetActive(false);
        notifiBoss1PhaseUI.SetActive(false);
    }
    private void Update()
    {
        if(boss1.startBoss1Phase)
            canChallenge = false;
        if (canChallenge)
        {
            challengeText.SetActive(true);
            if(InputManager.Instance.moveDir.y == -1 || Input.GetKeyDown(KeyCode.S))
            {
                canChallenge = false;
                InputManager.Instance.moveDir.y = 0;
                notifiBoss1PhaseUI.SetActive(true);
                GameManager.Instance.HideAllInGameUI();
            }
        } else
            challengeText.SetActive(false);
    }
    public void AcceptChallengeUILogic()
    {
        challengeText.SetActive(false);
        notifiBoss1PhaseUI.SetActive(false);
    }
    public void CloseNotifiBoss1PhaseUI()
    {
        notifiBoss1PhaseUI.SetActive(false);
        GameManager.Instance.ShowAllInGameUI();
        canChallenge = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //canChallenge = true;
            scenarioGroundBoss1.StartBossPhase();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canChallenge = false;
        }
    }
}
