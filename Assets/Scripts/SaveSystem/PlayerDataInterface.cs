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

    public int GetDayNumber() { return PlayerData.GetDayNumber(); }
    public void SetDayNumber(int day) { PlayerData.SetDayNumber(day); }
    public void ChangeDayNumber(int change) { PlayerData.ChangeDayNumber(change); }

    public int GetRelationshipScore(string name) { return PlayerData.GetRelationshipScore(name); }
    public void SetRelationshipScore(string name, int score) { PlayerData.SetRelationshipScore(name, score); }
    public void ChangeRelationshipScore(string name, int change) { PlayerData.ChangeRelationshipScore(name, change); }

    public void ChangeKellyScore(int change) { PlayerData.ChangeRelationshipScore("Kelly", change); }
    public void ChangeEllieScore(int change) { PlayerData.ChangeRelationshipScore("Ellie", change); }

    public void SaveGame() { PlayerData.SaveGame(); }
    public void LoadGame() { PlayerData.LoadGame(); }

    void Update() {
        relationshipText.SetText("Day: {0}\nKelly: {1}\nEllie: {2}", GetDayNumber(), GetRelationshipScore("Kelly"), GetRelationshipScore("Ellie"));
    }
}
