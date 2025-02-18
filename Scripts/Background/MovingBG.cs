using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBG : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(-moveSpeed * Time.fixedDeltaTime, 0f, 0f);
        if(transform.position.x <= 94f)
            transform.position = new Vector3(286f, transform.position.y, 0f);
    }
}
