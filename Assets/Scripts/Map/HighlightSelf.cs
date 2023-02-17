using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class HighlightSelf : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject externalCameraTarget;

    CameraControl cameraControl;
    DisplayLocation display;

    void Awake()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();
        display = FindObjectOfType<DisplayLocation>(true);
    }

    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        cameraControl.PanCamera(externalCameraTarget != null ? externalCameraTarget : gameObject);
        display.Display(transform.parent.name);

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
