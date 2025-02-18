using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioRightGround : MonoBehaviour
{
    [SerializeField] private List<GameObject> dirtiesFromGroundScenario;
    private void Start()
    {
        foreach (GameObject go in dirtiesFromGroundScenario)
        {
            go.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Can Collide Player"))
        {
            GameManager.Instance.CreateScreenShakeFx(GameManager.Instance.strongEarthQuake);
            foreach (GameObject go in dirtiesFromGroundScenario)
            {
                go.SetActive(true);
            }
        }
    }
}
