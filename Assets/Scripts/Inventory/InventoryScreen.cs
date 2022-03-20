using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{
    [SerializeField] GameObject[] itemButtons = new GameObject[9];
    [SerializeField] GameObject itemSwitch;
    [SerializeField] GameObject keyItemSwitch;
    [SerializeField] GameObject background;
    [HideInInspector] public Item selectedItem = null;
    private bool isKeyItems = false;

    void OnEnable()
    {
        if (isKeyItems)
        {
            for (int i = 0; i < Inventory.keyItemList.Count; i++)
            {
                itemButtons[i].transform.GetChild(0).GetComponent<Text>().text = Inventory.keyItemList[i].name;
                itemButtons[i].transform.GetChild(1).GetComponent<Image>().sprite = Inventory.keyItemList[i].sprite;
                itemButtons[i].SetActive(true);
                itemButtons[i].GetComponent<Button>().onClick.AddListener(() => {
                    Debug.Log("Selecting " + (i-1));
                    SelectItem(Inventory.keyItemList[i-1].name); 
                });
            }
        }
        else
        {
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
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(itemButtons[0].activeSelf ? itemButtons[0] : itemSwitch);
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
    }

    public void SelectItem(string itemName)
    {
        Debug.Log("Selected " + itemName);
        selectedItem = Inventory.list.Find(i => i.name == itemName);

        if (selectedItem == null)
        {
            selectedItem = Inventory.keyItemList.Find(i => i.name == itemName);
            Inventory.keyItemList.Remove(selectedItem);
        }

        Inventory.list.Remove(selectedItem);
    }

    public void SwitchInventory(bool isKeyItems)
    {
        OnDisable();
        this.isKeyItems = isKeyItems;
        OnEnable();
    }
}
