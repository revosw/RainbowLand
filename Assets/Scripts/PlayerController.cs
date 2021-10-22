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
    public float movementForce;

    public float jumpForce;
    public float maxMovementVelocity;
    public float slowdownMultiplier;
    public int numberOfJumps;
    public int jumpsPerformed;

    private float moveInputX;

    public Transform groundPoint;
    public LayerMask whatIsGround;
    public bool isGrounded;

    public float shootCooldown;
    private float cooldownTimer;
    public Transform firePoint;
    public GameObject[] projectiles;


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
        if (ctx.started)
        {
            if (jumpsPerformed < numberOfJumps)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                // rb.AddForce(new Vector2(0, jumpForce));
                jumpsPerformed++;
            }
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<Vector2>().x == 0.0f)
        {
            moveInputX = 0;
            return;
        }
        if (isGrounded) //ground movement
        {
            Debug.Log("move on ground");
            moveInputX = ctx.ReadValue<Vector2>().x;
            // rb.velocity = new Vector2(moveInputX * moveSpeed, rb.velocity.y);
            // rb.AddForce(new Vector2(moveInputX*moveSpeed, 0));
        }
        else if (!isGrounded && ctx.started) // changing movement during jump/fall
        {
            Debug.Log("move changed in air");
            moveInputX = ctx.ReadValue<Vector2>().x;

            // rb.velocity = new Vector2(moveInputX * moveSpeed * airMovement, rb.velocity.y);
        }
        
        
        Debug.Log("x input value: " + moveInputX);

    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (cooldownTimer >= shootCooldown)
        {
            Debug.Log("SHOOT");
            cooldownTimer = 0;
            int projectileIndex = FindProjectile();
            projectiles[projectileIndex].transform.position = firePoint.position;
            projectiles[projectileIndex].GetComponent<Projectile>().activate();
            // .setDirection(Mathf.Sign(transform.localScale.x));    
        }

        cooldownTimer += Time.deltaTime;
    }

    private int FindProjectile()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (!projectiles[i].activeInHierarchy)
                return i;
        }

        return 0;
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
        var kb = Keyboard.current;

        var currentVelocity = rb.velocity;

        //todo: this check of isGrounded solves air speed getting stuck on landing, but also forces air speed to always be reduced...
        // if (isGrounded)
        // {
        if (moveInputX != 0)
        {
            Debug.Log("Player velocity: x = " + currentVelocity.x + " y = " + currentVelocity.y);
            if (Mathf.Abs(currentVelocity.x) < maxMovementVelocity || Mathf.Sign(rb.velocity.x) != Mathf.Sign(moveInputX))
            {
                rb.AddForce(new Vector2(moveInputX*movementForce, 0));
            }
 
        }
        else
        {
            rb.velocity = new Vector2(currentVelocity.x * slowdownMultiplier, currentVelocity.y);
        }
            // rb.velocity = new Vector2(moveInputX * moveSpeed, rb.velocity.y);
        // }
        // if (!isGrounded)
        // {
        //     rb.velocity = new Vector2(moveInputX * moveSpeed * airMovement, rb.velocity.y);
        // }

        isGrounded = Physics2D.OverlapCircle(groundPoint.position, .1f, whatIsGround);
        if (isGrounded)
        {
            jumpsPerformed = 0;
        }
        
    }
}