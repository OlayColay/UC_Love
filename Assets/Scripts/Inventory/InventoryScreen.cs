using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{
    [SerializeField] GameObject[] itemButtons = new GameObject[6];
    [SerializeField] GameObject itemSwitch;
    [SerializeField] GameObject keyItemSwitch;
    [SerializeField] Image background;
    [SerializeField] Sprite itemsBackground;
    [SerializeField] Sprite keyItemsBackground;
    public GameObject cancelButton;
    [HideInInspector] public static Item selectedItem = null;
    [HideInInspector] public static bool canceled = false;
    private bool isKeyItems = false;
    private AudioClip pop;

    void OnEnable()
    {
        pop = Resources.Load<AudioClip>("SFX/Pop");
        
        if (isKeyItems)
        {
            background.sprite = keyItemsBackground;
            for (int i = 0; i < Inventory.keyItemList.Count; i++)
            {
                itemButtons[i].transform.GetChild(0).GetComponent<Text>().text = Inventory.keyItemList[i].name;
                itemButtons[i].GetComponent<Image>().sprite = Inventory.keyItemList[i].sprite;
                itemButtons[i].SetActive(true);
                int index = i; // Needed because the listener doesn't remember what i is, only what i is after the end of the for loop
                itemButtons[i].GetComponent<Button>().onClick.AddListener(() => {
                    // Debug.Log("Selecting " + index);
                    SelectItem(Inventory.keyItemList[index].name); 
                });
            }
        }
        else
        {
            background.sprite = itemsBackground;
            for (int i = 0; i < Inventory.list.Count; i++)
            {
                itemButtons[i].transform.GetChild(0).GetComponent<Text>().text = Inventory.list[i].name;
                itemButtons[i].GetComponent<Image>().sprite = Inventory.list[i].sprite;
                itemButtons[i].SetActive(true);
                int index = i; // Needed because the listener doesn't remember what i is, only what i is after the end of the for loop
                itemButtons[i].GetComponent<Button>().onClick.AddListener(() => {
                    // Debug.Log("Selecting " + index);
                    SelectItem(Inventory.list[index].name); 
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
            item.GetComponent<Image>().sprite = null;
            item.GetComponent<Button>().onClick.RemoveAllListeners();
            item.SetActive(false);
        }
    }

    public void SelectItem(string itemName)
    {
        // Debug.Log("Selected " + itemName);
        selectedItem = Inventory.list.Find(i => i.name == itemName);

        if (selectedItem == null)
        {
            selectedItem = Inventory.keyItemList.Find(i => i.name == itemName);
            Inventory.keyItemList.Remove(selectedItem);
        }

        MusicPlayer.audioSource.PlayOneShot(pop);
        Inventory.list.Remove(selectedItem);
    }

    public void SwitchInventory(bool isKeyItems)
    {
        OnDisable();
        this.isKeyItems = isKeyItems;
        OnEnable();
    }

    public static void Cancel()
    {
        MusicPlayer.audioSource.PlayOneShot(Resources.Load<AudioClip>("SFX/Pop"));
        canceled = true;
    }
}
