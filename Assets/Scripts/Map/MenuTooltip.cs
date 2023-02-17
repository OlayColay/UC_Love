using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string displayName;

    DisplayLocation display;

    void Awake()
    {
        display = FindObjectOfType<DisplayLocation>(true);
    }

    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        display.Display(displayName);

        display.textTweenSeq.Complete(true);
        display.imgTweenSeq.Complete(true);
        display.textTweenSeq.Append(display.textField.DOFade(1f, 0.5f)); 
        display.imgTweenSeq.Append(display.bgImage.DOFade(1f, 0.5f)); 
    }

    // When highlighted with mouse.
    public void OnPointerExit(PointerEventData eventData)
    {
        display.textTweenSeq.Complete(true);
        display.imgTweenSeq.Complete(true);
        display.textTweenSeq.Append(display.textField.DOFade(0f, 0.5f));        
        display.imgTweenSeq.Append(display.bgImage.DOFade(0f, 0.5f)); 
    }
}
