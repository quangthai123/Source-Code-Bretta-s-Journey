using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosBetweenScenes : MonoBehaviour
{
    private GameDatas tempGameData;
    private Player player;
    void Awake()
    {
        tempGameData = Resources.Load<GameDatas>("TempGameData");
        player = GetComponent<Player>();
        transform.position = tempGameData.initializePos;
        if (player.facingDir != tempGameData.facingDir)
            player.Flip();
    }
}
