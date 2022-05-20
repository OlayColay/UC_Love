using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public static void PlayMusic(string introPath, string loopPath)
    {
        AudioClip introClip = Resources.Load<AudioClip>(introPath);
        if ((Background.Instance.audioSource.clip = introClip) == null)
        {
            Debug.LogError("Couldn't find audio clip in " + introPath);
            return;
        }
        Background.Instance.audioSource.loop = false;
        Background.Instance.audioSource.Play();

        AudioClip loopClip = Resources.Load<AudioClip>(loopPath);
        if (loopClip == null)
        {
            return;
        }

        YarnCommands yc = Background.Instance.gameObject.AddComponent<YarnCommands>();
        yc.StartCoroutine(yc.PlayLoop(introClip.length, loopClip));
    }
    private IEnumerator PlayLoop(float introLength, AudioClip loopClip)
    {
        yield return new WaitForSecondsRealtime(introLength);
        Background.Instance.audioSource.clip = loopClip;
        Background.Instance.audioSource.loop = true;
        Background.Instance.audioSource.Play();
        Destroy(Background.Instance.gameObject.GetComponent<YarnCommands>());
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
        vnCamera.GetComponent<AudioListener>().enabled = false; // Prevent double AudioListener warning
        
        var sceneLoad = SceneManager.LoadSceneAsync("PlazaMinigame", LoadSceneMode.Additive);
        while (!sceneLoad.isDone) // We have to wait for the scene to finish loading before finding objects in it
        {
            yield return null;
        }

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

    [YarnCommand("LoadScene")]
    public static void LoadScene(string scene)
    {
        BlackScreen.Instance.blackScreen.DOFade(1f, 0.5f).OnComplete(() => SceneManager.LoadScene(scene));
        PlayerPrefs.Save();
    }

    [YarnCommand("SaveVariable")]
    public static void SaveVariable(string name, dynamic variable)
    {
        if (variable is int)
        {
            PlayerPrefs.SetInt(name, variable);
            Debug.Log("Saved int " + variable + " to " + name);
        }
        else if (variable is float)
        {
            PlayerPrefs.SetFloat(name, variable);
            Debug.Log("Saved float " + variable + " to " + name);
        }
        else if (variable is string)
        {
            PlayerPrefs.SetString(name, variable);
            Debug.Log("Saved string " + variable + " to " + name);
        }
        else
        {
            Debug.LogError("Cannot save an invalid type of variable!");
        }
    }
}
