using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3;
    public float jumpForce = 250;
    public GameObject groundCheckPosition;
    public float groundCheckRadius = 0.1f;
    
    public Rigidbody2D playerRb;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private Vector2 movement;
    private int jumps = 0;
    private bool wasGrounded = true;
    
    private float face = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(wasGrounded)
        movement.x = Input.GetAxis("Horizontal");

        
        animator.SetFloat("Speed", Math.Abs(movement.x));
        
        if (movement.x * face < 0) face *= -1;
        spriteRenderer.flipX = (face < 0);

        if (Input.GetButton("Jump") && wasGrounded)
        {
            jumps = 10;
        }

    }


    private void OnGrounding()
    {
        animator.SetBool("isJumping", false);
    }

    private void OnDisGrounding()
    {
        animator.SetBool("isJumping", true);
    }
    
    private bool isGrounded()
    {
        var colliders = Physics2D.OverlapCircleAll(groundCheckPosition.transform.position, groundCheckRadius,LayerMask.GetMask("Default"));
        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private void FixedUpdate()
    {

        var grounded = isGrounded();
        if (grounded && !wasGrounded)
        {
            OnGrounding();
        }
        else if (!grounded && wasGrounded)
        {
            OnDisGrounding();
        }

        wasGrounded = grounded;
        
            playerRb.MovePosition(playerRb.position + movement * speed * Time.fixedDeltaTime);

            if (jumps >0)
            {
                jumps--;
                playerRb.AddForce(Vector2.up * jumpForce);
            }
            
    }
}
