using Projectiles;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngineInternal;

namespace Player
{
    public class PlayerController : MonoBehaviour, IPausable
    {
        // Movement speed variables
        [FormerlySerializedAs("speed")] [Header("Movement speed variables")]
        public float movementForce;
        public float jumpForce;
        public float maxMovementVelocity;
        public float slowdownMultiplier;
        public int numberOfJumps;
        public int maxExtraJumps;

        private float moveInputX;
        private bool facingRight = true;

        public Transform groundPoint;
        public LayerMask whatIsGround;
        public bool isGrounded;

        public bool canShoot = false;
        public float shootCooldown;
        private float cooldownTimer;
        public Transform firePoint;
        public GameObject[] projectiles;

        private PlayerControls controls;
        private float movement;


        private Rigidbody2D rb;
        [SerializeField] GameManager gameManager;


        // Awake is called before Start
        void Awake()
        {
            // Parts of code taken from https://www.youtube.com/watch?v=vAZV5xO_AHU
            
            // The game always starts in the main menu, so UI should
            // be enabled first
            //gameManager = 

            rb = GetComponent<Rigidbody2D>();
            controls = new PlayerControls();



            controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<float>();
            controls.Player.Movement.canceled += _ => movement = 0;
            controls.Player.Pause.performed += _ => OnPauseGame();

            controls.Player.Jump.started += Jump;
            controls.Player.Disable();
            controls.UI.Enable();
            controls.UI.Cancel.performed += _ => OnResumeGame();
        }

        // Update is called once per frame
        void Update()
        {
            // var kb = Keyboard.current; // just a reminder that this exists...
            
            if (movement < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (movement > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        

            var currentVelocity = rb.velocity; // used a lot in here
            
            rb.velocity = new Vector2(movement * movementForce, rb.velocity.y);
            //
            // if (moveInputX != 0)
            // {
            //     if (moveInputX < 0 && facingRight == true)
            //     {
            //         Vector3 scaler = transform.localScale;
            //         scaler.x *= -1;
            //         transform.localScale = scaler;
            //         facingRight = !facingRight;
            //     }                
            //     else if (moveInputX > 0 && facingRight == false)
            //     {
            //         Vector3 scaler = transform.localScale;
            //         scaler.x *= -1;
            //         transform.localScale = scaler;
            //         facingRight = !facingRight;
            //     }
            //
            //     // Debug.Log("Player velocity: x = " + currentVelocity.x + " y = " + currentVelocity.y);
            //
            //     // if not at max velocity, or if input is in opposite direction of current velocity (turning around)
            //     // Mathf.Sign returns 1 if input is positive or 0, and -1 if negative
            //     if (Mathf.Abs(currentVelocity.x) < maxMovementVelocity ||
            //         Mathf.Sign(currentVelocity.x) != Mathf.Sign(moveInputX))
            //     {
            //         rb.AddForce(new Vector2(moveInputX * movementForce, 0));
            //     }
            // }
            // else // if input == 0, we decelerate player velocity  
            // {
            //     rb.velocity = new Vector2(currentVelocity.x * slowdownMultiplier, currentVelocity.y);
            // }
            //
            // // is player touching ground?
            // isGrounded = Physics2D.OverlapCircle(groundPoint.position, 0.1f, whatIsGround);
            //
            // // reset jump counter.
            // // fixme: needs tweaking. Is triggering and resetting jump counter exactly as first jump starts?
            // if (isGrounded)
            // {
            //     numberOfJumps = maxExtraJumps;
            // }
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

        public void Jump(InputAction.CallbackContext ctx)
        {if (ctx.started)
            {
                if (numberOfJumps > 0 && !isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    // rb.AddForce(new Vector2(0, jumpForce));
                    numberOfJumps--;
                    // Debug.Log("Jumps performed: " + jumpsPerformed);
                    
                } else if (isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }
        }

        // public void Move(InputAction.CallbackContext ctx)
        // {
        //     if (ctx.ReadValue<Vector2>().x == 0.0f)
        //     {
        //         moveInputX = 0;
        //         return;
        //     }
        //
        //     if (isGrounded) //ground movement
        //     {
        //         Debug.Log("move on ground");
        //         moveInputX = ctx.ReadValue<Vector2>().x;
        //     }
        //     else if (!isGrounded && ctx.started) // changing movement during jump/fall
        //     {
        //         Debug.Log("move changed in air");
        //         moveInputX = ctx.ReadValue<Vector2>().x;
        //     }
        //
        //
        //     Debug.Log("x input value: " + moveInputX);
        // }

        //fixme: projectile direction changes when player direction changes..?
        // public void Shoot(InputAction.CallbackContext ctx)
        // {
        //     if (canShoot) {
        //         if (cooldownTimer >= shootCooldown)
        //         {
        //             Debug.Log("SHOOT");
        //             cooldownTimer = 0;
        //             int projectileIndex = FindProjectile();
        //             // todo: can we fix issue with projectile following player orientation
        //             // by changing the way we assign a transform position to it?
        //             projectiles[projectileIndex].transform.position = firePoint.position;
        //             projectiles[projectileIndex].GetComponent<Projectile>().activate(transform.localScale.x);
        //         }
        //
        //         cooldownTimer += Time.deltaTime;
        //     }
        // }

        private int FindProjectile()
        {
            for (int i = 0; i < projectiles.Length; i++)
            {
                if (!projectiles[i].activeInHierarchy)
                    return i;
            }

            return 0;
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
    }
}