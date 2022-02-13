using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary> Static Yarn Spinner commands. Commands either return nothing or are coroutines </summary>
public class YarnCommands : MonoBehaviour
{
    [YarnCommand("Wait")]
    public static IEnumerator<WaitForSeconds> Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    [YarnCommand("ChangeScene")]
    public static IEnumerator<WaitForSeconds> ChangeScene(string bgPath, float seconds = 2f)
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
}
