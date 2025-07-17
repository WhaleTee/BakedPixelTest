using UnityEngine;

public class Ammo : Item
{
    public AmmoType Type { get; protected set; }

    public Ammo(string name, float weight, Sprite icon, int maxStack, AmmoType type) 
        : base(name, weight, icon, maxStack)
    {
        Type = type;
    }

    public override string GetInfo() => base.GetInfo() + $", Type: {Type}";
}