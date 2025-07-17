using System.Collections;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text weightText;
    [SerializeField] private GameObject slotInfoPopup;
    [SerializeField] private float slotInfoPopupLifetime;

    private InventorySlotUI[] slotUIs;
    
    private Coroutine slotInfoPopupCoroutine;

    private void Awake()
    {
        InitSlots();
    }

    private void Start()
    {
        for (var i = 0; i < InventorySystem.Instance.TotalSlots; i++)
        {
            var slotUI = slotUIs[i];
            slotUI.Initialize(i);
            slotUI.OnShowSlotInfoPopup += ShowPopup;
        }
        InventorySystem.Instance.OnInventoryChanged += UpdateUI;
        InventorySystem.Instance.OnCoinsChanged += UpdateCoins;
    }

    private void ShowPopup(int slotIndex)
    {
        var slot = InventorySystem.Instance.GetSlot(slotIndex);
        if (slotInfoPopupCoroutine != null) StopCoroutine(slotInfoPopupCoroutine);
        slotInfoPopupCoroutine = StartCoroutine(ShowPopupForSeconds(slot, slotInfoPopupLifetime));
    }

    private IEnumerator ShowPopupForSeconds(InventorySlot slot, float time)
    {
        var info = slot.Item.GetInfo();
        slotInfoPopup.SetActive(true);
        slotInfoPopup.GetComponentInChildren<TMP_Text>().text = info;
        yield return new WaitForSeconds(time);
        slotInfoPopup.SetActive(false);
    }

    private void InitSlots()
    {
        slotUIs = new InventorySlotUI[InventorySystem.Instance.TotalSlots];
        for (var i = 0; i < InventorySystem.Instance.TotalSlots; i++)
        {
            var slot = Instantiate(slotPrefab, itemsContainer);
            slotUIs[i] = slot.GetComponent<InventorySlotUI>();
        }
    }

    private void UpdateUI()
    {
        for (var i = 0; i < InventorySystem.Instance.TotalSlots; i++)
        {
            slotUIs[i].UpdateSlot();
        }

        weightText.text = $"Вес инвентаря: {InventorySystem.Instance.TotalWeight:F2}кг";
    }

    private void UpdateCoins()
    {
        coinsText.text = $"Монеты: {InventorySystem.Instance.Coins}";
    }
}