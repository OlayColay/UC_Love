using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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

    private GameObject selectedLocation; // The currently selected location

    private bool isActive = true; // If this script should be active
    private int delayFrames = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the event system
        MapEvents.current.onGamePaused += SetDisabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return; // Don't do anything if the game is paused
        if (delayFrames > 0)
        {
            delayFrames--;
            return;
        }

        GameObject newLocation = GetElementOverMouse();

        if (newLocation != null)
        {
            // If we ended up selecting something new, then tell the event system
            if (selectedLocation == null || !selectedLocation.Equals(newLocation))
            {
                MapEvents.current.LocationSelected(newLocation);
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                Debug.Log("Travel to " + newLocation.name);
                
                // TODO: I'm not well-versed in Yarn, but should we just load the scene separately
                // instead of as a Coroutine?
                // The reason that I use a Coroutine is so that we can wait for the scene to load with a
                // while loop without freezing the game :)
                        
                FindObjectOfType<AudioListener>().enabled = false;
                FindObjectOfType<EventSystem>().enabled = false;
                blackScreen.DOFade(1f, 0.5f).OnComplete( () => StartCoroutine(LoadYarnScene(newLocation.name)));
            }

            selectedLocation = newLocation;
        }
    }

    public void SetActive(bool val) {
        if (!isActive && val) delayFrames = 3; // This prevents the same click from resuming the game AND traveling somewhere simultaneously
        isActive = val;
    }
    public void SetDisabled(bool val) { SetActive(!val); }

    GameObject GetElementOverMouse()
    {
        // Get the location of the click
        Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(new Vector3(
            Mouse.current.position.x.ReadValue(),
            Mouse.current.position.y.ReadValue(),
            Camera.main.nearClipPlane
        ));

        // Debug.Log(string.Format("Mouse position x={0} y={1} z={2}",
        //     mouseLocation.x,
        //     mouseLocation.y,
        //     mouseLocation.z
        //     ));

        // Fire a raycast in place
        RaycastHit2D raycastResult = Physics2D.Raycast(new Vector2(mouseLocation.x, mouseLocation.y), Vector2.zero, 0, selectableLayer);

        // If we found something, select it
        if (raycastResult.transform != null)
        {
            return raycastResult.transform.gameObject;
        }
        return null;
    }

    public static IEnumerator LoadYarnScene(string sceneName)
    {
        EventSystem.current.enabled = false;
        FindObjectOfType<AudioListener>().enabled = false;

        AsyncOperation async = SceneManager.LoadSceneAsync("VisualNovel", LoadSceneMode.Additive);

        // Wait until scene finishes loading
        while (!async.isDone)
        {
            yield return null;
        }

        FindObjectOfType<DialogueRunner>().StartDialogue(sceneName);
        SceneManager.UnloadSceneAsync(sceneName == "Intro" ? "UiScene" : "MapScene");
    }
}
