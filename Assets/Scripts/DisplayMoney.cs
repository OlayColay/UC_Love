using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayMoney : MonoBehaviour
{
    private Text textField;

    private void Start()
    {
        // Initialize
        textField = GetComponent<Text>();

        // Get money to text
        textField.text = '$' + Inventory.GetMoney().ToString();
    }
}
