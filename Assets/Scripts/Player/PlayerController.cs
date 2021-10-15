using UnityEngine;

/// <summary>
/// Class which handles player movement
/// 
/// Code taken from https://www.youtube.com/watch?v=vAZV5xO_AHU
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float jumpStrength;
    private Rigidbody2D rb;
    private PlayerControls controls;
    private float movement;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();

        controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<float>();
        controls.Player.Movement.canceled += _ => movement = 0;

        controls.Player.Jump.started += _ => Jump();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();


    private void Update()
    {
        if (movement < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movement > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movement * movementSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision other) {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
