using TMPro;
using UnityEngine;

public class Lock : MonoBehaviour
{
    private InventorySlotUI slotUI;

    private void Awake()
    {
        slotUI = GetComponentInParent<InventorySlotUI>();
        GetComponentInChildren<TMP_Text>().text = InventorySystem.Instance.UnlockSlotsCost.ToString();
    }

    public void TryUnlock() => slotUI.Unlock();
}