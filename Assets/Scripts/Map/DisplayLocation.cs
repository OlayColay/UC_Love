using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayLocation : MonoBehaviour
{
    private Text textField;

    private void Start()
    {
        // Initialize
        textField = GetComponent<Text>();
    }

    public void Display(string text)
    {
        // Change the text field to show what was clicked on
        textField.text = text;
    }
}
