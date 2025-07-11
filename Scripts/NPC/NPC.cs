using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour
{
    protected bool canInteract;
    [SerializeField] protected GameObject showInteractImage;
    protected virtual void Update()
    {
        if((Input.GetKeyDown(KeyCode.E) || InputManager.Instance.attacked) && canInteract)
        {
            OnInteract();
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
    protected abstract void OnInteract();
}
