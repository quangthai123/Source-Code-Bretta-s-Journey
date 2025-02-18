using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlaskManager : MonoBehaviour
{
    private PlayerStats playerStats;
    [SerializeField] private List<GameObject> flaskAllLv;
    [SerializeField] private List<GameObject> emptyFlasks;
    [SerializeField] private List<GameObject> fullFlasks; 
    private int flaskLv;
    private int flaskQuantity;
    private int fullHealFlaskQuantity;
    private void Awake()
    {
        
    }
    void Start()
    {
        playerStats = Player.Instance.playerStats;
        flaskLv = playerStats.flaskLv;
        flaskQuantity = playerStats.flaskQuantity;
        fullHealFlaskQuantity = playerStats.fullHealFlaskQuantity;
        flaskAllLv[flaskLv].SetActive(true);
        foreach (Transform empty in flaskAllLv[flaskLv].transform.Find("Empty"))
        {
            emptyFlasks.Add(empty.gameObject);
        }
        foreach (Transform full in flaskAllLv[flaskLv].transform.Find("Full"))
        {
            fullFlasks.Add(full.gameObject);
        }
        for (int i = 0; i < emptyFlasks.Count; i++)
        {
            if (i < flaskQuantity)
                emptyFlasks[i].SetActive(true);
            else
                emptyFlasks[i].SetActive(false);
        }
        for (int i = 0; i < fullFlasks.Count; i++)
        {
            if (i < flaskQuantity)
                fullFlasks[i].SetActive(true);
            else
                fullFlasks[i].SetActive(false);
        }
        for (int i = 0; i < fullFlasks.Count; i++)
        {
            if (i < fullHealFlaskQuantity)
                fullFlasks[i].SetActive(true);
            else
                fullFlasks[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckUpdateFlaskLvAndShowNextFlaskLV();
        CheckAndActiveFlaskByFlaskQuantity();
        CheckHealingOrRefillFlask();
    }

    private void CheckHealingOrRefillFlask()
    {
        if (fullHealFlaskQuantity != playerStats.fullHealFlaskQuantity)
        {
            fullHealFlaskQuantity = playerStats.fullHealFlaskQuantity;
            for (int i = 0; i < fullFlasks.Count; i++)
            {
                if (i < fullHealFlaskQuantity)
                    fullFlasks[i].SetActive(true);
                else
                    fullFlasks[i].SetActive(false);
            }
        }
    }

    private void CheckAndActiveFlaskByFlaskQuantity()
    {
        if (flaskQuantity != playerStats.flaskQuantity)
        {
            flaskQuantity = playerStats.flaskQuantity;
            for (int i = 0; i < emptyFlasks.Count; i++)
            {
                if(i < flaskQuantity)
                    emptyFlasks[i].SetActive(true);
                else
                    emptyFlasks[i].SetActive(false);
            }
            for (int i = 0; i < fullFlasks.Count; i++)
            {
                if(i < flaskQuantity)
                    fullFlasks[i].SetActive(true);
                else
                    fullFlasks[i].SetActive(false);
            }
        }
    }

    private void CheckUpdateFlaskLvAndShowNextFlaskLV()
    {
        if (flaskLv != playerStats.flaskLv)
        {
            flaskLv = playerStats.flaskLv;
            foreach (GameObject flaskLv in flaskAllLv)
            {
                flaskLv.SetActive(false);
            }
            flaskAllLv[flaskLv].SetActive(true);
            emptyFlasks.Clear();
            fullFlasks.Clear();
            foreach (Transform empty in flaskAllLv[flaskLv].transform.Find("Empty"))
            {
                emptyFlasks.Add(empty.gameObject);
            }
            foreach (Transform full in flaskAllLv[flaskLv].transform.Find("Full"))
            {
                fullFlasks.Add(full.gameObject);
            }
        }
    }
}
