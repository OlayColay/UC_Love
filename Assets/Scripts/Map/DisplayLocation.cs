using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Text))]
public class DisplayLocation : MonoBehaviour
{
    [HideInInspector] public Text textField;
    [HideInInspector] public Image bgImage;
    [HideInInspector] public Sequence textTweenSeq;
    [HideInInspector] public Sequence imgTweenSeq;
    private Color color;

    private void Awake()
    {
        // Initialize
        textField = GetComponent<Text>();
        bgImage = transform.parent.GetComponent<Image>();
        color = new Color(textField.color.r, textField.color.g, textField.color.b, 0f);
        textTweenSeq = DOTween.Sequence();
        imgTweenSeq = DOTween.Sequence();
        Debug.Log(bgImage.name);
    }

    public void Display(string text)
    {
        // Change the text field to show what was clicked on
        textField.text = text;
    }
}
