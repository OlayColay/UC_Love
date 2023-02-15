using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    [SerializeField] GameObject pauseMenuUI;
    private AudioClip pop;

    public GameObject resumeButton;
    public GameObject menuButton;

    private void Awake()
    {
        pop = Resources.Load<AudioClip>("SFX/Pop");
    }

    public void TogglePauseGame()
    {
        if (GameIsPaused)
        {
            // Debug.Log("Resuming game");
            Resume();
        }
        else
        {
            // Debug.Log("Pausing game");
            Pause();
        }
    }

    public void Resume()
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        pauseMenuUI.SetActive(false);

        // If a gamepad is connected, we'll highlight the menu button when closing the pause menu
        if (Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(menuButton);
        }
    }

    public void Pause()
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        pauseMenuUI.SetActive(true);

        // If a gamepad is connected, we'll highlight the resume button when opening the pause menu
        if (Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
    }

    public void LoadMenu()
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        // Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("UiScene");
    }

    public void Save()
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        // Debug.Log("Saving...");
        Inventory.SaveGame();
        Notification.instance.Wrapper("Game saved!", "");
    }

    public void OpenInventory()
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        Inventory.OpenInventoryFromMap();
    }

    public void CloseInventory()
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        Inventory.inventoryScreen.gameObject.SetActive(false);
    }
}
