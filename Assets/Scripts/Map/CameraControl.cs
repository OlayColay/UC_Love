using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 target;

    [SerializeField]
    private float panningFactor;
    [SerializeField]
    private float posLerp;

    
    void Start()
    {
        origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        target = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        // Debug.Log(string.Format("Origin: x={0} y={1}",
        //     transform.position.x,
        //     transform.position.y
        // ));

        // Subscribe to the event system
        MapEvents.current.onLocationSelected += PanCamera;
    }

    void Update()
    {
        // Smoothly pan the camera towards the target
        transform.position = Vector3.Lerp(transform.position, target, posLerp);
    }

    private void PanCamera(GameObject location)
    {
        // Adjust the target based on the new location's transform
        target.x = (location.transform.position.x - origin.x) * panningFactor + origin.x;
        target.y = (location.transform.position.y - origin.y) * panningFactor + origin.y;
    }
}
