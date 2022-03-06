using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunner : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float activationRange = 15f;
    [SerializeField] private float smoothTime = 0.1f;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector3 currentVelocity = Vector3.zero;
    private Transform playerPos;
    public bool startCharge = false;
    public bool finishCharge = true;
    public float direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Enemy") && !finishCharge)
        {
            startCharge = false;
            finishCharge = true;
            animator.SetBool("charge", false);

        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //runner
        float vertDistanceToPlayer = Mathf.Abs(transform.position.y - playerPos.position.y);
        float horizDistanceToPlayer = Mathf.Abs(transform.position.x - playerPos.position.x);

        if (finishCharge) {
            if(transform.position.x > playerPos.position.x)
                GetComponent<SpriteRenderer>().flipX = true;
            else
                GetComponent<SpriteRenderer>().flipX = false;

            direction = 0;

            if (!startCharge) {
                if (vertDistanceToPlayer < 0.5 && horizDistanceToPlayer < activationRange) {
                    // touchingObject = false;
                    startCharge = true;
                    finishCharge = false;
                    
                    animator.SetBool("charge", true);

                    if (playerPos.position.x - transform.position.x < 0)
                        direction = -1;
                    else
                        direction = 1;
                }
            }
        }

        if (startCharge) {
            Vector3 targetVelocity = Vector3.zero;
            targetVelocity.x += direction * speed * 2 * Time.deltaTime;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
        }
        else
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector3.zero, ref currentVelocity, smoothTime);
        }
    }
}
