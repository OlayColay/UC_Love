using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class NPC : MonoBehaviour
{
    [SerializeField] Yarn.Program script;
    [SerializeField] string startingNode = "";

    Image image;
    AudioSource audioSource;

    // Awake is called super early
    void Awake()
    {
        image = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }

    [YarnCommand("SetSprite")]
    public void SetSprite(string spritePath)
    {
        image.sprite = Resources.Load<Sprite>(spritePath);
    }

    [YarnCommand("PlaySound")]
    public void PlaySound(string soundPath)
    {
        audioSource.clip = Resources.Load<AudioClip>(soundPath);
        audioSource.Play();
    }
}
