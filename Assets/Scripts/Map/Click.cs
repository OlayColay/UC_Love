using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Click : MonoBehaviour
{
    [SerializeField]
    private LayerMask selectableLayer; // The layer that can be selected


    private GameObject selectedLocation; // The currently selected location

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject newLocation = GetElementOverMouse();

        if (newLocation != null)
        {
            // If we ended up selecting something new, then tell the event system
            if (selectedLocation == null || !selectedLocation.Equals(newLocation))
            {
                MapEvents.current.LocationSelected(newLocation);
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                Debug.Log("Travel to " + newLocation.name);
            }

            selectedLocation = newLocation;
        }
    }

    GameObject GetElementOverMouse()
    {
        // Get the location of the click
        Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(new Vector3(
            Mouse.current.position.x.ReadValue(),
            Mouse.current.position.y.ReadValue(),
            Camera.main.nearClipPlane
        ));

        // Debug.Log(string.Format("Mouse position x={0} y={1} z={2}",
        //     mouseLocation.x,
        //     mouseLocation.y,
        //     mouseLocation.z
        //     ));

        // Fire a raycast in place
        RaycastHit2D raycastResult = Physics2D.Raycast(new Vector2(mouseLocation.x, mouseLocation.y), Vector2.zero, 0, selectableLayer);

        // If we found something, select it
        if (raycastResult.transform != null)
        {
            return raycastResult.transform.gameObject;
        }
        return null;
    }
}
