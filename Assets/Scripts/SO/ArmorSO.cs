﻿using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 0)]
public class ArmorSO : ItemSO
{
    [field: SerializeField] public int Protection { get; protected set; }
    [field: SerializeField] public ArmorType Type { get; protected set; }
    public override Item GetData() => new Armor(Name, Weight, Icon, MaxStack, Protection, Type);
    public override Type GetDataType() =>  typeof(Armor);
}