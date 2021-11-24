using System;
using Projectiles;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerController : MonoBehaviour, IPausable
    {

        private SpriteRenderer sprite;
        // Movement speed variables
        [FormerlySerializedAs("speed")] [Header("Movement speed variables")]
        public float movementForce;

        public float airMovementFraction;

        [Tooltip("Force applied to player rigidbody when performing a jump.")]
        public float jumpForce;

        [Tooltip("Max velocity for player in horizontal direction.")]
        public float maxMovementVelocity;

        [Tooltip("Value in range [0, 1). " +
                 "When no movement input is active, player velocity is reduced by multiplying it with this value each update. " +
                 "Lower values == faster slowdown. DO NOT SET TO 1 OR ABOVE, lol...")]
        public float slowdownMultiplier;
        public float airSlowdownMultiplier;

        [Tooltip("Number of jumps remaining to player. Reset this midair to 'restore' double jump ability.")]
        public int numberOfJumpsRemaining;

        [Tooltip("Number of jumps to be able to perform midair. Set to 0 to disable double jump.")]
        public int maxExtraJumps;

        public float moveInputX;
        private bool facingRight = true;

        [Tooltip("LayerMask of what constitutes ground.")]
        public LayerMask whatIsGround;
        [Tooltip("A transform used as contact surface with ground.")]
        public Transform groundChecker;
        //todo: rewrite as Collider2D instead of Transform

        [Tooltip("Radius from GroundChecker to extend search for ground layer contact.")]
        public float groundCheckRadius = 0.2f;
        public bool isGrounded;
        
        public Collider2D wallChecker;
        public bool isWallTouching;
        public bool isWallGrabbing;
        public bool hitWallThisFrame;
        public Vector2 touchedWallNormalVector;
        
        public float wallJumpForce;
        public float wallJumpForceAngle;
        [FormerlySerializedAs("gravityWallSlideDivider")] public float gravityWallSlideCounterForce = 1;
        private float gravityForce;


        //DIALOGUE START
        [SerializeField] private DialogueUI dialogueUI; //Serialized reference to the dialogue UI.

        public DialogueUI DialogueUI => dialogueUI; // Getter for dialogueUI.

        public IInteractable Interactable { get; set; }
        //DIALOGUE END


        //todo: review the implementation and usage of this throughout the script.


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

        /// Awake is called before Start
        void Awake()
        {
            // Parts of code taken from https://www.youtube.com/watch?v=vAZV5xO_AHU

            // The game always starts in the main menu, so UI should
            // be enabled first
            //gameManager = 

            rb = GetComponent<Rigidbody2D>();
            gravityForce = rb.gravityScale;
            controls = new PlayerInputAction();
            sprite = GetComponentInChildren<SpriteRenderer>();


            // todo: What the fuck is even going on here..?
            //  Need to read up on PlayerControls API


            // controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<float>();
            // controls.Player.Movement.canceled += _ => movement = 0;
            // we don't use 'movement' variable for anything, it seems?


            controls.Player.Move.performed += Move;
            controls.Player.Move.canceled += Move;
            ;

            controls.Player.Fire.started += Shoot;

            controls.Player.Jump.started += Jump;
            controls.Player.WallGrab.started += WallGrab;
            controls.Player.WallGrab.canceled += WallGrab;
            controls.Player.Pause.performed += _ => OnPauseGame();
            controls.UI.Cancel.performed += _ => OnResumeGame();

            controls.Player.Disable();
            controls.UI.Enable();
            // controls.Player.Enable();
        }


        // Update is called once per frame
        void Update()
        {
            //Dialogue related code ->
            if (dialogueUI != null)
            {
                if (dialogueUI.IsOpen) return;

                if (Keyboard.current[Key.E].wasPressedThisFrame)
                {
                    Interactable?.Interact(this); //If interactable is not null, reference this player.
                }
            }

            // Ground detector
            GroundCheck();
            
            // WallDetector
            WallCheck();
            
            
            // Things that happen in air or on wall
            if (!isGrounded)
            {

                // If we hit a wall this frame...
                if (hitWallThisFrame)
                {
                    // ...and grab was being held...
                    if (controls.Player.WallGrab.IsPressed())
                    {
                        Debug.Log("GRAB");
                        isWallGrabbing = true;
                        rb.velocity = new Vector2(0, 0); // ... we stop movement...
                        rb.gravityScale = 0; // and start a grab by turning off gravity
                    }
                }

                // If we were already touching the wall... 
                else if (isWallTouching)
                {
                    // ...AND if wallGrab was activated this frame
                    if (controls.Player.WallGrab.WasPressedThisFrame())
                    {
                        Debug.Log("GRAB");
                        isWallGrabbing = true;
                        rb.velocity = new Vector2(0, 0); // ... we stop movement...
                        rb.gravityScale = 0; // and start a grab by turning off gravity                           
                    }
                    // ... or if we were already grabbing
                    else if (isWallGrabbing)
                    {
                        rb.gravityScale = 0; // we stick around
                    }
                    // ... or if we're not grabbing but still touching the wall and pushing towards it
                    else if ((touchedWallNormalVector.x * moveInputX) > 0 && rb.velocity.y < 0)
                    {
                        // rb.AddForce(new Vector2(0, gravityForce*gravityWallSlideCounterForce), ForceMode2D.Impulse);
                        rb.gravityScale =
                            gravityForce / gravityWallSlideCounterForce; // ... we reduce gravity to slide down wall
                    }
                    else // touching but not pushing against, so we fall normal
                    {
                        rb.gravityScale = gravityForce; //reset gravity
                    }
                }

                // If we are not touching a wall in any way...
                else 
                {
                    rb.gravityScale = gravityForce; //reset gravity
                }
            }



            var currentVelocity = rb.velocity; // used a lot in here

            //todo: rewrite this to handle facing using the animator instead? Is that even a thing..?
            
            // Handle player sprite facing direction
            if (moveInputX != 0)
            {
                // we need to only turn the sprite itself around when grabbing a wall
                // but while we are running around, we need to set the transform of the whole
                // player object, so that the fire point etc is turned as well.
                // We can do this by checking if we are touching AND grabbing a wall,
                // and selecting which transform to change based on this.
                // However, when we then release the wall, we MUST ensure that the player transform
                // and the sprite transform are synchronised again, otherwise we will end up with unwanted behavior,
                // like the player seemingly moonwalking...
                running = true;
                
                if (moveInputX < 0 && facingRight == true) // movement to left, but facing right.
                {
                    Vector3 scaler = sprite.transform.localScale;
                    scaler.x *= -1;
                    sprite.transform.localScale = scaler;
                    facingRight = !facingRight;
                }
                else if (moveInputX > 0 && facingRight == false) //movement to right, but facing left 
                {
                    Vector3 scaler = sprite.transform.localScale;
                    scaler.x *= -1;
                    sprite.transform.localScale = scaler;
                    facingRight = !facingRight;
                }

                // Mathf.Sign returns 1 if input is positive or 0, and -1 if negative
                // if not at max velocity, or if input is in opposite direction of current velocity (turning around)
                if (Mathf.Abs(currentVelocity.x) < maxMovementVelocity ||
                    Mathf.Sign(currentVelocity.x) != Mathf.Sign(moveInputX))
                {
                    if (isGrounded)
                    {
                        rb.AddForce(new Vector2(moveInputX * movementForce, 0));
                    } else if (
                        isWallTouching 
                        && isWallGrabbing 
                        && ((touchedWallNormalVector.x * moveInputX) < 0))
                    {
                        // Ignore input away from wall if we are grabbing it.
                        //todo: this is bad code and I should feel bad. Fix it when brain is not soup.
                    }
                    else // movement in air has lower "effect" on speed than on ground, because physics.
                    {
                        rb.AddForce(new Vector2(moveInputX * movementForce * airMovementFraction, 0));
                    }
                }
            }
            else // if input == 0, we decelerate player velocity  
            {
                running = false;
                //apply force opposite of movement direction, scaled by velocity magnitude
                // rb.AddForce(new Vector2(
                //      (slowdownMultiplier * movementForce * (-1*Mathf.Sign(rb.velocity.x))),
                //     0));
                if (isGrounded)
                {
                    rb.velocity = new Vector2(currentVelocity.x * slowdownMultiplier, currentVelocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(currentVelocity.x * airSlowdownMultiplier, currentVelocity.y);

                }
            }

            // is player touching ground?
            // isGrounded = Physics2D.OverlapCircle(groundChecker.position, 0.1f, whatIsGround);

            // reset jump counter.
            // fixme: needs tweaking. Is triggering and resetting jump counter exactly as first jump starts?
            if (isGrounded)
            {
                numberOfJumpsRemaining = maxExtraJumps;
                rb.gravityScale = gravityForce;
            }

            animator.SetBool("isGrounded", isGrounded);
            animator.SetBool("isRunning", running);
        }


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

        void WallCheck()
        {

            bool wasOnWall = isWallTouching;
            RaycastHit2D ray = Physics2D.Linecast(
                new Vector2(firePoint.position.x, transform.position.y),
                transform.position,
                whatIsGround);
            // RaycastHit2D ray = wallChecker.Raycast()
            if (ray.collider != null)
            {
                isWallTouching = true;
                touchedWallNormalVector = ray.normal;
            }
            else
            {
                isWallTouching = false;
            }

            if (!wasOnWall) //we were not on the wall...
            {
                hitWallThisFrame = isWallTouching; //... but are we now?
                if (hitWallThisFrame) Debug.Log("Hit wall this frame!");
            }
            else
            {
                hitWallThisFrame = false;
            }
        }

        private void WallGrab(InputAction.CallbackContext obj)
        {
            isWallGrabbing = !obj.canceled; //reset wallGrab status when button is released.
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

        // InputAction.CallbackContext ctx
        public void Jump(InputAction.CallbackContext ctx)
        {
            // Air jump
            if (
                numberOfJumpsRemaining > 0  // we have jump charges left
                && !isGrounded              // and we are not on the ground
                && !isWallTouching)         // and we are not touching a wall
            {
                Debug.Log("AIRJUMP");
                // if !isGrounded, we spend one of our double jump charges.
                rb.velocity = new Vector2(rb.velocity.x, 0); // Stop fall
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                numberOfJumpsRemaining--;
            }
            // Wall Jump
            else if (isWallTouching && !isGrounded) 
            {
                if (isWallGrabbing)
                {
                    rb.gravityScale = gravityForce; //reset gravity so that we can be sure we actuall jump.
                }
                // if (!isWallGrabbing) rb.velocity = new Vector2(rb.velocity.x, 0); // Stop fall


                double radAngle = wallJumpForceAngle * Math.PI / 180;
                float xForce;

                //todo:
                // fixme: this makes no sense here, since we are touching up against a wall,
                //  and therefore have a horizontal speed of 0.
                if (Mathf.Abs(rb.velocity.x) < maxMovementVelocity)
                {
                    xForce = (float) (wallJumpForce * Math.Cos(radAngle) * (-1 * touchedWallNormalVector.x));
                }
                else xForce = 0;

                float yForce = (float) (wallJumpForce * Math.Sin(radAngle));
                Debug.Log($"WAllJUMP  x:{xForce}  y:{yForce}");
                rb.velocity = new Vector2(0, 0); // stop velocity, so we actually manage to kick off wall
                rb.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);
            }
            // else if (isGrounded && isWallTouching)
            // {
            //     rb.AddForce(new Vector2(0, jumpForce/gravityWallSlideDivider), ForceMode2D.Impulse);
            //
            // }

            // Normal Jump
            else if (isGrounded)
            {
                rb.gravityScale = gravityForce; // ensure that gravity is correct on a normal jump
                Debug.Log("JUMP");
                // rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
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