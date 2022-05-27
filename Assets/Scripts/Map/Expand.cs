using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Expand : MonoBehaviour
{
    private static bool expanded;
    private RectTransform parent;
    [SerializeField] private float movementLengthTween;
    [SerializeField] private float movementLength;
    private AudioClip pop;
    
    private void Awake()
    {
        parent = transform.parent.GetComponent<RectTransform>();
        expanded = (PlayerPrefs.GetInt("Expanded", 1) == 1);
        pop = Resources.Load<AudioClip>("SFX/Pop");
        
        if (!expanded) parent.anchoredPosition = new Vector2(parent.anchoredPosition.x, parent.anchoredPosition.y + movementLength);
    }

    public void Clicked()
    {
        MusicPlayer.audioSource.PlayOneShot(pop);
        expanded = !expanded;
        parent.DOAnchorPosY(parent.anchoredPosition.y - (expanded ? movementLengthTween : -movementLengthTween), 0.5f);
        PlayerPrefs.SetInt("Expanded", expanded ? 1 : 0);
    }
}
