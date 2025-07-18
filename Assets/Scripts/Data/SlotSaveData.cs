using System;
using System.IO;

[Serializable]
public class SlotSaveData
{
    public bool isLocked;
    public int count;
    public string itemID;

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(isLocked);
        writer.Write(count);
        writer.Write(itemID ?? "");
    }

    public static SlotSaveData Deserialize(BinaryReader reader)
    {
        return new SlotSaveData
        {
            isLocked = reader.ReadBoolean(),
            count = reader.ReadInt32(),
            itemID = reader.ReadString()
        };
    }
}