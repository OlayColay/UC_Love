using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    [SerializeField] private float speed = 200f;
    [SerializeField] private float activationRange = 10f;
    [SerializeField] private float smoothTime = 0.3f;
    private Rigidbody2D rb;
    private Vector3 currentVelocity = Vector3.zero;
    private Transform playerPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    { 

        //chaser
        // Debug.Log(Vector3.Distance(transform.position, playerPos.position));
        if (Vector3.Distance(transform.position, playerPos.position) < activationRange)
        {
            Vector3 targetVelocity = (playerPos.position - transform.position).normalized * speed * Time.deltaTime;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
            //Vector2.MoveTowards(transform.position, playerPos.position, speed );
        }
        else 
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector3.zero, ref currentVelocity, smoothTime);
        }
    }

    public void SetDifficulty(string difficulty)
    {
        // Debug.Log("chaser " + difficulty);

        if (difficulty == "easy")
        {
            speed = 150f;
            activationRange = 8f;
        }
        else if (difficulty == "medium")
        {
            speed = 200f;
            activationRange = 10f;
        }
        else if (difficulty == "hard")
        {
            speed = 270f;
            activationRange = 12f;
        }
    }
}
