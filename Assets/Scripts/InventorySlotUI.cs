using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject infoPopupButton;
    [SerializeField] private TMP_Text countText;
    
    public event Action<int> OnShowSlotInfoPopup;

    private Lock @lock;

    public int SlotIndex { get; private set; }

    private void Awake()
    {
        @lock = GetComponentInChildren<Lock>();
    }

    public void Initialize(int index)
    {
        SlotIndex = index;
        name = $"Slot {SlotIndex}";
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        var slot = InventorySystem.Instance.GetSlot(SlotIndex);
        
        @lock.gameObject.SetActive(slot.IsLocked);
        if (slot.IsLocked)
        {
            icon.gameObject.SetActive(false);
            infoPopupButton.SetActive(false);
            return;
        }

        if (!slot.IsEmpty)
        {
            icon.sprite = slot.Item.Icon;
            countText.text = slot.Count > 1 ? slot.Count.ToString() : "";
            icon.gameObject.SetActive(true);
            infoPopupButton.SetActive(true);
        }
        else
        {
            icon.gameObject.SetActive(false);
            infoPopupButton.SetActive(false);
            countText.text = "";
        }
    }

    public void OnShowPopupButton()
    {
        var slot = InventorySystem.Instance.GetSlot(SlotIndex);
        
        if (!slot.IsEmpty && !slot.IsLocked) OnShowSlotInfoPopup?.Invoke(SlotIndex);
    }

    public void Unlock() => InventorySystem.Instance.UnlockSlot(SlotIndex);
}