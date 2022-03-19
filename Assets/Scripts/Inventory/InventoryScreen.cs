using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{
    [SerializeField] GameObject[] itemButtons = new GameObject[9];
    [HideInInspector] public Item selectedItem = null;

    void OnEnable()
    {
        selectedItem = null;
        GetComponent<Image>().enabled = true;

        for (int i = 0; i < Inventory.list.Count; i++)
        {
            itemButtons[i].transform.GetChild(0).GetComponent<Text>().text = Inventory.list[i].name;
            itemButtons[i].transform.GetChild(1).GetComponent<Image>().sprite = Inventory.list[i].sprite;
            itemButtons[i].SetActive(true);
            itemButtons[i].GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log("Selecting " + (i-1));
                SelectItem(Inventory.list[i-1].name); 
            });
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(itemButtons[0]);
    }

    void OnDisable()
    {
        foreach (GameObject item in itemButtons)
        {
            item.transform.GetChild(0).GetComponent<Text>().text = "ItemName";
            item.transform.GetChild(1).GetComponent<Image>().sprite = null;
            item.GetComponent<Button>().onClick.RemoveAllListeners();
            item.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
    }

    public void SelectItem(string itemName)
    {
        Debug.Log("Selected " + itemName);
        selectedItem = Inventory.list.Find(i => i.name == itemName);

        gameObject.SetActive(false);
    }
}
