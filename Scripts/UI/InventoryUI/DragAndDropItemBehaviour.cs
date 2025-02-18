using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragAndDropItemBehaviour : MonoBehaviour, IDragHandler
{
    private Transform oldParent;
    private void Start()
    {
        oldParent = transform.parent;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //transform.SetParent(InventoryUI.Instance.transform, true);
    }

    //public void OnDrop(PointerEventData eventData)
    //{
    //    //if()
    //}
}
