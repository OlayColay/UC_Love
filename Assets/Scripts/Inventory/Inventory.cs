using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public static class Inventory
{
    public static List<Item> list = new List<Item>();
    public static InventoryScreen inventoryScreen;

    [YarnCommand("AddItem")]
    public static void AddItem(int LAScore, int BScore, int SBScore, int RScore, int IScore, string spritePath, string name)
    {
        list.Add(new Item(LAScore, BScore, SBScore, RScore, IScore, spritePath, name));
    }

    [YarnCommand("GiveItem")]
    public static void GiveItem(string recipientName, string itemName)
    {
        Item givenItem = list.Find(i => i.name == itemName);
        PlayerData.ChangeRelationshipScore(recipientName, givenItem.scores[recipientName]);
        list.Remove(givenItem);
    }

    [YarnCommand("OpenInventory")]
    public static IEnumerator OpenInventory()
    {
        if (!inventoryScreen)
        {
            inventoryScreen = GameObject.Find("InventoryScreen").GetComponent<InventoryScreen>();
        }
        inventoryScreen.enabled = true;

        while (inventoryScreen.selectedItem == null)
        {
            yield return null;
        }
    }

    [YarnFunction("GetItemName")]
    public static string GetItemName(int index)
    {
        if (index >= list.Count)
        {
            return "nothing";
        }
        return list[index].name;
    }

    [YarnFunction("GetItemSprite")]
    public static string GetItemSprite(int index)
    {
        if (index >= list.Count)
        {
            return "nothing";
        }
        return "Gifts/" + list[index].sprite.name;
    }

    [YarnFunction("GetInventoryLength")]
    public static int GetInventoryLength()
    {
        return list.Count;
    }
}
