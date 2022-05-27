using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEvents : MonoBehaviour
{
    public static MapEvents current;
    public static bool autoSave;

    private AudioClip pop;
    
    private void Awake()
    {
        current = this;

        pop = Resources.Load<AudioClip>("SFX/Pop");
        autoSave = (PlayerPrefs.GetInt("AutoSave", 1) == 1);
        FindObjectOfType<Toggle>().SetIsOnWithoutNotify(autoSave);
        if (autoSave)
        {
            // Debug.Log("Autosaving...");
            Inventory.SaveGame();
        } 
    }

    public event Action<GameObject> onLocationSelected;
    public void LocationSelected(GameObject location)
    {
        if (onLocationSelected != null)
        {
            onLocationSelected(location);
        }
    }

    public event Action<bool> onGamePaused;
    public void GamePaused(bool isPaused)
    {
        onGamePaused(isPaused);
    }

    public void ToggleAutoSave()
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        autoSave = !autoSave;
        PlayerPrefs.SetInt("AutoSave", autoSave ? 1 : 0);
        // Debug.Log("Autosave is now " + autoSave + '\n' + StackTraceUtility.ExtractStackTrace());
    }
}
