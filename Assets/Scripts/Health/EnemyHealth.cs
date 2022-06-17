using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int enemyHealth;

    public int currentHealth { private set; get; }

    public Animator animator;
    float Timer = 0f;
    public SpriteRenderer spriteRenderer;
    public GameObject deathEffect;

    Color red = Color.red;
    Color white = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = enemyHealth;    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            DeathEffect();
            Destroy(transform.parent.gameObject);
        }
        if (animator.GetBool("takeDamage")) {
            Timer += Time.deltaTime;
            if(Timer >= 0.3f) spriteRenderer.color = white;
            if (Timer >= 1f) {
                animator.SetBool("takeDamage", false);
                Timer = 0f;
            }
        }
        if (spriteRenderer.color == red)
        {
            Timer += Time.deltaTime;
            if (Timer >= 0.3f) spriteRenderer.color = white;
        }
    }
    
    public void TakeDamage(int _damage) {
        currentHealth -= _damage;
        animator.SetBool("takeDamage", true);
        spriteRenderer.color = red;
    }

    private void DeathEffect() {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 4f);
        }
    }

    public void FullHeal()
    {
        currentHealth = enemyHealth;
    } 
}
