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
        StartCoroutine(CountDownDestroyObject());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player") {
            collision.GetComponent<Health>().TakeDamage(damage);
            Debug.Log("Player hit!");
            Destroy(gameObject);
        }
    }

    IEnumerator CountDownDestroyObject()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
