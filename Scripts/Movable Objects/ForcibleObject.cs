using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcibleObject : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Vector2 force;
    public int facingDir;
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(force.x * transform.parent.GetComponent<Enemy>().facingDir, force.y), ForceMode2D.Impulse);
        //rb.AddTorque(5f * transform.parent.GetComponent<Enemy>().facingDir);
    }
    //public void Flip()
    //{
    //    if()
    //}
}
