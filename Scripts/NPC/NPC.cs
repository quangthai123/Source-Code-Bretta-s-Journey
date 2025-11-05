using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InteractNpcStateType
{
    Idle,
    Rest
}
public abstract class NPC : MonoBehaviour
{
    protected Player player;
    protected bool canInteract;
    protected bool interacted;
    [SerializeField] protected GameObject showInteractImage;
    [Header("Player Interact Info")]
    [SerializeField] protected float targetPosX;
    [SerializeField] protected int targetFacingDir;
    [SerializeField] protected InteractNpcStateType interactType;
    protected virtual void Start()
    {
        player = Player.Instance;
    }
    protected virtual void Update()
    {
        if((Input.GetKeyDown(KeyCode.E) || InputManager.Instance.attacked) 
            && canInteract && !interacted && player.isActiveAndEnabled)
        {
            interacted = true;
            StartInteract();
        }
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canInteract = true;
            showInteractImage.SetActive(true);
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canInteract = false;
            showInteractImage.SetActive(false);
        }
    }
    protected virtual void StartInteract()
    {
        PlayScreenUI.instance.HideControlUI();
        PlayerStates targetState;
        switch (interactType)
        {
            case InteractNpcStateType.Rest:
                targetState = player.restState;
                break;
            default:
                targetState = player.idleState;
                break;
        }
        player.InteractNpc(new Vector2(targetPosX, player.transform.position.y), targetFacingDir, targetState, OnInteract);
    }
    protected virtual void OnInteract()
    {
        interacted = false;
    }
}
