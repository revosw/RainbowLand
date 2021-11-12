using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class which handles player movement
/// 
/// Code taken from https://www.youtube.com/watch?v=vAZV5xO_AHU
/// </summary>
public class PlayerController : MonoBehaviour, IPausable
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpStrength;
    private Rigidbody2D rb;
    private PlayerControls controls;
    private float movement;
    private bool isGrounded;
    [SerializeField] GameManager gameManager;


    private void Awake()
    {
        // The game always starts in the main menu, so UI should
        // be enabled first
        //gameManager = 
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();



        controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<float>();
        controls.Player.Movement.canceled += _ => movement = 0;
        controls.Player.Pause.performed += _ => OnPauseGame();

        controls.Player.Jump.started += _ => Jump();
        controls.Player.Disable();
        controls.UI.Enable();
        controls.UI.Cancel.performed += _ => OnResumeGame();
    }

    public void OnPauseGame()
    {
        controls.UI.Enable();
        controls.Player.Disable();
        gameManager.OnPauseGame();
    }
    public void OnResumeGame()
    {
        controls.UI.Disable();
        controls.Player.Enable();
        gameManager.OnResumeGame();
    }


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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
