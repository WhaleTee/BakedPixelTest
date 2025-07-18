using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<ItemSO> items = new();

    public Item GetItem(string itemID)
    {
        var itemSo = items.FirstOrDefault(item => item.ID == itemID);
        if (itemSo != null) return itemSo.GetData();

        Debug.LogError($"Item not found: {itemID}");
        return null;
    }

    public string GetItemID(Item item)
    {
        var itemSo = items.FirstOrDefault(i => i.GetDataType() == item.GetType());
        if (itemSo != null) return itemSo.ID;

        Debug.LogError($"Item ID not found: {item.Name}");
        return null;
    }
}