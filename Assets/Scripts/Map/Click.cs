using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using DG.Tweening;

public class Click : MonoBehaviour
{
    [SerializeField]
    private LayerMask selectableLayer; // The layer that can be selected
    [SerializeField]
    private Image blackScreen;

    private AudioClip pop;
    private GameObject selectedLocation; // The currently selected location

    void Awake()
    {
        pop = Resources.Load<AudioClip>("SFX/Pop");
    }

    public void LoadSceneWrapper(string sceneName)
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        FindObjectOfType<EventSystem>().enabled = false;
        blackScreen.DOFade(1f, 0.5f).OnComplete( () => StartCoroutine(LoadYarnScene(sceneName)));
    }

    public static IEnumerator LoadYarnScene(string sceneName)
    {
        if (EventSystem.current) EventSystem.current.enabled = false;
        // FindObjectOfType<AudioListener>().enabled = false;

        AsyncOperation async = SceneManager.LoadSceneAsync("VisualNovel", LoadSceneMode.Additive);

        // Wait until scene finishes loading
        while (!async.isDone)
        {
            yield return null;
        }

        FindObjectOfType<DialogueRunner>().StartDialogue(sceneName);
        SceneManager.UnloadSceneAsync((sceneName == "Intro" || sceneName == "NewGame") ? "UiScene" : "MapScene");
    }
}
