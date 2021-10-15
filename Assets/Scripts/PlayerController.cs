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
        rb.velocity = new Vector2(moveInputX* moveSpeed, rb.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);
        if (isGrounded)
        {
            jumpsPerformed = 0;
        }
    }
}
