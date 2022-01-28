using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLocation : MonoBehaviour
{

    private Text textField;

    void Start()
    {
        // Initialize
        textField = GetComponent<Text>();

        // Subscribe to the event system
        MapEvents.current.onLocationSelected += Display;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Display(GameObject location)
    {
        // Change the text field to show what was clicked on
        textField.text = location.name;
    }
}
