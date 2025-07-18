using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventorySystem : PersistentSingleton<InventorySystem>
{
    public event Action OnInventoryChanged;
    public event Action OnCoinsChanged;
    [field: SerializeField] public int TotalSlots { get; private set; } = 30;
    [field: SerializeField] public int UnlockSlotsCost { get; private set; } = 75;
    [field: SerializeField] public int UnlockSlotsAtStart { get; private set; } = 15;
    [field: SerializeField] public int Coins { get; private set; } = 1125;

    [Space]
    [Header("ItemDatabase")]
    [SerializeField] private ItemDatabase itemDatabase;
    
    public float TotalWeight { get; private set; }

    private InventorySlot[] slots;
    private int UnlockedSlots { get; set; }

    private void Start()
    {
        slots = new InventorySlot[TotalSlots];
        for (var i = 0; i < TotalSlots; i++)
        {
            slots[i] = new InventorySlot { IsLocked = true };
            if (UnlockedSlots < UnlockSlotsAtStart) UnlockSlot(slots[i]);
        }

        OnInventoryChanged += CalculateWeight;
        ApplySaveData(MobileSaveSystem.LoadInventory());
    }

    private void OnDestroy()
    {
        SaveInventory();
    }

    private void SaveInventory()
    {
        var saveData = new InventorySaveData
        {
            coins = Coins
        };

        for (var i = 0; i < TotalSlots; i++)
        {
            var slotData = new SlotSaveData
            {
                isLocked = slots[i].IsLocked,
                count = slots[i].Count
            };

            if (!slots[i].IsEmpty)
            {
                slotData.itemID = itemDatabase.GetItemID(slots[i].Item);
                slotData.count = slots[i].Count;
            }

            saveData.slots.Add(slotData);
        }

        MobileSaveSystem.SaveInventory(saveData);
    }
    
    private void ApplySaveData(InventorySaveData saveData)
    {
        if (saveData == null) return;
        Coins = saveData.coins;
        UnlockedSlots = saveData.slots.Count(slot => !slot.isLocked);

        for (var i = 0; i < TotalSlots && i < saveData.slots.Count; i++)
        {
            slots[i].IsLocked = saveData.slots[i].isLocked;
            
            if (saveData.slots[i].itemID != null)
            {
                // Загружаем данные из ScriptableObject
                var item = itemDatabase.GetItem(saveData.slots[i].itemID);
                slots[i].SetItem(item, saveData.slots[i].count);
            }
            else
            {
                slots[i].Clear();
            }
        }
        
        OnInventoryChanged?.Invoke();
        OnCoinsChanged?.Invoke();
    }

    public void SwapSlots(int i1, int i2)
    {
        if (i1 == i2) return;
        if (i1 >= TotalSlots || i1 < 0) return;
        if (i2 >= TotalSlots || i2 < 0) return;
        (slots[i1], slots[i2]) = (slots[i2], slots[i1]);
        OnInventoryChanged?.Invoke();
    }

    public void AddItem(Item item, int amount = 1)
    {
        for (var i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            if (!slot.IsEmpty && slot.Item.GetType() == item.GetType() && slot.Count < slot.Item.MaxStack)
            {
                if (amount <= 0) break;
                var canAddThisSlot = slot.Item.MaxStack - slot.Count;
                if (canAddThisSlot >= amount)
                {
                    slot.AddCount(amount);
                    canAddThisSlot -= amount;
                    amount = 0;
                    Debug.Log($"Successfully added {slot.Item.GetInfo()} count of {amount} to the inventory slot {i}.");
                }

                if (canAddThisSlot <= 0) continue;
                amount = Mathf.Abs(canAddThisSlot - amount);
                slot.AddCount(canAddThisSlot);

                Debug.Log(
                    $"Successfully added {slot.Item.GetInfo()} count of {canAddThisSlot} to the inventory slot {i}.");
            }
        }

        if (amount > 0)
        {
            for (var i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];
                if (slot.IsEmpty && !slot.IsLocked)
                {
                    slot.SetItem(item, amount);
                    Debug.Log($"Successfully added {item.GetInfo()} count of {amount} to the inventory slot {i}.");
                    amount = 0;
                    break;
                }
            }

            if (amount > 0)
                Debug.LogError($"Can't add {item.Name} count of {amount} to the inventory");
        }

        OnInventoryChanged?.Invoke();
    }

    public void ClearSlot()
    {
        var itemSlots = new Dictionary<int, InventorySlot>();

        for (var i = 0; i < slots.Length; i++)
            if (!slots[i].IsEmpty)
                itemSlots.Add(i, slots[i]);


        if (itemSlots.Count > 0)
        {
            var randomIndex = itemSlots.Keys.ToList()[Random.Range(0, itemSlots.Count)];
            var slot = itemSlots.First(kv => kv.Key == randomIndex).Value;
            var itemName = slot.Item.Name;
            slot.Clear();
            Debug.Log($"Successfully cleared slot {randomIndex} from {itemName}.");
            OnInventoryChanged?.Invoke();
        }
        else Debug.LogError($"Can't clear any slot no items found!");
    }

    public void Shoot()
    {
        var ammoSlots = slots.Where(slot => !slot.IsEmpty && slot.Item is Ammo).ToArray();
        if (ammoSlots.Length > 0)
        {
            var ammoSlot = ammoSlots[Random.Range(0, ammoSlots.Length)];
            if (ammoSlot.Item is Ammo ammo)
            {
                var weaponSlot = slots
                    .FirstOrDefault(slot =>
                        !slot.IsEmpty && slot.Item is Weapon weapon && weapon.RequiredAmmo == ammo.Type);
                if (weaponSlot is { Item: Weapon weapon })
                {
                    ammoSlot.RemoveCount(1);
                    Debug.Log($"Shot from {weapon.Name} with {ammo.Name}, damage: {weapon.Damage}");
                }
                else Debug.LogError($"Can't shoot by {ammo.Name} no weapon found!");
            }
        }
        else Debug.LogError($"Can't shoot no ammo found!");


        OnInventoryChanged?.Invoke();
    }

    public void UnlockSlot(int slotIndex)
    {
        if (Coins < UnlockSlotsCost || UnlockedSlots >= TotalSlots || !slots[slotIndex].IsLocked)
        {
            Debug.LogError($"Can't unlock new slot!");
            return;
        }

        UnlockSlot(slots[slotIndex]);
    }

    public void UnlockSlot(InventorySlot slot)
    {
        slot.IsLocked = false;
        UnlockedSlots++;
        Coins -= UnlockSlotsCost;

        OnCoinsChanged?.Invoke();
        OnInventoryChanged?.Invoke();
    }

    private void CalculateWeight()
    {
        TotalWeight = 0;
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && !slot.IsLocked)
                TotalWeight += slot.Item.Weight * slot.Count;
        }
    }

    public InventorySlot GetSlot(int index) => slots[index];

    public void AddCoins(int coins)
    {
        Coins += coins;
        OnCoinsChanged?.Invoke();
    }
}