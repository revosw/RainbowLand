using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit!");
        if (collision.tag == "player") {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
