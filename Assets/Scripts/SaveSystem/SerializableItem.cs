using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SerializableItem
{
    public Hashtable scores;
    public string spritePath;
    public string name;

    public SerializableItem(Item item)
    {
        scores = new Hashtable(item.scores);
        spritePath = AssetDatabase.GetAssetPath(item.sprite);
        name = item.name;
    }

    public Item asItem()
    {
        return new Item((int)scores["Ellie"], (int)scores["Kelly"], (int)scores["Santana"],
            (int)scores["Riviera"], (int)scores["Irene"], (int)scores["Tommy"], spritePath, name);
    }

}
