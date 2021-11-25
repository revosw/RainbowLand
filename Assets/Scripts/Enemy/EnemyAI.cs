using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public Transform enemyGFX;
    public Animator animator;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public int bounceForce;
    Path path;
    int currentWaypoint = 0;

    Seeker seeker;
    Rigidbody2D rb;

    float jumpCD = 5f;
    private float nextJump;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    //Calculates path when target is within range.
    void UpdatePath() {
        print("Update path");
        if (Math.Abs(target.position.x - rb.position.x) < 20
            && Math.Abs(target.position.y - rb.position.y) < 10) {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, target.position, Onpathcomplete);
        }
    }

    void Onpathcomplete(Path p) {
        print("Path complete");
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        print("fixed update");
        if ( path == null) 
            return;
        if(currentWaypoint >= path.vectorPath.Count) {
            return;
        } 

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance) {
            currentWaypoint++;
        }

        if (rb.velocity.x >= .01f) {
            enemyGFX.localScale = new Vector3(10f, 10f, 1f);
        } else if (rb.velocity.x <= -.01f) {
            enemyGFX.localScale = new Vector3(-10f, 10f, 1f);
        }

        //The boss jumping script
        if (Math.Abs(target.position.x - rb.position.x) < 4
            && Math.Abs(target.position.y - rb.position.y) < 6
            && Time.time > nextJump) {
            nextJump = Time.time + jumpCD;
            rb.AddForce(transform.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
