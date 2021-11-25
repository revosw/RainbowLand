using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] public float healAmount;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("player")) {
            collision.GetComponent<Health>().Heal(healAmount);
            this.gameObject.SetActive(false);
        }
    }
}
