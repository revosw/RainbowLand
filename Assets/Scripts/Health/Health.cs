using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth = 3;
    public float maxHealth;
    public float currentHealth { get; private set; }
    private PlayerController _playerController;
    public Animator animator;

    public int deathTimer;

    void Start()
    {
        transform.position = GameMaster.lastCheckPointPosition;
    }

    private void Awake()
    {
        maxHealth = startingHealth;
        currentHealth = maxHealth;
        _playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Vector3 force = transform.position - collision.transform.position;
            force = force.normalized;
            //fix this
            gameObject.GetComponent<Rigidbody2D>().AddForce(force * 300);
        }
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            // take damage animation
            animator.SetTrigger("takeDamage");
        }
        else
        {
            // DEAD
            FindObjectOfType<EnemyHealth>().FullHeal();
            _playerController.OnDeath();
            // StartCoroutine(WaitForAnimationSeconds(deathTimer));
        }
    }



    public void Heal(float _heal)
    {
        if (currentHealth < 3)
        {
            currentHealth += _heal;
        }
    }

    public void FullHeal()
    {
        currentHealth = maxHealth;
    }
}