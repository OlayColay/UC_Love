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

    public void ToggleAutoSave()
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        autoSave = !autoSave;
        PlayerPrefs.SetInt("AutoSave", autoSave ? 1 : 0);
        // Debug.Log("Autosave is now " + autoSave + '\n' + StackTraceUtility.ExtractStackTrace());
    }
}
