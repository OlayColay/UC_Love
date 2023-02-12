using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectSelf : MonoBehaviour, ISelectHandler
{
    public GameObject externalCameraTarget;

    CameraControl cameraControl;
    DisplayLocation display;

    void Awake()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();
        display = FindObjectOfType<DisplayLocation>();
    }

    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        cameraControl.PanCamera(externalCameraTarget != null ? externalCameraTarget : transform.GetChild(0).gameObject);
        display.Display(name);
    }
}
