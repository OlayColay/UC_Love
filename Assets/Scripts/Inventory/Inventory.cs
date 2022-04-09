using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public static class Inventory
{
    public static List<Item> list = new List<Item>();
    public static List<Item> keyItemList = new List<Item>();
    public static InventoryScreen inventoryScreen;

    [YarnCommand("AddItem")]
    public static void AddItem(int LAScore, int BScore, int SBScore, int RScore, int IScore, string spritePath, string name, bool isKeyItem = false)
    {
        if (isKeyItem)
        {
            keyItemList.Add(new Item(LAScore, BScore, SBScore, RScore, IScore, spritePath, name));
        }
        else
        {
            list.Add(new Item(LAScore, BScore, SBScore, RScore, IScore, spritePath, name));
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
            Debug.LogError("Couldn't find item in list! Are you looking for a key item?");
            return;
        }
        
        PlayerData.ChangeRelationshipScore(recipientName, givenItem.scores[recipientName]);
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

    [YarnFunction("GetItemName")]
    public static string GetItemName(int index, bool isKeyItem = false)
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
    public static string GetItemSprite(string itemName, bool isKeyItem = false)
    {
        Item item;
        if (isKeyItem)
        {
            item = keyItemList.Find(i => i.name == itemName);
            if (item != null)
                return "Gifts/" + item.sprite.name;
            Debug.LogError("Item with sprite not found! Make sure that you check for its existence in the inventory before allowing the option to use it to be chosen");
            return "nothing";
        }
        item = list.Find(i => i.name == itemName);
        if (item != null)
            return "Gifts/" + item.sprite.name;
        Debug.LogError("Item with sprite not found! Make sure that you check for its existence in the inventory before allowing the option to use it to be chosen");
        return "nothing";
    }

    [YarnFunction("GetInventoryLength")]
    public static int GetInventoryLength(bool isKeyItems = false)
    {
        return isKeyItems ? keyItemList.Count : list.Count;
    }

    [YarnFunction("GetInventoryCanceled")]
    public static bool GetInventoryCanceled()
    {
        return InventoryScreen.canceled;
    }

    [YarnFunction("ItemExists")]
    public static bool ItemExists(string itemName, bool isKeyItem = false)
    {
        return isKeyItem ? (keyItemList.Find(i => i.name == itemName) != null ? true : false) : (list.Find(i => i.name == itemName) != null ? true : false);
    }
}
