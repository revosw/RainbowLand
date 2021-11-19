using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDealDamage : MonoBehaviour
{

    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision) {
        //todo: remove hardcode tag
        if (collision.tag == "player") {
            collision.GetComponent<Health>().TakeDamage(damage);
        } else {

        }
    }
}
