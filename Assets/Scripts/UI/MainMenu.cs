using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Image blackScreen;
    private AudioClip pop;

    private void Awake()
    {
        // YarnCommands.SaveString("Dwayne", "false");
        pop = Resources.Load<AudioClip>("SFX/Pop");
        if (PlayerPrefs.GetString("MainSave", "") == "")
        {
            GameObject load = GameObject.Find("LoadButton");
            load.GetComponent<Button>().enabled = false;
            load.GetComponent<Image>().color = Color.gray;
        }
    }

    public void NewGame()
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        if (PlayerPrefs.GetString("MainSave", "") == "")
        {
            blackScreen.DOFade(1f, 0.5f).OnComplete( () => StartCoroutine(Click.LoadYarnScene("NewGame")));
        }
        else
        {
            blackScreen.DOFade(1f, 0.5f).OnComplete( () => StartCoroutine(Click.LoadYarnScene("Intro")));
        }
    }

    public void LoadGame()
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        Inventory.LoadGame();
        blackScreen.DOFade(1f, 0.5f).OnComplete( () => { 
            SceneManager.LoadScene("MapScene");
            YarnCommands.PlayMusic("Music/Gameplay");
        });
    }

    public void QuitGame()
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_WEBGL)
            Application.OpenURL("https://olaycolay.itch.io/uc-love");
        #else
            Application.Quit();
        #endif
    }
}
