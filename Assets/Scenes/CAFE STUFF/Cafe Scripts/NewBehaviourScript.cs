using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBehaviourScript : MonoBehaviour
{
    public Controls controls;
    public float MovementSpeed = 2;

    private void Start()
    {
        // If this script doesn't have any controls attached to it, attach a new instance of the controls
        if (controls == null) controls = new Controls();
        // Enable controls so the player's inputs do stuff
        controls.Enable();
        // Another option just for the Move part to be enabled:
        // controls.player.Move.Enable();
    }

    private void Update()
    {
        // Move player by reading the Move control value into a variable
        // The x value of the 2D vector is horizontal movement (A and D)
        float movement = controls.Land.Move.ReadValue<Vector2>().x;
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
    }
}
