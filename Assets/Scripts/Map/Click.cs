using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Click : MonoBehaviour
{
    [SerializeField]
    private LayerMask clickableLayer; // The layer that can be clicked

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            GetElementsClicked();
        }
    }

    void GetElementsClicked()
    {
        // Get the location of the click
        Vector3 clickLocation = Camera.main.ScreenToWorldPoint(new Vector3(
            Mouse.current.position.x.ReadValue(),
            Mouse.current.position.y.ReadValue(),
            Camera.main.nearClipPlane
        ));

        // Debug.Log(string.Format("Mouse position x={0} y={1} z={2}",
        //     clickLocation.x,
        //     clickLocation.y,
        //     clickLocation.z
        //     ));

        // Fire a raycast in place
        RaycastHit2D clickResult = Physics2D.Raycast(new Vector2(clickLocation.x, clickLocation.y), Vector2.zero, 0, clickableLayer);

        // If we found something, select it
        if (clickResult.transform != null)
        {
            GameObject obj = clickResult.transform.gameObject;
            // Debug.Log(obj.name);
            MapEvents.current.LocationSelected(obj);
        }
        else
        {
            // Debug.Log("No game objects found");
        }
    }
}
