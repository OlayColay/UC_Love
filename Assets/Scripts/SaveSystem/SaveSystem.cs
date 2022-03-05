using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        // Define the path
        string path = Path();
        Debug.Log("Saving game to " + path);
        // Create file stream
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();

        // Write the data to the file
        formatter.Serialize(stream, data);

        // Close the stream
        stream.Close();
    }

    public static SaveData LoadGame()
    {
        // Define the path
        string path = Path();
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;

            stream.Close();

            return data;
        } else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }

    }

    private static string Path()
    {
        return Application.persistentDataPath + "/save.uclove";
    }
}
