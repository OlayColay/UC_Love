using System.Collections;

[System.Serializable]
public class SerializableItem
{
    public int[] scores = new int[6];
    public string spritePath;
    public string name;

    public SerializableItem(Item item)
    {
        scores[0] = item.scores["Ellie"];
        scores[1] = item.scores["Kelly"];
        scores[2] = item.scores["Santana"];
        scores[3] = item.scores["Riviera"];
        scores[4] = item.scores["Irene"];
        scores[5] = item.scores["Tommy"];

        spritePath = item.spritePath;
        name = item.name;
    }

    public Item asItem()
    {
        return new Item(scores[0], scores[1], scores[2], scores[3], scores[4], scores[5], spritePath, name);
    }

}
