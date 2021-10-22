using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
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
            Destroy(transform.parent.gameObject);
    }

    public void TakeDamage(int _damage) {
        currentHealth -= _damage;
    }
}
