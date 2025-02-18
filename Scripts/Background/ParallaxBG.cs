using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;
    private Transform cameraTransf;
    private float bgLength;
    private float xPosition;
    void Start()
    {
        bgLength = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
        cameraTransf = Camera.main.transform;
    }
    void Update()
    {
        float distanceOffsetWithCam = cameraTransf.position.x * (1 - parallaxEffect);
        float distanceToMove = cameraTransf.position.x * parallaxEffect;
        transform.position = new Vector2(xPosition + distanceToMove, transform.position.y);
        if (distanceOffsetWithCam > xPosition + bgLength)
            xPosition += bgLength;
        else if (distanceOffsetWithCam < xPosition - bgLength)
            xPosition -= bgLength;
    }
}
