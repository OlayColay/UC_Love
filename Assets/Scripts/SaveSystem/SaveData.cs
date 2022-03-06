using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    
    public Hashtable relationshipScores; // We're using a Hashtable (not a Dictionary) because it's serializable
    public int day;

    // Create a SaveData from the static PlayerData class
    public SaveData()
    {
        // 1. Copy the contents of relationshipScores Dictionary
        relationshipScores = new Hashtable();
        foreach (KeyValuePair<string, int> entry in PlayerData.RelationshipScores)
        {
            relationshipScores.Add(entry.Key, entry.Value);
        }

        // 2. Copy the day number
        day = PlayerData.DayNumber;
    }
}
