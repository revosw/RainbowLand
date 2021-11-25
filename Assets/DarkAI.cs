using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkAI : MonoBehaviour
{
    private SpriteRenderer sprite;

    [Tooltip("LayerMask of what constitutes ground.")]
    public LayerMask whatIsGround;

    [Tooltip("A transform used as contact surface with ground.")]
    public Transform groundChecker;
    //todo: rewrite as Collider2D instead of Transform

    [Tooltip("Radius from GroundChecker to extend search for ground layer contact.")]
    public float groundCheckRadius = 0.2f;

    private bool isGrounded;

    public float moveInputX;


    public Animator animator;

    bool running = false;

    private Rigidbody2D rb;

    private void Awake() {
        sprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.x > 0f) {
            moveInputX = 1;
        } else if (rb.velocity.x < 0f) {
            moveInputX = -1;
        }
        sprite.transform.localScale = new Vector3(moveInputX, 1, 1);

        GroundCheck();
        if (Mathf.Abs(rb.velocity.x) > 0) running = true;
        else running = false;

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", running);

    }

    bool GroundCheck() {
        isGrounded = false;
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(groundChecker.position, groundCheckRadius, whatIsGround);
        if (colliders.Length > 0) {
            isGrounded = true;
        }
        // //Debug.Log("Ground Check performed: isGrounded = " + isGrounded);
        return isGrounded;
    }
}
