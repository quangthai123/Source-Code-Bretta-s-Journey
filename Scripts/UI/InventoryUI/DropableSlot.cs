using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DropableSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            Debug.Log("Dropped!");
            eventData.pointerDrag.GetComponent<Transform>().position = transform.position;
            eventData.pointerDrag.GetComponent<Transform>().SetParent(transform, true);
        }
    }
}
