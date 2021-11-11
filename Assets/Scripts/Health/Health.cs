using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Rigidbody2D rb;
    public Animator animator;
    private GameMaster gm;

    void Start() {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPosition;
    }

    private void Awake() {
        currentHealth = startingHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            Vector3 force = transform.position - collision.transform.position;
            force = force.normalized;
            //fix this
            gameObject.GetComponent<Rigidbody2D>().AddForce(force * 3000);
            }
    }

    public void TakeDamage(float _damage) {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0) {
            // take damage animation
            animator.SetTrigger("takeDamage");
        } else {
            // DEAD
            animator.SetBool("isDead", true);
            GetComponent<PlayerMovement>().enabled = false;
            //animator for death
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
