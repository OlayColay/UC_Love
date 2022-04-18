using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLitterer : MonoBehaviour
{
    [SerializeField] private float activationRange = 14f;
    [SerializeField] private float startTimeBetweenShots = 2f;
    private float timeBetweenShots;

    private Rigidbody2D rb;
    private Animator animator;
    // private SpriteRenderer m_SpriteRenderer;

    private Vector3 currentVelocity = Vector3.zero;
    private Transform playerPos;
    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        timeBetweenShots = startTimeBetweenShots + Random.Range(0, 1.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.x > playerPos.position.x)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;


        if (Vector3.Distance(transform.position, playerPos.position) < activationRange) {
            if (timeBetweenShots <= 0) {
                animator.SetTrigger("throw");
                //add a small delay?
                Instantiate(projectile, transform.position, Quaternion.identity);
                timeBetweenShots = startTimeBetweenShots + Random.Range(0, 1.0f);
            }

            else {
                timeBetweenShots -= Time.deltaTime;
            }
        }

        rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector3.zero, ref currentVelocity, 0.8f);
    }
}
