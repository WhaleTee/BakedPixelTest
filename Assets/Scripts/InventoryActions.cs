using UnityEngine;

public class InventoryActions : MonoBehaviour
{
    [SerializeField] private int addAmmoCount;
    [SerializeField] private int addCoinsCount;
    [SerializeField] private AmmoSO[] ammos;
    [SerializeField] private ItemSO[] items;

    public void AddAmmo()
    {
        if (ammos.Length <= 0) return;

        foreach (var ammo in ammos)
            InventorySystem.Instance.AddItem(ammo.GetData(), addAmmoCount);
    }

    public void AddRandomItem()
    {
        var randomIndex = Random.Range(0, items.Length);
        InventorySystem.Instance.AddItem(items[randomIndex].GetData());
    }

    public void DeleteRandomItem()
    {
        InventorySystem.Instance.ClearSlot();
    }

    public void Shoot() => InventorySystem.Instance.Shoot();
    public void AddCoins() => InventorySystem.Instance.AddCoins(addCoinsCount);
}