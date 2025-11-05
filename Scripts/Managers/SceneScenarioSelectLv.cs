using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScenarioSelectLv : SceneScenario
{
    public static SceneScenarioSelectLv instance;
    //[SerializeField] private GameObject mainUI;
    //[SerializeField] private GameObject currencyUI;
    [SerializeField] private GameObject controlUI;
    [SerializeField] private GameObject flashFxUI;
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject selectLvImage;

    [SerializeField] private PolygonCollider2D bgCollider;
    [SerializeField] private CinemachineConfiner2D bgConfiner;
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        isScenario = false;
    }
    private void Start()
    {
        isScenario = SaveManager.instance.tempGameData.isScenario;
        if(isScenario)
            return;
        Player.Instance.rb.gravityScale = 0f;
        flashFxUI.SetActive(false);
        //mainUI.SetActive(false);
        //currencyUI.SetActive(false);
        controlUI.SetActive(false);
        //mainMenuButton.SetActive(false);
        selectLvImage.SetActive(false);
        bgConfiner.m_BoundingShape2D = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isScenario)
        {
            Player.Instance.rb.linearVelocity = new Vector2(0f, -60f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isScenario)
        {
            flashFxUI.SetActive(true);
        }
    }
    public void FinishScenarioSelectLv()
    {
        if (isScenario)
            return;
        isScenario = true;
        SaveManager.instance.tempGameData.isScenario = true;
        Player.Instance.rb.gravityScale = 6f;
        //mainUI.SetActive(true);
        //currencyUI.SetActive(true);
        controlUI.SetActive(true);
        //mainMenuButton.SetActive(true);
        selectLvImage.SetActive(true);
        bgConfiner.m_BoundingShape2D = bgCollider;
    }
}
