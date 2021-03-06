using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 target;

    [SerializeField]
    private float panningOffset;
    [SerializeField]
    private float panningSpeed;

    
    void Start()
    {
        origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        target = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        // Smoothly pan the camera towards the target
        transform.position = Vector3.Lerp(transform.position, target, panningSpeed);
    }

    public void PanCamera(GameObject location)
    {
        // Adjust the target based on the new location's transform
        Vector3 pos = location.transform.position;
        target.x = (pos.x - origin.x) * panningOffset + origin.x;
        target.y = (pos.y - origin.y) * panningOffset + origin.y;
    }
}
