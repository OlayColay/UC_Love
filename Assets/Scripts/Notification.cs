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

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        parentImage = textMesh.transform.parent.GetComponent<Image>();
    }

    [YarnCommand("Notify")]
    public static void Notify(string text, string soundPath)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(Resources.Load<AudioClip>(soundPath));
        DOTween.Clear();

        textMesh.text = text;
        parentImage.DOFade(0f, 0f);
        textMesh.DOFade(0f, 0f);
        parentImage.DOFade(1f, 1f).OnComplete(() => parentImage.DOFade(0f, 1f).SetDelay(2f));
        textMesh.DOFade(1f, 1f).OnComplete(() => textMesh.DOFade(0f, 1f).SetDelay(2f));
    }
}
