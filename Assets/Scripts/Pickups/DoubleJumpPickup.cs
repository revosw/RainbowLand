using Player;
using UnityEngine;

namespace Pickups
{
    public class DoubleJumpPickup : Pickup
    {
        public override void DoOnTrigger(Collider2D other)
        {
            if (other.CompareTag("player"))
            {
                // Debug.Log();
                other.gameObject.GetComponent<PlayerController>().numberOfJumpsRemaining = 1;
            }
            
            base.DoOnTrigger(other);
        }
    }
}