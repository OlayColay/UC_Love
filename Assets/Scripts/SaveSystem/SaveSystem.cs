using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveGame()
    {
        SaveData data = new SaveData();

        string jsonData = JsonUtility.ToJson(data);

        PlayerPrefs.SetString("MainSave", jsonData);
    }

    public static SaveData LoadGame()
    {
        return JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("MainSave", ""));
    }

    private static string Path()
    {
        return Application.persistentDataPath + "/save.uclove";
    }
}
