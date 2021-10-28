using Player;
using UnityEngine;

namespace Pickups
{
    public class DoubleJumpPickup : Pickup
    {
        public override void DoOnTrigger(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Debug.Log();
                other.gameObject.GetComponent<PlayerController>().maxExtraJumps = 1;
            }
            
            base.DoOnTrigger(other);
        }
    }
}