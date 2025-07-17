using UnityEngine;

public class Armor : Item
{
    public int Protection { get; protected set; }
    public ArmorType Type { get; protected set; }

    public Armor(string name, float weight, Sprite icon, int maxStack, int protection, ArmorType type) 
        : base(name, weight, icon, maxStack)
    {
        Protection = protection;
        Type = type;
    }

    public override string GetInfo()
    {
        return base.GetInfo() + $", Type: {Type}, Protection: {Protection}";
    }
}