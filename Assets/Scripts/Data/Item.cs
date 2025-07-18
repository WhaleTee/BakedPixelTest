using System;
using UnityEngine;

public enum ItemType { Ammo, Weapon, Armor }
public enum AmmoType { Pistol, Rifle }
public enum WeaponType { Pistol, Rifle }
public enum ArmorType { Torso, Head }


public abstract class Item
{
    public string Name { get; protected set; }
    public float Weight { get; protected set; }
    public Sprite Icon { get; protected set; }
    public int MaxStack { get; protected set; } = 1;

    protected Item(string name, float weight, Sprite icon, int maxStack)
    {
        Name = name;
        Weight = weight;
        Icon = icon;
        MaxStack = maxStack;
    }

    public virtual string GetInfo() => $"{Name}";
}