using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public static class PlayerData
{
    public static Dictionary<string, int> RelationshipScores { get; private set; }
    private static int _day;

    private static string[] names = { "Kelly", "Ellie" }; 

    // Static constructor
    static PlayerData() {

        RelationshipScores = new Dictionary<string, int>();
        foreach (string name in names)
        {
            RelationshipScores.Add(name, 0);
        }
        
    }

    // Getters and setters for day number
    public static int DayNumber
    {
        get { return _day; }
        set { if (value >= 0) _day = value; }
    }
    public static int GetDayNumber() { return DayNumber; }
    public static void SetDayNumber(int day) { DayNumber = day; }
    public static void ChangeDayNumber(int change) { DayNumber += change; }

    // Getters and setters for relationship scores
    [YarnFunction("GetRelationshipScore")]
    public static int GetRelationshipScore(string name) { return RelationshipScores[name]; }
    [YarnCommand("SetRelationshipScore")]
    public static void SetRelationshipScore(string name, int score) { RelationshipScores[name] = score; }
    [YarnCommand("AddRelationshipScore")]
    public static void ChangeRelationshipScore(string name, int change) { RelationshipScores[name] += change; }

    // Save to the save system
    public static void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    // Load from the save system
    public static void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        
        if (data == null) return;

        // Reconstruct the save data
        // 1. Reconstruct RelationshipScores
        RelationshipScores.Clear();
        foreach (DictionaryEntry entry in data.relationshipScores)
        {
            // Add a new pair to the dictionary
            RelationshipScores.Add((string)entry.Key, (int)entry.Value);
        }

        // 2. Reconstruct DayNumber
        DayNumber = data.day;
    }
}
