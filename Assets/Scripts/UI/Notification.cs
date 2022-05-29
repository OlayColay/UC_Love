using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Yarn.Unity;

public class Notification : MonoBehaviour
{
    static TextMeshProUGUI textMesh;
    static AudioSource audioSource;
    static Image parentImage;
    public static Notification instance;

    void Awake()
    {
        if (instance)
        {
            Destroy(instance);
        }

        instance = this;
        textMesh = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        parentImage = textMesh.transform.parent.GetComponent<Image>();
    }

    public void Wrapper(string text, string soundPath)
    {
        StartCoroutine(Notify(text, soundPath));
    }

    [YarnCommand("Notify")]
    public static IEnumerator Notify(string text, string soundPath)
    {
        audioSource.Stop();
        if (soundPath != "") audioSource.PlayOneShot(Resources.Load<AudioClip>(soundPath));
        DOTween.Clear();
        DOTween.defaultTimeScaleIndependent = true;

        textMesh.text = text;
        parentImage.DOFade(0f, 0f);
        textMesh.DOFade(0f, 0f);
        parentImage.DOFade(1f, 1f);
        textMesh.DOFade(1f, 1f);

        yield return new WaitForSecondsRealtime(1f);
        
        textMesh.DOFade(0f, 1f).SetDelay(2f);
        parentImage.DOFade(0f, 1f).SetDelay(2f);
        DOTween.defaultTimeScaleIndependent = false;
    }
}
