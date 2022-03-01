using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

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
    public static void PlayMusic(string soundPath)
    {
        if ((Background.Instance.audioSource.clip = Resources.Load<AudioClip>(soundPath)) == null)
        {
            Debug.LogError("Couldn't find audio clip in " + soundPath);
            return;
        }
        Background.Instance.audioSource.Play();
    }

    [YarnCommand("SetRelationshipScore")]
    public static void SetRelationshipScore(string character, int newScore)
    {
        // TODO: Set relationship score based on enum converted from string
    }

    [YarnCommand("AddRelationshipScore")]
    public static void AddRelationshipScore(string character, int addedScore)
    {
        // TODO: Increment relationship score based on enum converted from string
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

    [YarnCommand("LoadScene")]
    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
