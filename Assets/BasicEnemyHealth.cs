using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyHealth : MonoBehaviour
{

    public int enemyHealth;
    private int currentHealth;
     
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = enemyHealth;    
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            DeathEffect();
            Destroy(gameObject);
        }
    }
    

    public void TakeDamage(int _damage) {
        currentHealth -= _damage;
    }

    private void DeathEffect() {
    }
}