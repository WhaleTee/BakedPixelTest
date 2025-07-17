using UnityEngine;

[CreateAssetMenu(fileName = "Ammo", menuName = "Ammo", order = 0)]
public class AmmoSO : ItemSO
{
    [field: SerializeField] public AmmoType Type { get; protected set; }
    
    public override string GetInfo() => base.GetInfo() + $"\nType: {Type}";
    public override Item GetData() => new Ammo(Name, Weight, Icon, MaxStack, Type);
}