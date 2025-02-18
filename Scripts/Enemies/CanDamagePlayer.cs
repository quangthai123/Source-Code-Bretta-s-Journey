using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDamagePlayer : MonoBehaviour
{
    public int damage;
    [SerializeField] protected BoxCollider2D boxCol;
    protected void OnEnable()
    {
        boxCol = GetComponent<BoxCollider2D>();
        boxCol.enabled = true;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponentInParent<Player>().GetDamage(transform, 0, true, false);
        }
    }
    protected void DeactiveColByAnimFrame()
    {
        boxCol.enabled = false;
    }
}
