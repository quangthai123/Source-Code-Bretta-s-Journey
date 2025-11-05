using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FollowPlayerBodyPart
{
    body,
    leftLeg,
    rightLeg,
    midTwoLeg,
}
public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private FollowPlayerBodyPart bodyPart = FollowPlayerBodyPart.body;
    private Player player;
    private void OnEnable()
    {
        player = Player.Instance;
    }
    private void Start()
    {
        player = Player.Instance;
    }
    private void LateUpdate()
    {
        switch (bodyPart)
        {
            case FollowPlayerBodyPart.leftLeg:
                transform.position = new Vector3(player.leftEffectPos.position.x, player.leftEffectPos.position.y, -5f);
                break;
            case FollowPlayerBodyPart.rightLeg:
                transform.position = new Vector3(player.rightEffectPos.position.x, player.rightEffectPos.position.y, -5f);
                break;
            case FollowPlayerBodyPart.midTwoLeg:
                transform.position = new Vector3(player.centerEffectPos.position.x, player.centerEffectPos.position.y, -5f);
                break;
            default:
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -5f);
                break;
        }
    }
}
