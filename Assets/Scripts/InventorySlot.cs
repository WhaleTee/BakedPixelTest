public class InventorySlot
{
    public Item Item { get; private set; }
    public int Count { get; private set; }
    public bool IsEmpty => Item == null;
    public bool IsLocked { get; set; } = true;
    public void Clear()
    {
        Item = null;
        Count = 0;
    }
    public void SetItem(Item item, int count)
    {
        Item = item;
        Count = count;
    }
    public void AddCount(int amount)
    {
        Count += amount;
    }
    public void RemoveCount(int amount)
    {
        Count -= amount;
        if (Count <= 0)
            Clear();
    }
}