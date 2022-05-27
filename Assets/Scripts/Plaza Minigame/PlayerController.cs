using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool gameOver = false;
    public static bool gameWon = false;

    //player stats
    public int health = 3;
    public float maxSpeed = 7f; //player's max speed
    public float smoothTime = 0.2f; //time to reach min/max speed
   
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Vector3 currentVelocityPlayer = Vector3.zero;
    private Animator animator;
    private AudioClip paper;

  
    //flyers (screen cover)
    public GameObject flyer1;
    public GameObject flyer2;
    public GameObject flyer3;
    private Vector3 currentVelocityFlyer1 = Vector3.zero;
    private Vector3 currentVelocityFlyer2 = Vector3.zero;
    private Vector3 currentVelocityFlyer3 = Vector3.zero;
    private Vector3 flyer1offset;
    private Vector3 flyer2offset;
    private Vector3 flyer3offset;

    void Awake()
    {
        gameOver = false;
        gameWon = false;
        
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        paper = Resources.Load<AudioClip>("SFX/Paper");

        flyer1offset = (flyer1.transform.position - Camera.main.transform.position);
        flyer2offset = (flyer2.transform.position - Camera.main.transform.position);
        flyer3offset = (flyer3.transform.position - Camera.main.transform.position);

        flyer1.transform.position = flyer1.transform.position + new Vector3(-10, 10, 0);
        flyer2.transform.position = flyer2.transform.position + new Vector3(10, 10, 0);
        flyer3.transform.position = flyer3.transform.position + new Vector3(0, -20, 0);
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

        if (health <= 2)
        {
            flyer1.GetComponent<SpriteRenderer>().enabled = true;
            flyer1.transform.position = Vector3.SmoothDamp(flyer1.transform.position, Camera.main.transform.position + flyer1offset, ref currentVelocityFlyer1, smoothTime);
        }

        if (health <= 1)
        {
            flyer2.GetComponent<SpriteRenderer>().enabled = true;
            flyer2.transform.position = Vector3.SmoothDamp(flyer2.transform.position, Camera.main.transform.position + flyer2offset, ref currentVelocityFlyer2, smoothTime);
        }

        if ((health <= 0))
        {
            flyer3.GetComponent<SpriteRenderer>().enabled = true;
            flyer3.transform.position = Vector3.SmoothDamp(flyer3.transform.position, Camera.main.transform.position + flyer3offset, ref currentVelocityFlyer3, smoothTime);
            // Debug.Log("you lose!!!!!!!!!!!!!!!!!!!");
        }

        if ((health <= 0) && (!gameOver))
        {
            gameOver = true;
            gameWon = false;
        }

        // Debug.Log(moveInput[0]);
        // Debug.Log(moveInput[1]);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 targetVelocity = moveInput * maxSpeed;
        rb.velocity =  Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocityPlayer, smoothTime);
        // Wheels spin only if moving
        animator.speed = rb.velocity.magnitude / maxSpeed;
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // Debug.Log("hit");
            MusicPlayer.audioSource.PlayOneShot(paper);
            health--;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Goal") && (health > 0) && (!gameOver))
         {
            // Debug.Log("you win!!!!!!!!!!!!!!!!!!!");
            gameOver = true;
            gameWon = true;
            GainMoney();
         }
    }

    private void GainMoney()
    {
        string difficulty = FindObjectOfType<MapController>().difficulty;
        int money = 0;

        if (difficulty == "easy")
        {
            money = 80 + (health * 10);
        }
        else if (difficulty == "medium")
        {
            money = 180 + (health * 10);
        }
        else if (difficulty == "hard")
        {
            money = 280 + (health * 10);
        }

        // int enemyMoney = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Inventory.ChangeMoney(money);
        // Debug.Log("Money gained: " + money);
    }


    public void DeleteEnemies()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        GameObject.Destroy(enemy);
    }

}
