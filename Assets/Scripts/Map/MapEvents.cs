using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEvents : MonoBehaviour
{
    public static MapEvents current;
    
    private void Awake()
    {
        current = this;
    }

    public event Action<GameObject> onLocationSelected;
    public void LocationSelected(GameObject location)
    {
        if (onLocationSelected != null)
        {
            onLocationSelected(location);
        }
    }

    public event Action<bool> onGamePaused;
    public void GamePaused(bool isPaused)
    {
        onGamePaused(isPaused);
    }
}
