using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomper : MonoBehaviour
{

    public int damageToDeal;
    private Rigidbody2D rb;
    public float bounceForce;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "HurtBox") {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damageToDeal);
            rb.AddForce(transform.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
