using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeController : MonoBehaviour
{
    public static bool gameOver = false;
    public static bool gameWon = false;

    //player stats
    public float maxSpeed = 7f; //player's max speed
    public float smoothTime = 0.2f; //time to reach min/max speed

    public TimerScript timer;
   
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Vector3 currentVelocityPlayer = Vector3.zero;

    void Awake()
    {
        gameOver = false;
        gameWon = false;
        
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        moveInput = playerInput.Player.Move.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 targetVelocity = moveInput * maxSpeed;
        rb.velocity =  Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocityPlayer, smoothTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("you lose!!!!!!!!!!!!!!!!!!");
            gameOver = true;
            gameWon = false;
            timer.TimerOn = false;
            playerInput.Disable();
        }
        else if ((collision.gameObject.tag == "Goal") && (!gameOver))
        {
            Debug.Log("you win!!!!!!!!!!!!!!!!!!!");
            gameOver = true;
            gameWon = true;
            timer.TimerOn = false;
            playerInput.Disable();
        }
    }

    public void TimeUp()
    {
        Debug.Log("time up!!!!!!!!!!!!!!!!!!");
        gameOver = true;
        gameWon = false;
        playerInput.Disable();
    }
}
