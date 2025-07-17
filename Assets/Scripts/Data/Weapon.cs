using UnityEngine;

public class Weapon : Item
{
    public AmmoType RequiredAmmo { get; protected set; }
    public int Damage { get; protected set; }

    public Weapon(string name, float weight, Sprite icon, int maxStack, AmmoType requiredAmmo, int damage) 
        : base(name, weight, icon, maxStack)
    {
        RequiredAmmo = requiredAmmo;
        Damage = damage;
    }

    public override string GetInfo() => base.GetInfo() + $", Damage: {Damage}, Ammo type: {RequiredAmmo}";
}