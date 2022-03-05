using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlacker : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 currentVelocity = Vector3.zero;

    void Awake()
    {
        //animator?
        rb = GetComponent<Rigidbody2D>();

    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector3.zero, ref currentVelocity, 0.8f);
    }
}
