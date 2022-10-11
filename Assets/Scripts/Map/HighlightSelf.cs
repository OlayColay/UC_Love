using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightSelf : MonoBehaviour, IPointerEnterHandler
{
    public GameObject externalCameraTarget;

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
        cameraControl.PanCamera(externalCameraTarget != null ? externalCameraTarget : gameObject);
        display.Display(transform.parent.name);
    }
}
