using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using DG.Tweening;

/// <summary> Static Yarn Spinner commands. Commands either return nothing or are coroutines </summary>
public class YarnCommands : MonoBehaviour
{
    [YarnCommand("Wait")]
    public static IEnumerator<WaitForSeconds> Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    [YarnCommand("FadeBackground")]
    public static IEnumerator<WaitForSeconds> FadeBackground(string bgPath, float seconds = 2f)
    {
        float halvedDuration = seconds / 2;
        
        BlackScreen.Instance.Fade(false, halvedDuration);
        yield return new WaitForSeconds(halvedDuration);
        Background.SetBackground(bgPath);
        BlackScreen.Instance.Fade(true, halvedDuration);
        yield return new WaitForSeconds(halvedDuration);
    }

    [YarnCommand("PlayMusic")]
    public static void PlayMusic(string musicPath)
    {
        AudioClip musicClip = Resources.Load<AudioClip>(musicPath);
        if ((MusicPlayer.audioSource.clip = musicClip) == null)
        {
            // Debug.LogError("Couldn't find audio clip in " + introPath);
            return;
        }
        MusicPlayer.audioSource.loop = true;
        MusicPlayer.audioSource.Play();
    }

    [YarnCommand("StopMusic")]
    public static void StopMusic()
    {
        MusicPlayer.audioSource.Stop();
    }

    [YarnCommand("GymMinigame")]
    public static IEnumerator GymMinigame(int liftGainPerPress, int punchTarget, float punchTime, int pushupSpeed, int pushupThreshold)
    {
        var sceneLoad = SceneManager.LoadSceneAsync("GymMinigame", LoadSceneMode.Additive);
        while (!sceneLoad.isDone) // We have to wait for the scene to finish loading before finding objects in it
        {
            yield return null;
        }

        FindObjectOfType<GymMinigameController>().NewGymMinigame(liftGainPerPress, punchTarget, punchTime, pushupSpeed, pushupThreshold);
        while (!GymMinigameController.minigameDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync("GymMinigame");
    }

    [YarnCommand("PlazaMinigame")]
    public static IEnumerator PlazaMinigame(string difficulty)
    {
        BlackScreen.Instance.GetComponent<Image>().DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(1f);
        Camera vnCamera = Camera.main;
        
        var sceneLoad = SceneManager.LoadSceneAsync("PlazaMinigame", LoadSceneMode.Additive);
        while (!sceneLoad.isDone) // We have to wait for the scene to finish loading before finding objects in it
        {
            yield return null;
        }

        vnCamera.GetComponent<AudioListener>().enabled = false; // Prevent double AudioListener warning
        vnCamera.gameObject.SetActive(false);
        Background.Instance.gameObject.SetActive(false);
        BlackScreen.Instance.GetComponent<Image>().DOFade(0f, 1f);

        // Set up minigame here
        FindObjectOfType<MapController>().SetEnemyDifficulty(difficulty); //set enemy parameters like speed, projectile rate
        FindObjectOfType<MapController>().SpawnEnemies(difficulty); //set min & max number of enemies and spawn enemies

        while (!PlayerController.gameOver)
        {
            yield return null;
        }

        BlackScreen.Instance.GetComponent<Image>().DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(1f);

        FindObjectOfType<PlayerController>().DeleteEnemies();
        var unload = SceneManager.UnloadSceneAsync("PlazaMinigame");
        while (!unload.isDone) // We have to wait for the scene to finish unloading
        {
            yield return null;
        }

        vnCamera.gameObject.SetActive(true);
        vnCamera.GetComponent<AudioListener>().enabled = true;
        Background.Instance.gameObject.SetActive(true);
        BlackScreen.Instance.GetComponent<Image>().DOFade(0f, 1f);
        yield return new WaitForSecondsRealtime(1f);
    }

    [YarnCommand("CafeMinigame")]
    public static IEnumerator CafeMinigame()
    {
        BlackScreen.Instance.GetComponent<Image>().DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(1f);
        Camera vnCamera = Camera.main;
        EventSystem es = EventSystem.current;
        es.enabled = false;
        
        var sceneLoad = SceneManager.LoadSceneAsync("CafeMinigame", LoadSceneMode.Additive);
        while (!sceneLoad.isDone) // We have to wait for the scene to finish loading before finding objects in it
        {
            yield return null;
        }

        vnCamera.GetComponent<AudioListener>().enabled = false; // Prevent double AudioListener warning
        vnCamera.gameObject.SetActive(false);
        Background.Instance.gameObject.SetActive(false);
        BlackScreen.Instance.GetComponent<Image>().DOFade(0f, 1f);

        while (!CafeMinigameController.gameFinished)
        {
            yield return null;
        }

        BlackScreen.Instance.GetComponent<Image>().DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(1f);

        var unload = SceneManager.UnloadSceneAsync("CafeMinigame");
        while (!unload.isDone) // We have to wait for the scene to finish unloading
        {
            yield return null;
        }

        vnCamera.gameObject.SetActive(true);
        vnCamera.GetComponent<AudioListener>().enabled = true;
        es.enabled = true;
        Background.Instance.gameObject.SetActive(true);
        BlackScreen.Instance.GetComponent<Image>().DOFade(0f, 1f);
        yield return new WaitForSecondsRealtime(1f);
    }

    [YarnCommand("LoadScene")]
    public static void LoadScene(string scene)
    {
        BlackScreen.Instance.blackScreen.DOFade(1f, 0.5f).OnComplete(() => SceneManager.LoadScene(scene));
        PlayerPrefs.Save();
    }

    [YarnCommand("SaveInt")]
    public static void SaveInt(string name, int variable)
    {
        PlayerPrefs.SetInt(name, variable);
    }

    [YarnCommand("SaveString")]
    public static void SaveString(string name, string variable)
    {
        PlayerPrefs.SetString(name, variable);
    }

    [YarnCommand("Dwayne")]
    public static void Dwayne()
    {
        Cursor.SetCursor(Resources.Load<Texture2D>("Dwayne/Running"), new Vector2(18, 0), CursorMode.Auto);
    }
}
