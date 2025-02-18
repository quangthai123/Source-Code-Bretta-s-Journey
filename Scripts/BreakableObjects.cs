using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjects : MonoBehaviour
{
    private Animator anim;
    private Player player;
    private bool beAttacked = false;
    void Start()
    {
        anim = GetComponent<Animator>();
        player = Player.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BeAttackByPlayer()
    {
        if(beAttacked) return;
        beAttacked = true;
        anim.SetTrigger("Break");
    }
    private void DeactiveAfterFinishBreakAnim()
    {
        gameObject.SetActive(false);
    }
}
