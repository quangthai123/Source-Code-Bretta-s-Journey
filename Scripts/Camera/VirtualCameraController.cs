using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    private void Start()
    {
        CinemachineVirtualCamera cam = GetComponent<CinemachineVirtualCamera>();
        Transform player = Player.Instance.transform;
        cam.LookAt = player;
        cam.Follow = player;
    }
}
