using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{

    void Start()
    {
        //Destroy(gameObject, 3);
    }

    void Update()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
            Destroy(gameObject);
    }
}
