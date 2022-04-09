using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// This class is for UI elements to be able to access the static PlayerData class.
// There might be a better way, but for now this is what we've got.
public class PlayerDataInterface : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI relationshipText;

    public int GetDayNumber() { return Inventory.GetDay(); }
    public void SetDayNumber(int day) { Inventory.SetDay(day); }
    public void ChangeDayNumber(int change) { Inventory.ChangeDay(change); }

    public int GetMoney() { return Inventory.GetMoney(); }
    public void SetMoney(int day) { Inventory.SetMoney(day); }
    public void ChangeMoney(int change) { Inventory.ChangeMoney(change); }

    public int GetRelationshipScore(string name) { return Inventory.relationshipScores[name]; }
    public void SetRelationshipScore(string name, int score) { Inventory.relationshipScores[name] = score; }
    public void ChangeRelationshipScore(string name, int change) { Inventory.relationshipScores[name] += change; }

    public void ChangeKellyScore(int change) { ChangeRelationshipScore("Kelly", change); }
    public void ChangeEllieScore(int change) { ChangeRelationshipScore("Ellie", change); }

    public void SaveGame() { Inventory.SaveGame(); }
    public void LoadGame() { Inventory.LoadGame(); }

    void Update() {
        relationshipText.SetText("Day: {0}\nKelly: {1}\nEllie: {2}", GetDayNumber(), GetRelationshipScore("Kelly"), GetRelationshipScore("Ellie"));
    }
}
