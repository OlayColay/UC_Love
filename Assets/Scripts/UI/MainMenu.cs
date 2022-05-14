using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        // Inventory.ClearSave();
        SceneManager.LoadScene("MapScene");
    }

    public void LoadGame()
    {
        Inventory.LoadGame();
        SceneManager.LoadScene("MapScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game!!");
        Application.Quit(); // Doesn't work in Unity Editor
    }
}
