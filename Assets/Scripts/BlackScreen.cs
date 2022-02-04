using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

[RequireComponent(typeof(Image))]
public class BlackScreen : MonoBehaviour
{
    private static BlackScreen _instance;
    public static BlackScreen Instance { get { return _instance; } }

    private Image blackScreen;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
            blackScreen = GetComponent<Image>();
        }
    }

    public void Fade(bool fadeIn, float seconds = 1f)
    {
        StartCoroutine(FadeHelper(fadeIn, seconds));
    }
    [YarnCommand("Fade")]
    public static IEnumerator<WaitForSeconds> FadeHelper(bool fadeIn, float seconds = 1f)
    {
        _instance.blackScreen.DOFade(fadeIn ? 0f : 1f, seconds);
        yield return new WaitForSeconds(seconds);
    }
}
