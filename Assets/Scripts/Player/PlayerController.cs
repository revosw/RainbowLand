using Projectiles;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerController : MonoBehaviour, IPausable
    {
        // Movement speed variables
        [FormerlySerializedAs("speed")] [Header("Movement speed variables")]
        public float movementForce;

        [Tooltip("Force applied to player rigidbody when performing a jump.")]
        public float jumpForce;

        [Tooltip("Max velocity for player in horizontal direction.")]
        public float maxMovementVelocity;

        [Tooltip("Value in range [0, 1). " +
                 "When no movement input is active, player velocity is reduced by multiplying it with this value each update. " +
                 "Lower values == faster slowdown. DO NOT SET TO 1 OR ABOVE, lol...")]
        public float slowdownMultiplier;

        [Tooltip("Number of jumps remaining to player. Reset this midair to 'restore' double jump ability.")]
        public int numberOfJumpsRemaining;

        [Tooltip("Number of jumps to be able to perform midair. Set to 0 to disable double jump.")]
        public int maxExtraJumps;

        private float moveInputX;
        private bool facingRight = true;

        [Tooltip("LayerMask of what constitutes ground.")]
        public LayerMask whatIsGround;
        public bool isGrounded;

        [Tooltip("A transform used as contact surface with ground.")]
        public Transform groundChecker;

        [Tooltip("Radius from GroundChecker to extend search for ground layer contact.")]
        public float groundCheckRadius = 0.2f;


        //DIALOGUE START
        [SerializeField] private DialogueUI dialogueUI; //Serialized reference to the dialogue UI.

        public DialogueUI DialogueUI => dialogueUI; // Getter for dialogueUI.

        public IInteractable Interactable { get; set; }
        //DIALOGUE END


        //todo: review the implementation and usage of this throughout the script.
        bool GroundCheck()
        {
            isGrounded = false;
            Collider2D[] colliders =
                Physics2D.OverlapCircleAll(groundChecker.position, groundCheckRadius, whatIsGround);
            if (colliders.Length > 0)
            {
                isGrounded = true;
            }
            // Debug.Log("Ground Check performed: isGrounded = " + isGrounded);
            return isGrounded;
        }

        [Tooltip("Does the player have the shooting ability? Activate through pickups, or set manually.")]
        public bool canShoot = false;
        [Tooltip("Cooldown time before new projectile can be fired. Set to 0 for lols...")]
        public float shootCooldown;
        private float _cooldownTimer;
        [Tooltip("Where should projectiles be fired from? Ideally, this would be a transform that is a child " +
                 "of the Player object (or the player itself).")]
        public Transform firePoint;
        //todo: refactor this as a separate class thing?
        [Tooltip("The set of projectiles that the player can fire.")]
        public GameObject[] projectiles;

        private PlayerInputAction controls;
        private float movement;

        public Animator animator;
        bool running = false;


        private Rigidbody2D rb;
        [SerializeField] GameManager gameManager;


        // Update is called once per frame

        void Update()
        {
            //Dialogue related code ->
            if (dialogueUI != null)
                {
                if (dialogueUI.IsOpen) {
                    movementForce = 0;
                    jumpForce = 0;
                } else {
                    jumpForce = 35;
                    movementForce = 100;
                }
             }
            

            if (Keyboard.current[Key.E].wasPressedThisFrame)
            {
                Interactable?.Interact(this); //If interactable is not null, reference this player.
            }
            //////////

            // var kb = Keyboard.current; // just a reminder that this exists...

            // if (movement < 0)
            // {
            //     transform.localScale = new Vector3(-1, 1, 1);
            // }
            // else if (movement > 0)
            // {
            //     transform.localScale = new Vector3(1, 1, 1);
            // }
            // else
            // {
            //     
            // }

            GroundCheck();
            var currentVelocity = rb.velocity; // used a lot in here
            
            //todo: rewrite this to handle facing using the animator instead? Is that even a thing..?
            if (moveInputX != 0)
            {
                running = true;
                if (moveInputX < 0 && facingRight == true)
                {
                    Vector3 scaler = transform.localScale;
                    scaler.x *= -1;
                    transform.localScale = scaler;
                    facingRight = !facingRight;
                }
                else if (moveInputX > 0 && facingRight == false)
                {
                    Vector3 scaler = transform.localScale;
                    scaler.x *= -1;
                    transform.localScale = scaler;
                    facingRight = !facingRight;
                }
            
            
                // if not at max velocity, or if input is in opposite direction of current velocity (turning around)
                // Mathf.Sign returns 1 if input is positive or 0, and -1 if negative
                if (Mathf.Abs(currentVelocity.x) < maxMovementVelocity ||
                    Mathf.Sign(currentVelocity.x) != Mathf.Sign(moveInputX))
                {
                    rb.AddForce(new Vector2(moveInputX * movementForce, 0));
                }
            }
            else // if input == 0, we decelerate player velocity  
            {
                running = false;
                rb.velocity = new Vector2(currentVelocity.x * slowdownMultiplier, currentVelocity.y);
            }
            
            // is player touching ground?
            // isGrounded = Physics2D.OverlapCircle(groundChecker.position, 0.1f, whatIsGround);
            
            // reset jump counter.
            // fixme: needs tweaking. Is triggering and resetting jump counter exactly as first jump starts?
            if (isGrounded)
            {
                numberOfJumpsRemaining = maxExtraJumps;
            }
            
            animator.SetBool("isGrounded", isGrounded);
            animator.SetBool("isRunning", running);
        }

        public void OnPauseGame()
        {
            controls.UI.Enable();
            controls.Player.Disable();
            gameManager.OnPauseGame();
        }

        /// Awake is called before Start
        void Awake()
        {
            // Parts of code taken from https://www.youtube.com/watch?v=vAZV5xO_AHU

            // The game always starts in the main menu, so UI should
            // be enabled first
            //gameManager = 

            rb = GetComponent<Rigidbody2D>();
            controls = new PlayerInputAction();


            // todo: What the fuck is even going on here..?
            //  Need to read up on PlayerControls API


            // controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<float>();
            // controls.Player.Movement.canceled += _ => movement = 0;
            // we don't use 'movement' variable for anything, it seems?


            controls.Player.Move.performed += Move;
            controls.Player.Move.canceled += Move;;

            controls.Player.Fire.started += Shoot;

            controls.Player.Jump.started += Jump;
            
            controls.Player.Pause.performed += _ => OnPauseGame();
            controls.UI.Cancel.performed += _ => OnResumeGame();
            
            controls.Player.Disable();
            controls.UI.Enable();
            // controls.Player.Enable();
        }

        public void OnResumeGame()
        {
            controls.UI.Disable();
            controls.Player.Enable();
            gameManager.OnResumeGame();
        }

        // InputAction.CallbackContext ctx
        public void Jump(InputAction.CallbackContext ctx)
        {
            Debug.Log("JUMP");
            if (numberOfJumpsRemaining > 0 && !isGrounded)
            {
                // if !isGrounded, we spend one of our double jump charges.
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                numberOfJumpsRemaining--;
            }
            else if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

        }

        public void Move(InputAction.CallbackContext ctx)
        {
            float horizontalInput = ctx.ReadValue<Vector2>().x;

            if (horizontalInput == 0f)
            {
                moveInputX = 0;
            }
            else
                //ground movement
            {
                Debug.Log("move on ground");

                if (horizontalInput > 0f)
                {
                    moveInputX = 1;
                }
                else if (horizontalInput < 0f)
                {
                    moveInputX = -1;
                }
            }

            // else if (!isGrounded) // changing movement during jump/fall
            // {
            //     Debug.Log("move changed in air");
            //     if (horizontalInput > 0f)
            //     {
            //         moveInputX = 1;
            //     }
            //     else if (horizontalInput < 0f)
            //     {
            //         moveInputX = -1;
            //     }
            // }


            Debug.Log("x input value: " + moveInputX);
        }

        //fixme: projectile direction changes when player direction changes..?
        public void Shoot(InputAction.CallbackContext ctx)
        {
            
            //todo: projectiles don't despawn... Why?
            if (canShoot)
            {
                if (_cooldownTimer >= shootCooldown)
                {
                    Debug.Log("SHOOT");
                    _cooldownTimer = 0;
                    int projectileIndex = FindProjectile();
                    // todo: can we fix issue with projectile following player orientation
                    // by changing the way we assign a transform position to it?
                    var projectile = projectiles[projectileIndex];
                    var position = firePoint.position;
                    projectile.transform.position = position;
                    var direction = transform.localScale.x;
                    projectile.GetComponent<Projectile>().activate(direction);
                }

                _cooldownTimer += Time.deltaTime;
            }
        }

        private int FindProjectile()
        {
            for (int i = 0; i < projectiles.Length; i++)
            {
                if (!projectiles[i].activeSelf)
                    return i;
            }

            return 0;
        }

        //todo: refactor these two as a GroundChecker object?
        // private void OnCollisionEnter2D(Collision2D other)
        // {
        //     if (other.gameObject.CompareTag("Ground"))
        //     {
        //         isGrounded = true;
        //     }
        // }
        //
        // private void OnCollisionExit2D(Collision2D other)
        // {
        //     if (other.gameObject.CompareTag("Ground"))
        //     {
        //         isGrounded = false;
        //     }
        // }

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