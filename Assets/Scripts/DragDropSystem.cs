using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropSystem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private Vector2 dragStartPosition;
    private InventorySlotUI swappedSlot;
    private Vector2 swappedSlotOriginalPosition;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        dragStartPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        var resultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, resultList);
        var result = resultList.FirstOrDefault(i => i.gameObject.GetComponent<InventorySlotUI>());
        if (result.gameObject && result.gameObject.GetComponent<InventorySlotUI>() is { } slot)
        {
            if (swappedSlot != null)
            {
                swappedSlot.transform.position = swappedSlotOriginalPosition;
                if (swappedSlot == slot)
                {
                    swappedSlot = null;
                    return;
                }
            }

            swappedSlot = slot;
            swappedSlotOriginalPosition = swappedSlot.transform.position;
            swappedSlot.transform.position = dragStartPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        if (swappedSlot != null)
        {
            transform.position = swappedSlotOriginalPosition;
            swappedSlot = null;
        } else transform.position = dragStartPosition;
    }
}