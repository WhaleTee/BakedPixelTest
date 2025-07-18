using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon", order = 0)]
public class WeaponSO : ItemSO
{
    [field: SerializeField] public AmmoType RequiredAmmo { get; protected set; }
    [field: SerializeField] public int Damage { get; protected set; }
    
    public override string GetInfo() => base.GetInfo() + $"\nDamage: {Damage}\nAmmo: {RequiredAmmo}";
    
    public override Item GetData() => new Weapon(Name, Weight, Icon, MaxStack, RequiredAmmo, Damage);
    public override Type GetDataType() => typeof(Weapon);
}