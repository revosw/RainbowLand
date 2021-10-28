using Projectiles;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngineInternal;

namespace Player
{
    public class PlayerController : MonoBehaviour
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

        public float shootCooldown;
        private float cooldownTimer;
        public Transform firePoint;
        public GameObject[] projectiles;


        private Rigidbody2D rb;


        // Awake is called before Start
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            // var kb = Keyboard.current; // just a reminder that this exists...
        

            var currentVelocity = rb.velocity; // used a lot in here

            if (moveInputX != 0)
            {
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

                // Debug.Log("Player velocity: x = " + currentVelocity.x + " y = " + currentVelocity.y);
            
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
                rb.velocity = new Vector2(currentVelocity.x * slowdownMultiplier, currentVelocity.y);
            }

            // is player touching ground?
            isGrounded = Physics2D.OverlapCircle(groundPoint.position, 0.1f, whatIsGround);
        
            // reset jump counter.
            // fixme: needs tweaking. Is triggering and resetting jump counter exactly as first jump starts?
            if (isGrounded)
            {
                numberOfJumps = maxExtraJumps;
            }
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
            }
            else if (!isGrounded && ctx.started) // changing movement during jump/fall
            {
                Debug.Log("move changed in air");
                moveInputX = ctx.ReadValue<Vector2>().x;
            }


            Debug.Log("x input value: " + moveInputX);
        }

        //fixme: projectile direction changes when player direction changes..?
        public void Shoot(InputAction.CallbackContext ctx)
        {
            if (cooldownTimer >= shootCooldown)
            {
                Debug.Log("SHOOT");
                cooldownTimer = 0;
                int projectileIndex = FindProjectile();
                projectiles[projectileIndex].transform.position = firePoint.position;
                projectiles[projectileIndex].GetComponent<Projectile>().activate(transform.localScale.x);
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
    }
}