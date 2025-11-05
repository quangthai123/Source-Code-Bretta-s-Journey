using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Player player;
    public Transform currentEnemyTarget;
    void Start()
    {
        player = Player.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void SetFinishAnimation()
    {
        Player.Instance.SetFinishCurrentAnimation();
    }
    private void ChangeAttackRangeOnCombo1_2() => player.ChangeAttackRangeOnCombo1_2();
    private void ChangeAttackRangeOnCombo3_4() => player.ChangeAttackRangeOnCombo3_4();
    private void ChangeAttackRangeOnChargedAttack() => player.ChangeAttackRangeOnChargedAttack();
    private void ChangeAttackRangeOnDashAttack() => player.ChangeAttackRangeOnDashAttack();
    private void SetEndDashAttack()
    {
        player.endDashAttack = true;
        PlayerEffectSpawner.instance.Spawn("lightGroundedFx", player.centerEffectPos.position, Quaternion.identity);
    }
    private void SpawnAttack4Fx_Right()
    {
        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attack4Fx, player.attack4FxPos.position, Quaternion.identity);
    }
    private void SpawnCounterAttackFx()
    {
        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.counterAttackFx, player.AttackPointPos.position, Quaternion.Euler(0f, 0f, 40f));
    }
    private void SetPlayerIsBlockingAfterFinsishShield()
    {
        Player.Instance.isShielding = true;
    }
    private void SetPlayerIsNotBlockingAfterFinsishShield()
    {
        Player.Instance.isShielding = false;
    }
    private void SpawnRunEffect()
    {
        AudioManager.instance.PlaySFX(0);
        if (!player.CheckSlope())
            PlayerEffectSpawner.instance.Spawn("runFx", Player.Instance.leftEffectPos.position, Quaternion.identity);
    }
    private void SpawnTurnRunEffect()
    {
        AudioManager.instance.PlaySFX(0);
        if (!player.CheckSlope())
        {
            Transform fx = PlayerEffectSpawner.instance.Spawn("runFx", new Vector2(player.leftEffectPos.position.x - .15f * player.facingDir, player.leftEffectPos.position.y), Quaternion.identity);
            //fx.GetComponent<PlayerEffectDespawnByAnim>().Flip();
        }
    }
    private void StartHealEffect()
    {
        Transform healFx = PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.healEffect, Player.Instance.centerEffectPos.position, Quaternion.identity);
        if (player.playerStatsWithItems.CheckEquippedArmorial(6))
            healFx.GetComponent<Animator>().speed = 2f;
        else
            healFx.GetComponent<Animator>().speed = 1f;
    }
    public void CounterAttackCurrentEnemyTarget()
    {
        if (currentEnemyTarget != null)
        {
            currentEnemyTarget.GetComponentInParent<Enemy>().canBeHitByCounterAttack = true;
        }
        currentEnemyTarget = null;
    }
    public void AttackTrigger(int attackWeight)
    {
        switch (attackWeight)
        {
            case 0:
                player.DoDamageEnemy(attackWeight, player.playerStats.damage.GetValue()); break;
            case 1:
                player.DoDamageEnemy(attackWeight, player.playerStats.damage.GetValue() * 1.1f); break;
            case 2:
                player.DoDamageEnemy(attackWeight, player.playerStats.damage.GetValue() * 1.5f); break;
            case 3:
                if (!SaveManager.instance.tempGameData.learnedSkill[2])
                    player.DoDamageEnemy(attackWeight, player.playerStats.damage.GetValue() * 2.5f); 
                else
                    player.DoDamageEnemy(attackWeight, player.playerStats.damage.GetValue() * 3.5f);
                break;
            case 4: // Counter Attack
                player.DoDamageEnemy(attackWeight, player.playerStats.damage.GetValue() * 3f); break;
            case 5: // Charged Attack
                if (!SaveManager.instance.tempGameData.learnedSkill[6])
                    player.DoDamageEnemy(attackWeight, player.playerStats.damage.GetValue() * 4f); 
                else
                    player.DoDamageEnemy(attackWeight, player.playerStats.damage.GetValue() * 5f);
                break;
        }
    }
    public void LastHitOnDashAttack()
    {
        if (SaveManager.instance.tempGameData.learnedSkill[8])
            player.DoDamageEnemy(0, player.playerStats.damage.GetValue());
    }
    private void Healing()
    {
        player.playerStats.Healing();
    }
    private void SpawnEffectWhenStartUseMagicSkill()
    {
        //PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.useSkillFx2, player.transform.position, Quaternion.identity);
        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.strongGroundedEffect, player.centerEffectPos.position, Quaternion.identity);
    }
    private void UseMagicSkill()
    {
        SkillManager.instance.UseSkillSlot1();
    }
    private void SpawnDustFxOnBreakRun()
    {
        if(!player.CheckSlope())
            PlayerEffectSpawner.instance.Spawn("startDashFx", player.rightEffectPos.position + player.facingDir * new Vector3(.3f, 0f, 0f), Quaternion.identity);
    }
    private void CanHaveNextComboAtk() => player.canHaveNextCombo = true;
    private void SpawnDustFxOnChargedAttack()
    {
        PlayerEffectSpawner.instance.Spawn("endDashFx", player.leftEffectPos.position + new Vector3(-.5f * player.facingDir, 0f, 0f), Quaternion.identity);
    }
    private void PlayAttackNotEnemySound() => AudioManager.instance.PlaySFX(5);
    private void ChangeToFallStateAfterMagic1State()
    {
        if(!player.CheckGrounded() && !player.CheckJumpOnSlope())
            player.stateMachine.ChangeState(player.fallState);
    }
    private void MoveToLadder1()
    {
        player.transform.position = new Vector2(player.LadderPosX - .82f * player.facingDir, player.transform.position.y);
    }
    private void MoveToLadder2()
    {
        player.transform.position = new Vector2(player.LadderPosX - .5f * player.facingDir, player.transform.position.y);
    }
    private void MoveToLadder3()
    {
        player.transform.position = new Vector2(player.LadderPosX, player.transform.position.y);
    }
}