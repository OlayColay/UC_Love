using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float smoothTime = 0.25f;
    private Vector2 moveInput;
    private Vector3 currentVelocity = Vector3.zero;
    public int health;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();

        health = 3;
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

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 targetVelocity = moveInput * maxSpeed;
        rb.velocity =  Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);

    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("hit");
            health--;
            Destroy(collision.gameObject);
        }

    }
}
