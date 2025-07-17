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
    
    public Lock @lock { get; private set; }

    private int slotIndex;

    public void Initialize(int index)
    {
        slotIndex = index;
        @lock = GetComponentInChildren<Lock>();
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        var slot = InventorySystem.Instance.GetSlot(slotIndex);

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
        var slot = InventorySystem.Instance.GetSlot(slotIndex);
        
        if (!slot.IsEmpty && !slot.IsLocked) OnShowSlotInfoPopup?.Invoke(slotIndex);
    }

    public void Unlock() => InventorySystem.Instance.UnlockSlot(slotIndex);
}