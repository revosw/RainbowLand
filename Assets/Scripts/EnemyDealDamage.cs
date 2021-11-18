using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDealDamage : MonoBehaviour
{

    [SerializeField] private float damage;

    float dealDamageCD = 0.5f;
    private float nextDamageTimer;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "player" && Time.time > nextDamageTimer) {
            collision.GetComponent<Health>().TakeDamage(damage);
            nextDamageTimer = Time.time + dealDamageCD;
        }
        else {

        }
    }
}
