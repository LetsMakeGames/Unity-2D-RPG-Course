using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float xInput = 0.0f;
    [SerializeField] private float yInput = 0.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private Vector2 velocity = Vector2.zero;

    [Header("Dash Info")]
    [SerializeField] private float dashSpeed = 0.0f;
    [SerializeField] private float dashDuration = 0.0f;
    [SerializeField] private float dashTimer = 0.0f;
    [SerializeField] private float dashCooldown = 0.0f;
    [SerializeField] private float dashCooldownTimer = 0.0f;

    private bool isDashing = false;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance = 0.0f;
    [SerializeField] private LayerMask whatIsGround;

    private bool isGrounded = false;
    private bool isMoving = false;
    private bool facingRight = true;

    private int facingDir = 1;

    private Rigidbody2D rb;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Timers();
        CheckInput();
        Movement();
        AnimatorControllers();
        FlipController();

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        // Inputs
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        // Vertical Movement
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
    }

    private void Movement()
    {
        if (dashTimer > 0)
        {
            rb.velocity = new Vector2(xInput * dashSpeed, 0);
        }
        else
        {
            isDashing = false;
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
        
        velocity = rb.velocity;
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isDashing", isDashing);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

    }

    private void Dash()
    {
        if (dashCooldownTimer <= 0)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void Timers()
    {
        dashTimer -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
