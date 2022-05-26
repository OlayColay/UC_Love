using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Expand : MonoBehaviour
{
    private static bool expanded;
    private Transform parent;
    [SerializeField] private float movementLengthTween;
    [SerializeField] private float movementLength;
    
    private void Awake()
    {
        parent = transform.parent;
        expanded = (PlayerPrefs.GetInt("Expanded", 1) == 1);
        
        if (!expanded) parent.position = new Vector2(parent.position.x, parent.position.y + movementLength);
    }

    public void Clicked()
    {
        expanded = !expanded;
        parent.DOMoveY(parent.position.y - (expanded ? movementLengthTween : -movementLengthTween), 0.5f);
        PlayerPrefs.SetInt("Expanded", expanded ? 1 : 0);
    }
}
