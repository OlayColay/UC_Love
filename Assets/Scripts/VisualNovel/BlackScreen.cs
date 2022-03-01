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

    public void Fade(bool fadeIn, float seconds = 1f, string colorHex = "#000000")
    {
        StartCoroutine(FadeHelper(fadeIn, seconds, colorHex));
    }
    [YarnCommand("Fade")]
    public static IEnumerator FadeHelper(bool fadeIn, float seconds = 1f, string colorHex = "#000000")
    {
        colorHex += fadeIn ? "ff" : "00";
        Color newColor = Color.black;
        ColorUtility.TryParseHtmlString(colorHex, out newColor);
        _instance.blackScreen.color = newColor;

        _instance.blackScreen.DOFade(fadeIn ? 0f : 1f, seconds);
        yield return new WaitForSeconds(seconds);
    }
}