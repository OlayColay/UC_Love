using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // We're using arrays because they're serializable
    public int[] relationshipScores;
    public SerializableItem[] inventory;
    public SerializableItem[] keyInventory;
    public int day;
    public int time;
    public int money;

    // Create a SaveData from the static PlayerData class
    public void CopyFromGame()
    {
        // 1. Copy RelationshipScores
        relationshipScores = new int[6];
        relationshipScores[0] = Inventory.relationshipScores["Ellie"];
        relationshipScores[1] = Inventory.relationshipScores["Kelly"];
        relationshipScores[2] = Inventory.relationshipScores["Santana"];
        relationshipScores[3] = Inventory.relationshipScores["Riviera"];
        relationshipScores[4] = Inventory.relationshipScores["Irene"];
        relationshipScores[5] = Inventory.relationshipScores["Tommy"];

        // 2. Inventory
        inventory = new SerializableItem[Inventory.list.Count];
        for (int i = 0; i < Inventory.list.Count; i++)
        {
            // Debug.Log(Inventory.list[i].serialForm.name);
            inventory[i] = Inventory.list[i].serialForm;
        }
        keyInventory = new SerializableItem[Inventory.keyItemList.Count];
        for (int i = 0; i < Inventory.keyItemList.Count; i++)
        {
            // Debug.Log(Inventory.keyItemList[i].serialForm.name);
            keyInventory[i] = Inventory.keyItemList[i].serialForm;
        }
        
        // 3. Copy the day number
        day = Inventory.GetDay();
        time = Inventory.GetTimeOfDay();
        money = Inventory.GetMoney();
    }
}
