using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public float speed;
        public int bounces;
        private int remainingBounces;
        private bool hit;

        private BoxCollider2D boxCollider;
        private Rigidbody2D rb;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (remainingBounces <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                remainingBounces--;
    
            }
        }

        public void setDirection(float sign)
        {
            speed = speed;
        }

        public void activate(float direction)
        {
            remainingBounces = bounces; 
            gameObject.SetActive(true);
            rb.velocity = new Vector2(speed*direction, 0);
        }
    }
}