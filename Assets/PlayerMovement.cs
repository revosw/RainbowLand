using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rigidBody2D;
    public float moveSpeed, jumpForce;
    private bool isGrounded;
    private float inputX;
    const float groundCheckRadius = 0.2f;
    public Transform groundCheckCollider;
    public LayerMask groundLayer;

    bool airjump = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool GroundCheck() {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0) {
            isGrounded = true;
        }
        return isGrounded;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckCollider.position, groundCheckRadius, groundLayer);
        rigidBody2D.velocity = new Vector2(inputX * moveSpeed, rigidBody2D.velocity.y);
    }

    public void Move(InputAction.CallbackContext context) {
        inputX = context.ReadValue<Vector2>().x;
    }

    //Double Jump functionality
    public void Jump(InputAction.CallbackContext context) {
        if (context.performed && GroundCheck()) {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, jumpForce);
            airjump = true;
        } else if(airjump == true && context.performed) {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, jumpForce);
            airjump = false;
        }
        
    }
}
