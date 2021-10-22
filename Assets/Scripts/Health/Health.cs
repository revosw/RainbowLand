using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }

    private void Awake() {
        currentHealth = startingHealth;
    }

    public void TakeDamage(float _damage) {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0) {
            // take damage animation
        } else {
            // DEAD
            GetComponent<PlayerMovement>().enabled = false;
            //animator for death
        }
    }

    public void Heal(float _heal) {
        if (currentHealth < 3) {
            currentHealth += _heal;
        }
    }

    private void Update() {

    }

}
