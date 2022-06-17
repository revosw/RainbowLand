using System;
using UnityEngine;

namespace Pickups
{
    public class Pickup : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            DoOnTrigger(other);
        }

        public virtual void DoOnTrigger(Collider2D other)
        { 

            if (other.CompareTag("player"))
            {
                //Debug.Log("PICKUP!");
                gameObject.SetActive(false);
                // enabled = false;
                // Destroy(this.gameObject);
            }
        }
         
    }
} 