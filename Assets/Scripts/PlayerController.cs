using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{

    // Movement speed variables
    [FormerlySerializedAs("speed")] [Header("Movement speed variables")]
    public float moveSpeed;
    public float jumpForce;
    public float airMovement;
    public int numberOfJumps;
    private int jumpsPerformed;

    private float moveInputX;

    public Transform groundPoint;
    public LayerMask whatIsGround;
    public bool isGrounded;
    
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // input =  new InputPlayer();
        // input.Gameplay.Jump.performed += ctx => Jump();
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (jumpsPerformed < numberOfJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // rb.AddForce(new Vector2(0, jumpForce));
            jumpsPerformed++;
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            moveInputX = ctx.ReadValue<Vector2>().x;
            
        }
        else
        {
            moveInputX = ctx.ReadValue<Vector2>().x * airMovement;
        }
    }

    void OnEnable()
    {
        // input.Enable();
    }

    private void FixedUpdate()
    {
        // moveInput = Input.GetAxis("Horizontal");
        // moveInputX = input.Gameplay.Move.ReadValue<float>();
        // Debug.Log("Move: " + moveInput);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(moveInputX* moveSpeed, rb.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);
        if (isGrounded)
        {
            jumpsPerformed = 0;
        }
    }
}
