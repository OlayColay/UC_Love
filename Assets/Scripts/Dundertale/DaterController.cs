using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaterController : MonoBehaviour
{
    public static bool gameOver = false;
    public static bool gameWon = false;

    //player stats
    public int health = 3;
    public float maxSpeed = 7f; //player's max speed
    public float maxDistance = 1000f;
   
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Vector3 currentVelocityPlayer = Vector3.zero;
    private Animator animator;
    private Vector2 center;

    void Awake()
    {
        gameOver = false;
        gameWon = false;
        
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        center = new Vector2(Screen.width/2, Screen.height/2);
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update(){
        moveInput = playerInput.Player.Move.ReadValue<Vector2>();

        if (Mathf.Abs(moveInput.x) >= float.Epsilon || Mathf.Abs(moveInput.y) >= float.Epsilon)
        {
            animator.SetFloat("horizontal", moveInput[0]);
            animator.SetFloat("vertical", moveInput[1]);
        }

        if ((health <= 0) && (!gameOver))
        {
            gameOver = true;
            gameWon = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position + (Vector3)moveInput * maxSpeed * Screen.width * Time.fixedDeltaTime, center) >= maxDistance * Screen.width)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity =  moveInput * maxSpeed * Screen.width;
        // Wheels spin only if moving
        animator.speed = rb.velocity.magnitude / maxSpeed;
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // Debug.Log("hit");
            health--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Goal") && (health > 0) && (!gameOver))
        {
            // Debug.Log("you win!!!!!!!!!!!!!!!!!!!");
            gameOver = true;
            gameWon = true;
        }
    }

    public void DeleteEnemies()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        GameObject.Destroy(enemy);
    }

}
