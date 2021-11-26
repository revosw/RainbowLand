using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkAI : MonoBehaviour
{
    private SpriteRenderer sprite; //Boss sprite

    [Tooltip("LayerMask of what constitutes ground.")]
    public LayerMask whatIsGround;

    [Tooltip("A transform used as contact surface with ground.")]
    public Transform groundChecker;
    //todo: rewrite as Collider2D instead of Transform

    [Tooltip("Radius from GroundChecker to extend search for ground layer contact.")]
    public float groundCheckRadius = 0.2f;

    private bool isGrounded = false;

    private float moveInputX;


    private Animator animator;

    bool running = false;

    private Rigidbody2D rb;

    //Movement fields
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3[] positions;
    private int index;

    //Shooting fields
    private float stopAndShootCD = 3f;
    public float range;
    private float distToPlayer;
    public Transform player;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
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
        //Shooting code
        if (distToPlayer <= range) //check if player is in range to be shot at.
        {
            if (Time.time > stopAndShootCD) {
                stopAndShootCD = Time.time + 8f;
                StartCoroutine(StopAndShoot());
            }
        }


        //Movement code
        transform.position = Vector2.MoveTowards(transform.position, positions[index], Time.deltaTime * speed);
        if (transform.position == positions[index]) {
            if (index == positions.Length - 1) {
                index = 0;
            } else {
                index++;
            }
        }


        //Sprite change on turn
        if (rb.velocity.x > 0f) {
            moveInputX = 1;
        } else if (rb.velocity.x < 0f) {
            moveInputX = -1;
        }
        sprite.transform.localScale = new Vector3(moveInputX, 1, 1);

        //Ground check for running animation
        GroundCheck();
        if (Mathf.Abs(rb.velocity.x) > 0) running = true;
        else running = false;

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", running);

    }

    bool GroundCheck() {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(groundChecker.position, groundCheckRadius, whatIsGround);
        print(colliders.Length);
        if (colliders.Length > 0) {
            isGrounded = true;
        }
        Debug.Log("Ground Check performed: isGrounded = " + isGrounded);
        return isGrounded;
    }


    public GameObject bullet; //bullet prefab.
    public Transform shootPos; //Shoot from
    public Transform shootPosLeft;
    public float shootPower = 3f; //shooting power
    public Transform target; // target position
    private void Shoot() {
        Vector2 shootFrom = new Vector2(shootPos.position.x, shootPos.position.y);
        if (target.position.x < 0) { //Make sure boss doesnt shoot itself.
            shootFrom = new Vector2(shootPosLeft.position.x, shootPosLeft.position.y);
        }
        GameObject newBullet = Instantiate(bullet, shootFrom, Quaternion.identity);
        Vector2 direction = shootFrom - (Vector2)target.position; //get the direction to the target
        //newBullet.GetComponent<Rigidbody2D>().velocity = direction * shootPower;

        newBullet.GetComponent<Rigidbody2D>().velocity = -1 * (shootFrom - (Vector2)target.position);
    }


    IEnumerator StopAndShoot() {
        speed = 0;
        Shoot();
        yield return new WaitForSeconds(1);
        Shoot();
        yield return new WaitForSeconds(1);
        Shoot();
        yield return new WaitForSeconds(1);
        speed = 10f;
    }

}
