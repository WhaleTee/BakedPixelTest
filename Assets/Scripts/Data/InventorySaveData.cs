using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

[Serializable]
public class InventorySaveData
{
    public int coins;
    public List<SlotSaveData> slots = new();
    
    public byte[] Compress()
    {
        var data = Serialize();
        var output = new MemoryStream();
        using (var stream = new DeflateStream(output, CompressionLevel.Optimal))
        {
            stream.Write(data, 0, data.Length);
        }
        return output.ToArray();
    }
    
    public static InventorySaveData Decompress(byte[] compressedData)
    {
        var input = new MemoryStream(compressedData);
        var output = new MemoryStream();
        using (var stream = new DeflateStream(input, CompressionMode.Decompress))
        {
            stream.CopyTo(output);
        }
        var data = output.ToArray();
        
        return Deserialize(data);
    }
    
    private byte[] Serialize()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        writer.Write(coins);
        writer.Write(slots.Count);
            
        foreach (var slot in slots)
        {
            slot.Serialize(writer);
        }
            
        return ms.ToArray();
    }
    
    private static InventorySaveData Deserialize(byte[] data)
    {
        using var ms = new MemoryStream(data);
        using var reader = new BinaryReader(ms);
        var saveData = new InventorySaveData
        {
            coins = reader.ReadInt32(),
            slots = new List<SlotSaveData>()
        };
            
        var slotCount = reader.ReadInt32();
        for (var i = 0; i < slotCount; i++)
        {
            saveData.slots.Add(SlotSaveData.Deserialize(reader));
        }
            
        return saveData;
    }
}