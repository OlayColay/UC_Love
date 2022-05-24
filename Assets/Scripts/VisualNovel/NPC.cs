using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

[RequireComponent(typeof(Image), typeof(AudioSource))]
public class NPC : MonoBehaviour
{
    [SerializeField] Yarn.Program script;
    [SerializeField] Color inactiveColor = Color.gray;

    static Image background;
    static Image blackScreen;
    Image image;
    AudioSource audioSource;
    Transform parent;

    // Awake is called super early
    void Awake()
    {
        image = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        parent = transform.parent;

        background = GameObject.Find("Background").GetComponent<Image>();
        blackScreen = GameObject.Find("Black Screen").GetComponent<Image>();
    }

    [YarnCommand("SetSprite")]
    public IEnumerator<WaitForSeconds> SetSprite(string spritePath, float fadeTime = 0.5f)
    {
        if ((image.sprite = Resources.Load<Sprite>(spritePath)) == null)
        {
            // Debug.LogError("Couldn't find character sprite in " + spritePath);
        }

        if (image.color.a <= float.Epsilon || !image.enabled)
        {
            image.enabled = true;
            image.DOFade(1f, fadeTime);
            yield return new WaitForSeconds(fadeTime);
        }        
    }
    
    [YarnCommand("ClearSprite")]
    public IEnumerator<WaitForSeconds> ClearSprite(float fadeTime = 0.5f)
    {
        image.DOFade(0f, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        image.enabled = false;
        image.sprite = null;
    }

    [YarnCommand("SetActiveChar")]
    public void SetActiveChar()
    {
        transform.SetAsLastSibling();
        image.DOColor(Color.white, 0.5f);
        // First child is the bottom sprite, which will never be inactive
        parent.GetChild(1).GetComponent<Image>().DOColor(new Color(inactiveColor.r, inactiveColor.g, inactiveColor.b, parent.GetChild(1).GetComponent<Image>().color.a), 0.5f);
        parent.GetChild(2).GetComponent<Image>().DOColor(new Color(inactiveColor.r, inactiveColor.g, inactiveColor.b, parent.GetChild(2).GetComponent<Image>().color.a), 0.5f);
    }

    [YarnCommand("PlaySound")]
    public void PlaySound(string soundPath)
    {
        if ((audioSource.clip = Resources.Load<AudioClip>(soundPath)) == null)
        {
            // Debug.LogError("Couldn't find audio clip in " + soundPath);
            return;
        }
        audioSource.Play();
    }

    [YarnCommand("Flip")]
    public void Flip(float seconds = 0f)
    {
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -180, 0), seconds);
    }

    [YarnCommand("Shake")]
    public void Shake(float seconds = 1f, float strength = 10f)
    {
        transform.DOShakePosition(seconds, strength);
    }

    [YarnCommand("SetColor")]
    public void SetColor(float red, float green, float blue, float alpha = 1f)
    {
        image.color = new Color(red, green, blue, alpha);
    }
    [YarnCommand("SetColorHex")]
    public void SetColorHex(string hexCode)
    {
        Color newColor = new Color();
        ColorUtility.TryParseHtmlString(hexCode, out newColor);
        image.color = newColor;
    }
}
