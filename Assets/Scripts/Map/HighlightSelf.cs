using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HighlightSelf : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    CameraControl cameraControl;
    DisplayLocation display;

    void Awake()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();
        display = FindObjectOfType<DisplayLocation>();
    }

    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer enter " + name);
        OnSelect(eventData);
    }
    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        cameraControl.PanCamera(gameObject);
        display.Display(gameObject);
    }
}
