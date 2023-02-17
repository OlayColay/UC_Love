using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuTooltip : MonoBehaviour, IPointerEnterHandler
{
    public string displayName;

    DisplayLocation display;

    void Awake()
    {
        display = FindObjectOfType<DisplayLocation>();
    }

    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        display.Display(displayName);
    }
}
