using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0_AnimController : MonoBehaviour
{
    private Enemy0 enemy;
    private void Awake()
    {
        enemy = transform.parent.GetComponent<Enemy0>();
    }
    private void MoveByAnim1()
    {
        if (!enemy.canDamageByAnim)
            enemy.rb.linearVelocity = new Vector2(enemy.moveSpeed * 1/3f * enemy.facingDir, 0f);
    }
    private void MoveByAnim2()
    {
        if (!enemy.canDamageByAnim)
            enemy.rb.linearVelocity = new Vector2(enemy.moveSpeed * 1/2f * enemy.facingDir, 0f);
    }
    private void MoveByAnim3()
    {
        if (!enemy.canDamageByAnim)
            enemy.rb.linearVelocity = new Vector2(enemy.moveSpeed * enemy.facingDir, 0f);
        else
            Player.Instance.GetDamage(enemy.transform, 0, false, false);
    }
}
