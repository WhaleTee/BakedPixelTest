using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        InventorySystem.Instance.OnSlotSwapped += SwapSlots;
    }

    private void OnEnable()
    {
        InventorySystem.Instance.OnCoinsChanged += UpdateCoins;
        InventorySystem.Instance.OnInventoryChanged += UpdateWeightUI;
    }

    private void OnDestroy()
    {
        InventorySystem.Instance.OnInventoryChanged -= UpdateUI;
        InventorySystem.Instance.OnInventoryChanged -= UpdateWeightUI;
        InventorySystem.Instance.OnCoinsChanged -= UpdateCoins;
        InventorySystem.Instance.OnSlotSwapped -= SwapSlots;
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
    }

    private void UpdateWeightUI()
    {
        weightText.text = $"Вес инвентаря: {InventorySystem.Instance.TotalWeight:F2}кг";
    }

    private void UpdateCoins()
    {
        coinsText.text = $"Монеты: {InventorySystem.Instance.Coins}";
    }

    private void SwapSlots(int i1, int i2)
    {
        slotUIs[i1].transform.SetSiblingIndex(i2);
        slotUIs[i1].Initialize(i2);
        slotUIs[i2].transform.SetSiblingIndex(i1);
        slotUIs[i2].Initialize(i1);
        (slotUIs[i1], slotUIs[i2]) = (slotUIs[i2], slotUIs[i1]);
        LayoutRebuilder.ForceRebuildLayoutImmediate(slotUIs[i1].transform.parent.GetComponent<RectTransform>());
    }
}