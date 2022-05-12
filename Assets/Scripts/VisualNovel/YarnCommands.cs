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

    [YarnCommand("LoadScene")]
    public static void LoadScene(string scene)
    {
        GameObject.FindObjectOfType<DialogueRunner>().Stop();
        SceneManager.LoadScene(scene);
    }
}
