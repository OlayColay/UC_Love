using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public static class Inventory
{
    public static List<Item> list = new List<Item>();
    public static List<Item> keyItemList = new List<Item>();
    public static InventoryScreen inventoryScreen;

    public static readonly string[] names = { "Kelly", "Ellie", "Santana", "Riviera", "Irene", "Tommy", "Stan" };
    public static Dictionary<string, int> relationshipScores = new Dictionary<string, int>();
    private static int day = 1;
    private static int time = 0;    // 0 for morning, 1 for afternoon, 2 for night
    private static int money = 100;

    
    static Inventory()
    {
        foreach (string name in names)
            relationshipScores.Add(name, 0);
    }

    [YarnFunction("GetDay")]
    public static int GetDay() { return day; }
    [YarnCommand("SetDay")]
    public static bool SetDay(int Day)
    {
        if (Day < 0)
            return false;
        day = Day;
        return true;
    }
    public static bool ChangeDay(int change) { return SetDay(GetDay() + change); }
    
    [YarnFunction("GetTimeOfDay")]
    public static int GetTimeOfDay() { return time; }
    [YarnCommand("UpdateTimeOfDay")]
    public static void UpdateTimeOfDay()
    {
        if (time == 2)
        {
            day++;
            time = 0;
        }
        else
        {
            time++;
        }
    }

    [YarnFunction("GetMoney")]
    public static int GetMoney() { return money; }
    [YarnCommand("SetMoney")]
    public static void SetMoney(int Money)
    {
        money = Mathf.Max(Money, 0);
    }
    [YarnCommand("AddMoney")]
    public static void ChangeMoney(int change) { SetMoney(GetMoney() + change); }

    // Relationship scores
    [YarnFunction("GetRelationshipScore")]
    public static int GetRelationshipScore(string name) { return relationshipScores[name]; }
    [YarnCommand("SetRelationshipScore")]
    public static void SetRelationshipScore(string name, int score) { relationshipScores[name] = score; }
    [YarnCommand("AddRelationshipScore")]
    public static void ChangeRelationshipScore(string name, int change) { relationshipScores[name] += change; }

    [YarnCommand("CleanSave")]
    public static void CleanSave()
    {
        relationshipScores.Clear();
        foreach (string name in names)
            relationshipScores.Add(name, 0);

        money = 100;
        day = 0;
        time = 0;

        list.Clear();
        keyItemList.Clear();
    }

    // Inventory

    [YarnCommand("AddItem")]
    public static void AddItem(int LAScore, int BScore, int SBScore, int RScore, int IScore, int USCScore, string spritePath, string name, bool isKeyItem = false)
    {
        if (isKeyItem)
        {
            if (keyItemList.Count < 6)
            {
                keyItemList.Add(new Item(LAScore, BScore, SBScore, RScore, IScore, USCScore, spritePath, name));
            }
        }
        else
        {
            if (list.Count < 6)
            {
                list.Add(new Item(LAScore, BScore, SBScore, RScore, IScore, USCScore, spritePath, name));
            }
        }
    }

    [YarnCommand("GiveItem")]
    public static void GiveItem(string recipientName, string itemName, bool isKeyItem = false)
    {
        Item givenItem;
        if (isKeyItem)
        {
            givenItem = keyItemList.Find(i => i.name == itemName);
            keyItemList.Remove(givenItem);
        }
        else
        {
            givenItem = list.Find(i => i.name == itemName);
            list.Remove(givenItem);
        }

        if (givenItem == null)
        {
            // Debug.LogError("Couldn't find item in list! Are you looking for a key item?");
            return;
        }
        
        ChangeRelationshipScore(recipientName, givenItem.scores[recipientName]);
    }

    [YarnCommand("OpenInventory")]
    public static IEnumerator OpenInventory(bool cancelable = true)
    {
        if (!inventoryScreen)
        {
            inventoryScreen = Resources.FindObjectsOfTypeAll<InventoryScreen>()[0];
        }
        inventoryScreen.cancelButton.SetActive(cancelable);
        inventoryScreen.gameObject.SetActive(true);

        InventoryScreen.selectedItem = null;
        InventoryScreen.canceled = false;
        while (InventoryScreen.selectedItem == null && !InventoryScreen.canceled)
        {
            yield return null;
        }
        inventoryScreen.gameObject.SetActive(false);
    }
    public static void OpenInventoryFromMap()
    {
        if (!inventoryScreen)
        {
            inventoryScreen = Resources.FindObjectsOfTypeAll<InventoryScreen>()[0];
        }
        inventoryScreen.cancelButton.SetActive(true);
        inventoryScreen.gameObject.SetActive(true);
    }

    [YarnFunction("GetItemName")]
    public static string GetItemName(int index, bool isKeyItem)
    {
        if (isKeyItem)
        {
            if (index >= keyItemList.Count || index < 0)
            {
                return "nothing";
            }
            return keyItemList[index].name;
        }

        if (index >= list.Count || index < 0)
        {
            return "nothing";
        }
        return list[index].name;
    }

    [YarnFunction("GetItemSprite")]
    public static string GetItemSprite(string itemName, bool isKeyItem)
    {
        Item item;
        if (isKeyItem)
        {
            item = keyItemList.Find(i => i.name == itemName);
            if (item != null)
                return "Gifts/" + item.sprite.name;
            // Debug.LogError("Item with sprite not found! Make sure that you check for its existence in the inventory before allowing the option to use it to be chosen");
            return "nothing";
        }
        item = list.Find(i => i.name == itemName);
        if (item != null)
            return "Gifts/" + item.sprite.name;
        // Debug.LogError("Item with sprite not found! Make sure that you check for its existence in the inventory before allowing the option to use it to be chosen");
        return "nothing";
    }

    [YarnFunction("GetInventoryLength")]
    public static int GetInventoryLength(bool isKeyItems)
    {
        return isKeyItems ? keyItemList.Count : list.Count;
    }

    [YarnFunction("GetInventoryCanceled")]
    public static bool GetInventoryCanceled()
    {
        return InventoryScreen.canceled;
    }

    [YarnFunction("ItemExists")]
    public static bool ItemExists(string itemName, bool isKeyItem)
    {
        return isKeyItem ? (keyItemList.Find(i => i.name == itemName) != null ? true : false) : (list.Find(i => i.name == itemName) != null ? true : false);
    }

    [YarnFunction("GetItemScore")]
    public static int GetItemScore(string recipientName)
    {
        // Debug.Log(InventoryScreen.selectedItem.scores[recipientName]);
        return InventoryScreen.selectedItem.scores[recipientName];
    }

    // Save system stuff

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

        // 1. Reconstruct relationship scores
        Inventory.relationshipScores["Ellie"] = data.relationshipScores[0];
        Inventory.relationshipScores["Kelly"] = data.relationshipScores[1];
        Inventory.relationshipScores["Santana"] = data.relationshipScores[2];
        Inventory.relationshipScores["Riviera"] = data.relationshipScores[3];
        Inventory.relationshipScores["Irene"] = data.relationshipScores[4];
        Inventory.relationshipScores["Tommy"] = data.relationshipScores[5];

        // 2. Reconstruct day
        day = data.day;
        time = data.time;
        money = data.money;

        // 3. Reconstruct inventory
        Inventory.list.Clear();
        foreach (SerializableItem item in data.inventory)
        {
            Inventory.list.Add(item.asItem());
        }
        Inventory.keyItemList.Clear();
        foreach (SerializableItem keyItem in data.keyInventory)
        {
            Inventory.keyItemList.Add(keyItem.asItem());
        }
    }
}
