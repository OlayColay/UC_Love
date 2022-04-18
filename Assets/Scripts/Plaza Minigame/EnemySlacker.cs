using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlacker : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private Vector3 currentVelocity = Vector3.zero;
    [SerializeField] private float startTimeBetweenMove = 0.2f;
    private float timeBetweenMove;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
        timeBetweenMove = startTimeBetweenMove + Random.Range(0, 3.0f);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeBetweenMove <= 0) {
                // animator.SetTrigger("move");
 
                Vector3 targetVelocity = (Random.insideUnitSphere) * 30;
                if(targetVelocity.x >= 0)
                    GetComponent<SpriteRenderer>().flipX = true;
                else
                    GetComponent<SpriteRenderer>().flipX = false;

                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, 0.3f);
                timeBetweenMove = startTimeBetweenMove + Random.Range(0, 3.0f);
            }

            else {
                timeBetweenMove -= Time.deltaTime;
            }
        
        rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector3.zero, ref currentVelocity, 0.6f);
    }
}
