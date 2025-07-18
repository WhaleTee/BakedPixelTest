using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class MobileSaveSystem
{
    private static readonly string SAVE_PATH = Path.Combine(Application.persistentDataPath, "inventory.dat");
    private static readonly string BACKUP_PATH = Path.Combine(Application.persistentDataPath, "inventory_backup.dat");
    
    private static readonly BinaryFormatter formatter = new();

    public static void SaveInventory(InventorySaveData data)
    {
        try
        {
            var saveData = Serialize(data);
            
            // Сохраняем резервную копию
            if (File.Exists(SAVE_PATH))
            {
                File.Copy(SAVE_PATH, BACKUP_PATH, true);
            }

            // Сохраняем основной файл
            File.WriteAllBytes(SAVE_PATH, saveData);
            
            Debug.Log($"Inventory saved to: {SAVE_PATH}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e}");
            // Восстановление из резервной копии при ошибке
            RestoreBackup();
        }
    }

    public static InventorySaveData LoadInventory()
    {
        try
        {
            if (File.Exists(SAVE_PATH))
            {
                var data = File.ReadAllBytes(SAVE_PATH);
                return Deserialize<InventorySaveData>(data);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Load failed: {e}");
            RestoreBackup();
            return LoadInventory(); // Повторная попытка после восстановления
        }
        
        Debug.Log("No save file found");
        return null;
    }

    private static byte[] Serialize(object obj)
    {
        using var ms = new MemoryStream();
        formatter.Serialize(ms, obj);
        return ms.ToArray();
    }

    private static T Deserialize<T>(byte[] data)
    {
        using var ms = new MemoryStream(data);
        return (T)formatter.Deserialize(ms);
    }

    private static void RestoreBackup()
    {
        try
        {
            if (File.Exists(BACKUP_PATH))
            {
                File.Copy(BACKUP_PATH, SAVE_PATH, true);
                Debug.Log("Backup restored successfully");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Backup restore failed: {e}");
        }
    }
}