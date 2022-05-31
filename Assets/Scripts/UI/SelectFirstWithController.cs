using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class SelectFirstWithController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Gamepad.current != null)
        {
            EventSystem.current?.SetSelectedGameObject(gameObject);
        }
    }
}
