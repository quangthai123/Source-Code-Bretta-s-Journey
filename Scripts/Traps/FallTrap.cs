using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FallTrap : CanDamagePlayer
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.GetComponent<Player>() != null)
        {
            if (!collision.transform.parent.GetComponent<Player>().isDead)
            {
                collision.GetComponentInParent<Player>().GetDamage(transform, 0, true, true);
            }
        }
    }
}
