using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int enemyHealth;
    private int currentHealth;
    public Animator animator;
    float Timer = 0f;

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
        Timer += Time.deltaTime;
        animator.SetBool("takeDamage", true);
        if (Timer >= 2f) {
            animator.SetBool("takeDamage", false);
            Timer = 0f;
        }

    //animation for taking damage
}
}
