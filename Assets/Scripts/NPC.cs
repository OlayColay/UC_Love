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

    Image image;
    AudioSource audioSource;
    Transform parent;

    // Awake is called super early
    void Awake()
    {
        image = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        parent = transform.parent;
    }

    [YarnCommand("SetSprite")]
    public void SetSprite(string spritePath)
    {
        image.sprite = Resources.Load<Sprite>(spritePath);
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
        audioSource.clip = Resources.Load<AudioClip>(soundPath);
        audioSource.Play();
    }

    [YarnCommand("Flip")]
    public void Flip(float seconds = 0f)
    {
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -180, 0), seconds);
    }

    [YarnCommand("Wait")]
    public static IEnumerator<WaitForSeconds> Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
