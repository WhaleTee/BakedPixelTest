using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
public abstract class ItemSO : ScriptableObject
{
    [field: SerializeField] public string ID { get; private set; } = Guid.NewGuid().ToString();
    [field: SerializeField] public string Name { get; protected set; }
    [field: SerializeField] public float Weight { get; protected set; }
    [field: SerializeField] public Sprite Icon { get; protected set; }
    [field: SerializeField] public int MaxStack { get; protected set; } = 1;

    public virtual string GetInfo() => $"{Name}\nWeight: {Weight}kg";

    public abstract Item GetData();
    public abstract Type GetDataType();
}