using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

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

        background = parent.parent.Find("Background").GetComponent<Image>();
        blackScreen = parent.parent.Find("Black Screen").GetComponent<Image>();
    }

    [YarnCommand("SetSprite")]
    public void SetSprite(string spritePath)
    {
        if ((image.sprite = Resources.Load<Sprite>(spritePath)) == null)
        {
            Debug.LogError("Couldn't find character sprite in " + spritePath);
            return;
        }
        image.enabled = true;
    }
    
    [YarnCommand("ClearSprite")]
    public void ClearSprite()
    {
        image.enabled = false;
        image.sprite = null;
    }

    [YarnCommand("SetActiveChar")]
    public void SetActiveChar()
    {
        transform.SetAsLastSibling();
        image.DOColor(Color.white, 0.5f);
        parent.GetChild(0).GetComponent<Image>().DOColor(inactiveColor, 0.5f);
        parent.GetChild(1).GetComponent<Image>().DOColor(inactiveColor, 0.5f);
    }

    [YarnCommand("PlaySound")]
    public void PlaySound(string soundPath)
    {
        if ((audioSource.clip = Resources.Load<AudioClip>(soundPath)) == null)
        {
            Debug.LogError("Couldn't find audio clip in " + soundPath);
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
}
