using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class ShopScreen : MonoBehaviour
{
    [SerializeField] GameObject[] itemButtons = new GameObject[6];
    [SerializeField] Image background;
    [SerializeField] Sprite shopBackground;
    public GameObject cancelButton;

    [HideInInspector] public static Item selectedItem = null;
    private static int selectedIndex = 0;

    public static Item[] shopItems = new Item[6];
    public static bool[] keyItems = new bool[6];
    public static int[] costs = new int[6];

    public static ShopScreen instance;

    [HideInInspector] public static bool canceled = false;

    void OnEnable()
    {
        background.sprite = shopBackground;
        for (int i = 0; i < shopItems.Length; i++)
        {
            if (shopItems[i] == null)
            {
                break;
            }

            itemButtons[i].transform.GetChild(0).GetComponent<Text>().text = shopItems[i].name;
            itemButtons[i].GetComponent<Image>().sprite = shopItems[i].sprite;
            itemButtons[i].SetActive(true);
            itemButtons[i].GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log("Selecting " + (i-2));
                BuyItem(shopItems[i-2].name); 
            });
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(itemButtons[0].activeSelf ? itemButtons[0] : null);
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

    public void BuyItem(string itemName)
    {
        Debug.Log("Buying " + itemName);

        selectedIndex = Array.FindIndex<Item>(shopItems, i => i.name == itemName);

        if (keyItems[selectedIndex] && Inventory.keyItemList.Count == 6)
        {
            Debug.Log("Key Item inventory is full!");
            return;
        }
        else if (!keyItems[selectedIndex] && Inventory.list.Count == 6)
        {
            Debug.Log("Item inventory is full!");
            return;
        }

        // If not enough currency, can't buy the item
        if (false /*< costs[selectedIndex]*/)
        {
            Debug.Log("Not enough cash for item!");
            return;
        }

        selectedItem = shopItems[selectedIndex];
        // Reduce player currency here

        if (keyItems[selectedIndex])
        {
            shopItems[selectedIndex] = null;
            Inventory.keyItemList.Add(selectedItem);
        }
        else
        {
            Inventory.list.Add(selectedItem);
        }
    }

    [YarnCommand("AddToShop")]
    public static void AddToShop(int index, int LAScore, int BScore, int SBScore, int RScore, int IScore,
                                 string spritePath, string name, int cost, bool isKeyItem = false)
    {
        if (index < 0 || index >= 6)
        {
            Debug.LogError("Index of item must be between 0 and 5 inclusive!");
            return;
        }

        shopItems[index] = new Item(LAScore, BScore, SBScore, RScore, IScore, spritePath, name);
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
