using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private float speed;

    private Transform playerPos;
    private Vector2 targetDirection; 
    private Rigidbody2D rb;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        targetDirection = (playerPos.position - transform.position).normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = targetDirection * speed;

        //raycasting method
        // print(targetDirection);
        // Debug.Log(transform.TransformDirection(Vector3.forward));
        // Debug.DrawRay(transform.position, targetDirection);
        // if (Physics2D.Raycast(transform.position, targetDirection, 1))
        //             Destroy(gameObject);
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().health--;
            // Debug.Log("hit");
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
