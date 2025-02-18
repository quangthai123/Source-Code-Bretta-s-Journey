using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class PlayerSoulController : MonoBehaviour
{
    public static PlayerSoulController instance;
    [SerializeField] private float detectPlayerRadius;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float flySpeed;
    [Space]
    [SerializeField] private Transform attackPointPos;
    [SerializeField] private float attackRadius;
    public int damage;
    private Animator anim;
    private Player player;
    private int facingDir;
    private bool endAttack;
    private bool canChasePlayer = false;
    private bool startChase = false;
    //private EntityFx entityFx;
    [SerializeField] private float deathFxCooldown;
    [SerializeField] private float delayDuration;
    public int hp;
    private SpriteRenderer sr;
    public bool isDead;
    public bool startCoroutine;
    private bool startBackToPlayer;
    public int currencySoul;
    private Transform soulFx;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        
    }
    private void OnEnable()
    {
        soulFx = null;
        anim = GetComponentInChildren<Animator>();
        player = Player.Instance;
        sr = GetComponentInChildren<SpriteRenderer>();
        StopAllCoroutines();
        sr.enabled = true;
        canChasePlayer = false;
        startChase = false;
        endAttack = true;
        isDead = false;
        startCoroutine = false;
        startBackToPlayer = false;
        damage = (int)player.playerStats.damage.GetValue();
        hp = (int)player.playerStats.damage.GetValue() * 3;
        currencySoul = SaveManager.instance.tempGameData.currencySoul;
        soulFx = PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.soulDeadFx, transform.position, Quaternion.identity);
        anim.ResetTrigger("isDead");
    }
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        player = Player.Instance;
        //entityFx = GetComponent<EntityFx>();
        sr = GetComponentInChildren<SpriteRenderer>();
        //model = transform.Find("Model");
        facingDir = 1;
        canChasePlayer = false;
        startChase = false;
        endAttack = true;
        isDead = false;
        startCoroutine = false;
        startBackToPlayer = false;
        damage = (int)player.playerStats.damage.GetValue();
        hp = (int)player.playerStats.damage.GetValue() * 3;
        currencySoul = SaveManager.instance.tempGameData.currencySoul;
    }
    private void Update()
    {
        if(soulFx != null)
        {
            if(soulFx.gameObject.activeSelf)
                soulFx.position = transform.position;
        }
        if (Player.Instance.isDead)
        {
            anim.SetTrigger("Idle");
            return;
        } 
        //{
        //    anim.ResetTrigger("Idle");
        //}
        if (hp <= 0)
            isDead = true;
        if (!isDead)
        {
            if (DetecPlayer() && !startChase)
            {
                startChase = true;
                anim.SetTrigger("StartChase");
            }
            if (!CheckPlayerInAttackRange() && startChase)
                canChasePlayer = true;
            else if (CheckPlayerInAttackRange())
                canChasePlayer = false;
            if ((player.transform.position.x < transform.position.x && facingDir == 1) || (player.transform.position.x > transform.position.x && facingDir == -1))
                Flip();
            if (canChasePlayer && endAttack)
            {
                //anim.SetBool("Idle", false);
                anim.SetBool("Attack", false);
                anim.SetBool("Fly", true);
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, flySpeed * Time.deltaTime);
            }
            else if (!canChasePlayer && startChase && endAttack)
            {
                endAttack = false;
                //anim.SetBool("Idle", false);
                anim.SetBool("Fly", false);
                anim.SetBool("Attack", true);
            }
        } else
        {
            anim.SetTrigger("isDead");
            if (Vector2.Distance(transform.position, player.transform.position) > .1f && !startCoroutine)
            {
                startCoroutine = true;
                StartCoroutine(SoulDeathFx());
                StartCoroutine(DelayDurationCounter());
                PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.soulDeadFx, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            }
            else if(Vector2.Distance(transform.position, player.transform.position) > .1f && startBackToPlayer)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, flySpeed * 6f * Time.deltaTime);
            }
            else if(Vector2.Distance(transform.position, player.transform.position) <= .1f)
            {
                startCoroutine = false;
                player.playerStats.haveDied = false;
                SaveManager.instance.tempGameData.haveDied = false;
                SaveManager.instance.tempGameData.soulPos = Vector2.zero;
                SaveManager.instance.tempGameData.soulScene = string.Empty;
                SaveManager.instance.tempGameData.currencySoul = 0;
                player.playerStats.currency += currencySoul;
                SaveManager.instance.tempGameData.currency = player.playerStats.currency;
                PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.soulDeadFx, transform.position, Quaternion.identity);
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
    }
    private bool DetecPlayer() => Physics2D.OverlapCircle(transform.position, detectPlayerRadius, playerLayer);
    private bool CheckPlayerInAttackRange() => Physics2D.OverlapCircle(attackPointPos.position, attackRadius, playerLayer);
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectPlayerRadius);
        Gizmos.DrawWireSphere(attackPointPos.position, attackRadius);
    }
    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingDir *= -1;
    }
    public void ConvertToChaseAnim()
    {
        anim.SetBool("Attack", false);
        anim.SetBool("Fly", true);
        //anim.SetBool("Idle", true);
        //attackTimer = attackCoolDown;
        endAttack = true;
    }
    public void DoDamagePlayer(int attackWeight)
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPointPos.position, attackRadius, playerLayer);
        if (hit != null)
        {
            if (hit.GetComponentInParent<Player>() != null)
            {
                hit.GetComponentInParent<Player>().GetDamage(transform, attackWeight, false, false);
            }
        }
    }
    public void GetDamage(Transform opponentTransform, int attackWeight, float _damage)
    {
        //float rdX = Random.Range(0f, 1f);
        //float rdY = Random.Range(-1f, 1f);
        //Vector2 rdPos = new Vector2(player.attackEffectPos.position.x + rdX, player.attackEffectPos.position.y + rdY);
        //switch (attackWeight)
        //{
        //    case 0:
        //        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attackImpactFx, rdPos, Quaternion.identity); break;
        //    case 1:
        //        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attackImpact2Effect, rdPos, Quaternion.identity); break;
        //    case 2:
        //        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attackImpact3Effect, new Vector2(player.attackEffectPos.position.x, player.attackEffectPos.position.y + .33f), Quaternion.identity); break;
        //    case 3:
        //        PlayerEffectSpawner.instance.Spawn(PlayerEffectSpawner.instance.attackImpact4Effect, rdPos, Quaternion.identity); break;
        //}
        this.hp -= (int)_damage;
    }
    private IEnumerator SoulDeathFx()
    {
        sr.enabled = false;
        yield return new WaitForSeconds(deathFxCooldown);
        StartCoroutine(SoulDeathFx2());
    }
    private IEnumerator SoulDeathFx2()
    {
        sr.enabled = true;
        yield return new WaitForSeconds(deathFxCooldown);
        StartCoroutine(SoulDeathFx());
    }
    private IEnumerator DelayDurationCounter()
    {
        yield return new WaitForSeconds(delayDuration);
        startBackToPlayer = true;
    }
}
