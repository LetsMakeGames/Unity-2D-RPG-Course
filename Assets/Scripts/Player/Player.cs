using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float xInput = 0.0f;
    [SerializeField] private float yInput = 0.0f;
    [SerializeField] public float moveSpeed = 5.0f;
    [SerializeField] public float maxVelocityX = 5.0f;
    [SerializeField] public float jumpVelocity = 10.0f;

    [SerializeField] private Vector2 velocity = Vector2.zero;

    private bool isGrounded = false;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // Inputs
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        velocity = rb.velocity;


        // Horizontal Movement
        if (xInput != 0)
        {
            if (velocity.x < maxVelocityX && velocity.x > -maxVelocityX)
            {
                rb.AddForce(new Vector2(xInput * moveSpeed, rb.velocity.y));
                Debug.Log("Adding H Force");
            } else if (rb.velocity.x > maxVelocityX)
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
                Debug.Log("Over Max Velocity Right, set 5");
            } else if (rb.velocity.x < -maxVelocityX)
            {
                rb.velocity = new Vector2(rb.velocity.x, -5);
                Debug.Log("Over Max Velocity Left, set -5");
            }
            
            Debug.Log("You're moving horizontally!");
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            Debug.Log("You stopped moving...");
        }

        // Vertical Movement
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpVelocity));
            Debug.Log("Jump!");
        }
    }
}
