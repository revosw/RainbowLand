using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3[] positions;
    private int index;

    //jump related
    public Rigidbody2D rb2d;
    float jumpCD = 4f;
    private float nextJump;
    public int bounceForce;

    //Double Jump Related
    private bool isGrounded;
    const float groundCheckRadius = 0.2f;
    public Transform groundCheckCollider;
    public LayerMask groundLayer;

    public Player.PlayerController playercontroller;

    private void OnDestroy()
    {
        playercontroller.maxExtraJumps = 1; // Double jump activated.
    }

    bool GroundCheck() {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0) {
            isGrounded = true;
        }
        return isGrounded;
    }

    // Start is called before the first frame update
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckCollider.position, groundCheckRadius, groundLayer);

        transform.position = Vector2.MoveTowards(transform.position, positions[index], Time.deltaTime * speed);
        //rb2d.MovePosition(positions[index] * Time.fixedDeltaTime*speed);
        
        if (transform.position == positions[index]) {
            if (index == positions.Length - 1) {
                index = 0;
            } else {
                index++;
            }
        }
    }

    IEnumerator bedDouble() {
        yield return new WaitForSeconds(1);
        rb2d.AddForce(transform.up * (bounceForce+3), ForceMode2D.Impulse);
    }

    private void FixedUpdate() {
        if (Time.time > nextJump && GroundCheck()) {
            nextJump = Time.time + jumpCD;
            rb2d.AddForce(transform.up * bounceForce, ForceMode2D.Impulse);
            StartCoroutine(bedDouble());
        }
    }

}
