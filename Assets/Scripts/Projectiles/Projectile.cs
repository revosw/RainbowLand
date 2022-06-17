using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public float speed;
        public int damage;
        private Rigidbody2D rb;
        public float lifeTime;
        private float spawnTime;
        public float timeAlive;

        
        // todo: This whole class needs a rethink.
        // todo: Rewrite as an interface (?), and implement playerProjectile based on this.
        // Implementation doesn't feel 'nice' during play,
        // and  it's coupled too closely to the player projectile functionality.
        private void Awake()
        {
            // collider = GetComponent<Collider2D>();
            
            rb = GetComponent<Rigidbody2D>();
            spawnTime = Time.time;
            timeAlive = 0;
            Destroy(gameObject, lifeTime);
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // timeAlive += Time.fixedDeltaTime;
            // if (timeAlive > lifeTime)
            // {
            //  Destroy(rb);   
            // }

        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("MoneyHurt") || other.CompareTag("HurtBox"))
            {
                other.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            else if (other.CompareTag("Enemy"))
            {
                other.GetComponent<BasicEnemyHealth>().TakeDamage(damage);
                // gameObject.SetActive(false);
                Destroy(gameObject);

            } else if (other.CompareTag("Ground"))
            {
                Destroy(gameObject);
                // gameObject.SetActive(false);
            }
  
        }

        public void setDirection(float sign)
        {
            // speed = speed;
        }

        public void activate(float direction)
        {
            gameObject.SetActive(true);
            rb.velocity = new Vector2(speed*direction, 0);
        }
    }
}