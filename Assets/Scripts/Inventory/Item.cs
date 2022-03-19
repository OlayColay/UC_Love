using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public Dictionary<string,int> scores = new Dictionary<string, int>();
    public Sprite sprite;
    public string name;

    public Item(int LAScore, int BScore, int SBScore, int RScore, int IScore, string spritePath, string name)
    {
        scores.Add("Ellie", LAScore);
        scores.Add("Kelly", BScore);
        scores.Add("UCSB", SBScore);
        scores.Add("UCR", RScore);
        scores.Add("UCI", IScore);

        sprite = Resources.Load<Sprite>(spritePath);

        if (!sprite)
        {
            Debug.LogError("Could not find sprite with path " + spritePath);
        }

        this.name = name;
    }
}
