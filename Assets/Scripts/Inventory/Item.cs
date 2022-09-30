using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public Dictionary<string,int> scores = new Dictionary<string, int>();
    public Sprite sprite;
    public string name;
    public string spritePath;

    [HideInInspector] public SerializableItem serialForm;

    public Item(int LAScore, int BScore, int SBScore, int RScore, int IScore, int USCScore, string spritePath, string name)
    {
        scores.Add("Ellie", LAScore);
        scores.Add("Kelly", BScore);
        scores.Add("Santana", SBScore);
        scores.Add("Riviera", RScore);
        scores.Add("Irene", IScore);
        scores.Add("Tommy", USCScore);
        scores.Add("Stan", -10);

        this.spritePath = spritePath;
        sprite = Resources.Load<Sprite>(spritePath);

        if (!sprite)
        {
            // Debug.LogError("Could not find sprite with path " + spritePath);
        }

        this.name = name;

        serialForm = new SerializableItem(this);
    }
}
