using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    //Grounded stuff
    [Tooltip("LayerMask of what constitutes ground.")]
    public LayerMask whatIsGround;
    [Tooltip("A transform used as contact surface with ground.")]
    public Transform groundChecker;
    [Tooltip("Radius from GroundChecker to extend search for ground layer contact.")]
    public float groundCheckRadius = 0.2f;
    private bool isGrounded = false;
    public Transform player;
    
    //Movement fields
    bool running = false;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3[] positions;
    private int index;

    //Shooting fields
    private float stopAndShootCD = 3f;
    public float range;
    private float distToPlayer;

    [SerializeField] public float jumpForce = 3;

    BeerShooter beerShooter;
    EnemyHealth health;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponentInChildren<EnemyHealth>();
        beerShooter = GetComponent<BeerShooter>(); 
    }

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
        if (positions[index].x >= transform.position.x) transform.localScale = new Vector3(1f, 1f, 1f);
        else transform.localScale = new Vector3(-1f, 1f, 1f);
        
        if (positions[index].y - transform.position.y > 1 
            && Mathf.Abs(positions[index].x - transform.position.x) < 3) 
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

        transform.position = Vector2.MoveTowards(transform.position, positions[index], Time.deltaTime * speed);
        if (transform.position == positions[index]) {
            //rb.velocity = transform.position; // Må gi han fart???
            //if(Mathf.Abs(positions[index].y - transform.position.y) > 5) rb.AddForce(transform.up * 1000, ForceMode2D.Impulse); <- Hvis han skal et sted han trenge å hoppe for
            
            if (index == positions.Length - 1) {
                index = 0;
            } else {    
                index++;
            }
        }        
        //Ground check for running animation
        GroundCheck();
        
        if (Mathf.Abs(rb.velocity.x) > 0) running = true;
        else running = false;

        if (health.currentHealth < 20) beerShooter.incrementer = 2; // BOSS ENRAGE

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", running);
    }

    bool GroundCheck() {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckRadius, whatIsGround);
        return isGrounded;
    }   

    public GameObject bullet; //bullet prefab.
    public Transform shootPos; //Shoot from
    public Transform shootPosLeft;
    public float shootPower = 3f; //shooting power
    public Transform target; // target position
    private void Shoot() {
        Vector2 shootFrom = new Vector2(shootPos.position.x, shootPos.position.y);
        if (target.position.x < transform.position.x) {
             shootFrom = new Vector2(shootPosLeft.position.x, shootPosLeft.position.y);
        } 
        if (transform.localScale.x == -1f) //He will move shotposleft to the r.h.s once local scale is changed.
        {
            shootFrom = new Vector2(shootPosLeft.position.x, shootPosLeft.position.y);
            if (target.position.x < transform.position.x)
            {
                shootFrom = new Vector2(shootPos.position.x, shootPos.position.y);
            }

        }
        // If sprite is on left side shoot left - vice versa.
        GameObject newBullet = Instantiate(bullet, shootFrom, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = -1 * (shootFrom - (Vector2)target.position);
    }


    IEnumerator StopAndShoot() {
        Shoot();
        yield return new WaitForSeconds(1);
        Shoot();
        yield return new WaitForSeconds(1);
        Shoot();
        yield return new WaitForSeconds(1);
        Shoot();
        yield return new WaitForSeconds(1);
        Shoot();
        yield return new WaitForSeconds(1);
    }

}
