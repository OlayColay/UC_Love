using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    [SerializeField] GameObject pauseMenuUI;

    public void TogglePauseGame()
    {
        if (GameIsPaused)
        {
            Debug.Log("Resuming game");
            Resume();
        }
        else
        {
            Debug.Log("Pausing game");
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        MapEvents.current.GamePaused(false);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        MapEvents.current.GamePaused(true);
    }

    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("UiScene");
    }

    public void Save()
    {
        Debug.Log("Saving...");
        Inventory.SaveGame();
    }
}
