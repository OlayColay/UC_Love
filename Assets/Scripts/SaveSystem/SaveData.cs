using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // We're using Hashtables (not Dictionaries) because they're serializable

    public Hashtable relationshipScores;
    //public SerializableItem[] inventory;
    public int day;
    public int money;

    // Create a SaveData from the static PlayerData class
    public SaveData()
    {
        // 1. Copy RelationshipScores
        relationshipScores = new Hashtable(Inventory.relationshipScores);
        // 2. TODO: Inventory
        
        // 3. Copy the day number
        day = Inventory.GetDay();
        money = Inventory.GetMoney();
    }
}
