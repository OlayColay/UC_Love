using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class MapTime : MonoBehaviour
{
    public Sprite[] timeSprites;
    public Transform locationsParent;

    void Awake()
    {
        int timeOfDay = Inventory.GetTimeOfDay();

        GetComponent<SpriteRenderer>().sprite = timeSprites[timeOfDay];

        Color locationColor = (timeOfDay == 2) ? new Color(0.75f, 0.75f, 0.75f, 1f) : Color.white;

        for(int i = 0; i < locationsParent.childCount; i++)
        {
            locationsParent.GetChild(i).GetComponent<Image>().color = locationColor;
        }
    }
}
