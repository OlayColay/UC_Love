using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;

public class ShopScreen : MonoBehaviour
{
    [SerializeField] GameObject[] itemButtons = new GameObject[6];
    [SerializeField] Image background;
    [SerializeField] Sprite shopBackground;
    [SerializeField] TextMeshProUGUI cash;
    public GameObject cancelButton;

    [HideInInspector] public static Item selectedItem = null;

    public static Item[] shopItems = new Item[6];
    public static bool[] keyItems = new bool[6];
    public static int[] costs = new int[6];

    public static ShopScreen instance;

    [HideInInspector] public static bool canceled = false;

    void OnEnable()
    {
        background.sprite = shopBackground;
        cash.text = '$' + Inventory.GetMoney().ToString();
        for (int i = 0; i < shopItems.Length; i++)
        {
            if (shopItems[i] == null)
            {
                break;
            }

            // Debug.Log("Setting " + itemButtons[i].name + " as item " + i);

            itemButtons[i].transform.GetChild(0).GetComponent<Text>().text = shopItems[i].name;
            itemButtons[i].transform.GetChild(1).GetComponent<Text>().text = '$' + costs[i].ToString();
            itemButtons[i].GetComponent<Image>().sprite = shopItems[i].sprite;
            // Only enable button if this isn't a key item or we don't have this key item yet
            itemButtons[i].SetActive(!(keyItems[i] && Inventory.keyItemList.Find(j => j.name == shopItems[i].name) != null)); 

            int index = i; // Needed because the listener doesn't remember what i is, only what i is after the end of the for loop
            itemButtons[i].GetComponent<Button>().onClick.AddListener(() => {
                // Debug.Log("Selecting " + index);
                BuyItem(index); 
            });
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    void OnDisable()
    {
        foreach (GameObject item in itemButtons)
        {
            item.transform.GetChild(0).GetComponent<Text>().text = "ItemName";
            item.GetComponent<Image>().sprite = null;
            item.GetComponent<Button>().onClick.RemoveAllListeners();
            item.SetActive(false);
        }
    }

    public void BuyItem(int index)
    {
        // Debug.Log("Buying " + shopItems[index].name);

        if (keyItems[index] && Inventory.keyItemList.Count == 6)
        {
            Notification.Notify("Special Item inventory is full!", "SFX/Error");
            return;
        }
        else if (!keyItems[index] && Inventory.list.Count == 6)
        {
            Notification.Notify("Item inventory is full!", "SFX/Error");
            return;
        }

        // If not enough currency, can't buy the item
        if (Inventory.GetMoney() < costs[index])
        {
            Notification.Notify("Not enough cash for " + shopItems[index].name + '!', "SFX/Error");
            return;
        }

        selectedItem = shopItems[index];
        Inventory.ChangeMoney(-costs[index]);
        Notification.Notify(selectedItem.name + " purchased!", "SFX/Buy");
        cash.text = '$' + Inventory.GetMoney().ToString();

        if (keyItems[index])
        {
            shopItems[index] = null;
            itemButtons[index].SetActive(false);
            Inventory.keyItemList.Add(selectedItem);
        }
        else
        {
            Inventory.list.Add(selectedItem);
        }
    }

    [YarnCommand("AddToShop")]
    public static void AddToShop(int index, int LAScore, int BScore, int SBScore, int RScore, int IScore, int USCScore,
                                 string spritePath, string name, int cost, bool isKeyItem = false)
    {
        if (index < 0 || index >= 6)
        {
            // Debug.LogError("Index of item must be between 0 and 5 inclusive!");
            return;
        }

        // Prevent adding duplicate items to the shop
        if (shopItems[index] != null && shopItems[index].name == name)
        {
            return;
        }

        shopItems[index] = new Item(LAScore, BScore, SBScore, RScore, IScore, USCScore, spritePath, name);
        keyItems[index] = isKeyItem;
        costs[index] = cost;
    }

    [YarnCommand("OpenShop")]
    public static IEnumerator OpenShop(bool cancelable = true)
    {
        if (!instance)
        {
            instance = Resources.FindObjectsOfTypeAll<ShopScreen>()[0];
        }
        instance.cancelButton.SetActive(cancelable);
        instance.gameObject.SetActive(true);

        canceled = false;
        while (!canceled)
        {
            yield return null;
        }
        instance.gameObject.SetActive(false);
    }

    public static void Cancel()
    {
        canceled = true;
    }
}
