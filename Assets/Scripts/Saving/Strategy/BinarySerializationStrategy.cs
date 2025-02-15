using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinarySerializationStrategy : ISerializationStrategy
{
    private const string FormatFile = ".sav";

    public void SaveFile(Dictionary<string, object> state, string fileName)
    {
        string path = GetPathFromSaveFile(fileName);
#if UNITY_EDITOR
        Debug.Log("Saving to " + path);
#endif

        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    public Dictionary<string, object> LoadFile(string fileName)
    {
        string path = GetPathFromSaveFile(fileName);

        if (File.Exists(path) == false)
            return new Dictionary<string, object>();

        using (FileStream stream = File.Open(path, FileMode.Open))
        {
            return (Dictionary<string, object>) new BinaryFormatter().Deserialize(stream);
        }
    }

    private string GetPathFromSaveFile(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName + FormatFile);
    }

    public void DeleteFile(string fileName)
    {
        File.Delete(fileName);
    }
}