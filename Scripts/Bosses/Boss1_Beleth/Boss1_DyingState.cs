using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_DyingState : EnemyStates
{
    private Boss1 enemy;
    public Boss1_DyingState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Boss1 enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Start()
    {
        base.Start();
        Debug.Log("Counting!!!!!!!!!!!!!");
        enemy.FinishBossSlowMotionFx();
        player.RedFxOnFinishBoss();     
        enemy.RedFxOnFinishBoss();
        AudioManager.instance.StopAllBGM();
        SaveManager.instance.tempGameData.winBoss1 = true;
    }
    public override void Exit()
    {
        base.Exit();
    }


    public override void Update()
    {
        base.Update();
        if (finishAnim)
            enemy.SetActiveFalse();
    }
}
